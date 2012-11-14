using System;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using Core.ORM.Attributes;
using Core.Exceptions;

namespace Core.ORM
{
    #region [ Helpers ]

    public class FieldData
    {
        #region [ Fields ]
        
        private string m_FieldName = string.Empty;
        private Type m_Type = null;
        private object m_Value = null;
        
        #endregion

        #region [ Constructors ]

        public FieldData(string fieldName, Type type, object value)
        {
            m_FieldName = fieldName;
            m_Type = type;
            m_Value = value;
        }

        #endregion

        #region [ Properties ]

        public string FieldName 
        {
            get { return m_FieldName; }
        }

        public Type ObjectType
        {
            get { return m_Type; }
        }

        public object ObjectValue
        {
            get { return m_Value; }
        }

        #endregion
    }

    public class InhertitsTypeComparer : IComparer<Type>
    {
        #region IComparer<Type> Members

        public int Compare(Type x, Type y)
        {
            if (x.Equals(y))
                return 0;

            return x.IsSubclassOf(y) ? 1 : -1;
        }

        #endregion
    }

    #endregion

    public static class ObjectPropertiesMapper
    {
        #region Методы для работы со свойствами

        /// <summary>
        /// Возвращает свойство с аттрибутом DBRead по имени свойства.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="name">Имя свойства.</param>
        /// <returns>Возвращает свойство, если оно найдено, иначе null.</returns>
        public static PropertyInfo GetDBReadPropertyByName(Type type, string name)
        {
            PropertyInfo prop = type.GetProperty(name);
            if (prop != null && !DBAttributesManager.HasDBReadAttribute(prop))
                prop = null;

            return prop;
        }

        /// <summary>
        /// Возвращает массив свойств, у которых установлен аттрибут DBRead.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>PropertyInfo[]. В случае неудачи, возвращается пустой массив.</returns>
        public static List<PropertyInfo> GetDBReadProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (DBAttributesManager.HasDBReadAttribute(property))
                    result.Add(property);
            }

            return result;
        }

        /// <summary>
        /// Устанавливает значение свойства
        /// </summary>
        /// <param name="prop">информация о свойстве</param>
        /// <param name="obj">объект, который содержит данное свойство</param>
        /// <param name="value">значение</param>
        internal static void SetPropertyValue(PropertyInfo prop, object obj, object value)
        {
            if (value == DBNull.Value)
            {
                // смотрим, является ли поле Nullable
                DBNullableAttribute dbNullableAttribute = DBAttributesManager.GetDBNullableAttribute(prop);
                if (dbNullableAttribute != null && dbNullableAttribute.AllowNulls)
                {
                    // если поле является nullable, 
                    // то в случае если оно содержит опрелделенное значение, возвращаем NULL
                    if (prop.PropertyType == typeof(string))
                    {
                        //value = NullValue.NullString;
                        value = string.Empty;
                    }
                    else
                        value = null;
                }
                else
                {
                    // если из базы пришел Null, а свойство не помечено атрибутом DBNullable,
                    // то генерируем исключение
                    throw new CoreNullReferenceException(Resources.ResourceManager.GetString("DBNullableAttributeException", prop.Name));
                }
            }

            prop.SetValue(obj, value, null);
        }

        /// <summary>
        /// Возвращает значение свойства
        /// </summary>
        /// <param name="prop">свойство</param>
        /// <param name="obj">объект, чье свойство обрабатывается</param>
        /// <returns>Значение свойства с учетом интергации с БД</returns>
        internal static object GetPropertyValue(PropertyInfo prop, object obj)
        {
            object value = prop.GetValue(obj, null);

            // смотрим, является ли поле Nullable
            DBNullableAttribute dbNullableAttribute = DBAttributesManager.GetDBNullableAttribute(prop);
            if (dbNullableAttribute != null && dbNullableAttribute.AllowNulls)
            {
                // если поле является nullable, 
                // то в случае если оно содержит опрелделенное значение, возвращаем NULL
                if (prop.PropertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty((string)value))
                        value = DBNull.Value;
                }
            }

            if (value == null)
                value = DBNull.Value;

            return value;
        }

        /// <summary>
        /// Возвращает имя поля, соответствующее имени свойства.
        /// Имя берется из атрибута DBread. Если атрибута не найден, генерируется исключение.
        /// Для свойства типа MLString вовращается полное имя поля с постфиксом языка.
        /// </summary>
        /// <param name="prop">Свойство.</param>
        /// <returns></returns>
        public static string GetFieldNameByProperty(PropertyInfo prop)
        {
            // получаем атрибуты этого свойства
            DBReadAttribute dbRead = DBAttributesManager.GetDBReadAttribute(prop);

            if (dbRead == null)
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("PropertyDBReadAttributeException", prop.Name));

            string fieldName;
            // формируем имя поля
            if (prop.PropertyType == typeof(MLString))
            {
                if (CultureManager.CurrentLanguage == CultureManager.Languages.Russian)
                    fieldName = dbRead.FieldName + ObjectMapper.RussianEnding;
                else
                    fieldName = dbRead.FieldName + ObjectMapper.EnglishEnding;
            }
            else
                fieldName = dbRead.FieldName;

            return fieldName;
        }

        /// <summary>
        /// Заполняет свойства объекта из записи БД.
        /// Свойства объекта, которые нужно заполнить, должны быть помечены атрибутом DBReadAttribute.
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="row">запись БД</param>
        public static void FillObjectFromRow(object obj, DataRow row)
        {
            // получаем тип объекта
            Type objType = obj.GetType();

            // перебираем его свойства (публичные нестатические)
            foreach (PropertyInfo prop in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DBReadAttribute dbReadAttribute = DBAttributesManager.GetDBReadAttribute(prop);
                if (dbReadAttribute == null)
                    continue;

                string fieldName = dbReadAttribute.FieldName;
                // пытаемся прочитать значение из записи и записать его в свойство
                if (prop.PropertyType == typeof(MLString))
                {
                    string valRU = string.Empty;
                    string valEN = string.Empty;
                    // если свойство является MultyLanguageString, то читаем два значения из записи.
                    // Field_ru - русское значение
                    if (row[fieldName + ObjectMapper.RussianEnding] != DBNull.Value)
                        valRU = (string)row[fieldName + ObjectMapper.RussianEnding];

                    // Field_en - английское значение
                    if (row[fieldName + ObjectMapper.EnglishEnding] != DBNull.Value)
                        valEN = (string)row[fieldName + ObjectMapper.EnglishEnding];

                    SetPropertyValue(prop, obj, new MLString(valRU, valEN));
                }

                if (prop.PropertyType == typeof(MLText))
                {
                    string xmlPresentation = string.Empty;
                    if (row[fieldName] == DBNull.Value)
                        xmlPresentation = null;
                    else
                        xmlPresentation = (string)row[fieldName];

                    MLText mlt = new MLText();
                    mlt.LoadFromXML(xmlPresentation);
                    SetPropertyValue(prop, obj, mlt);
                }
                else
                    if (prop.PropertyType == typeof(TNKBPIdentifier))
                    {
                        //  если свойство является TNKBPIdentifier, то устанавливаем особым образом.
                        TNKBPIdentifier value = new TNKBPIdentifier((string)row[fieldName]);
                        SetPropertyValue(prop, obj, value);
                    }
                    else
                    {
                        // иначе просто присваиваем свойству значение поля
                        SetPropertyValue(prop, obj, row[fieldName]);
                    }
            }
        }

        /// <summary>
        /// Возвращает массивы имен столбцов в БД и соответствующих значений объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="withPrimaryKey">Включать ли столбцы, входящие в первичный ключ.</param>
        /// <param name="withDBReadOnlyColumns">Включать ли столбцы только для чтения из базы.</param>
        public static Dictionary<string, List<FieldData>> GetDBColumnsValues(object obj, bool withPrimaryKey, bool withDBReadOnlyColumns)
        {
            Type objType = obj.GetType();
            Dictionary<Type, List<FieldData>> mappingData = new Dictionary<Type, List<FieldData>>();

            foreach (PropertyInfo prop in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DBReadAttribute dbReadAttribute = DBAttributesManager.GetDBReadAttribute(prop);
                if (dbReadAttribute != null && (!dbReadAttribute.PrimaryKey || dbReadAttribute.PrimaryKey && withPrimaryKey))
                {
                    if (DBAttributesManager.HasDBReadOnlyAttribute(prop) && !withDBReadOnlyColumns)
                        continue;

                    Type tableType = GetTableTypeForProperty(objType, prop);
                    if (!mappingData.ContainsKey(tableType))
                        mappingData.Add(tableType, new List<FieldData>());

                    if (prop.PropertyType == typeof(MLString))
                    {
                        MLString value = (MLString)GetPropertyValue(prop, obj);

                        mappingData[tableType].Add(new FieldData(dbReadAttribute.FieldName + ObjectMapper.RussianEnding
                                                            , prop.PropertyType, value[CultureManager.Languages.Russian]));

                        mappingData[tableType].Add(new FieldData(dbReadAttribute.FieldName + ObjectMapper.EnglishEnding
                                    , prop.PropertyType, value[CultureManager.Languages.English]));
                    }

                    if (prop.PropertyType == typeof(MLText))
                    {
                        MLText value = (MLText)GetPropertyValue(prop, obj);
                        mappingData[tableType].Add(new FieldData(dbReadAttribute.FieldName, prop.PropertyType, value.ToXMLString()));
                    }
                    else if (prop.PropertyType == typeof(TNKBPIdentifier))
                    {
                        TNKBPIdentifier value = (TNKBPIdentifier)GetPropertyValue(prop, obj);
                        mappingData[tableType].Add(new FieldData(dbReadAttribute.FieldName, prop.PropertyType, value.ToString()));
                    }
                    else // свойства всех остальных типов просто передаем в параметр
                        mappingData[tableType].Add(new FieldData(dbReadAttribute.FieldName, prop.PropertyType, GetPropertyValue(prop, obj)));
                }
            }

            var coll = mappingData.OrderBy(elem => elem.Key, new InhertitsTypeComparer());
            var result = coll.Select(elem => new { TableName = DBAttributesManager.GetDBTableName(elem.Key), FieldNames = elem.Value });

            return result.ToDictionary(elem => elem.TableName, elem => elem.FieldNames);
        }

        /// <summary>
        /// Get sorting list of using Tables for current Type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="withPrimaryKey"></param>
        /// <returns></returns>
        public static List<string> GetObjectMappingDBTableData(Type type, bool withPrimaryKey)
        {
            Dictionary<string, List<string>> dict = GetObjectMappingData(type, withPrimaryKey);
            return dict.Select(elem => elem.Key).ToList();
        }

        /// <summary>
        /// Возвращает массивы имен столбцов в БД.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="withPrimaryKey">Включать ли столбцы, входящие в первичный ключ.</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetObjectMappingData(Type type, bool withPrimaryKey)
        {
            Dictionary<Type, List<string>> columnNamesList = new Dictionary<Type, List<string>>();
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DBReadAttribute dbReadAttribute = DBAttributesManager.GetDBReadAttribute(prop);
                if (dbReadAttribute != null && (!dbReadAttribute.PrimaryKey || dbReadAttribute.PrimaryKey && withPrimaryKey))
                {
                    Type tableType = GetTableTypeForProperty(type, prop);
                    if (!columnNamesList.ContainsKey(tableType))
                        columnNamesList.Add(tableType, new List<string>());

                    if (prop.PropertyType == typeof(MLString))
                    {
                        columnNamesList[tableType].Add(dbReadAttribute.FieldName + ObjectMapper.RussianEnding);
                        columnNamesList[tableType].Add(dbReadAttribute.FieldName + ObjectMapper.EnglishEnding);
                    }
                    else
                        columnNamesList[tableType].Add(dbReadAttribute.FieldName);
                }
            }

            var coll = columnNamesList.OrderBy(elem => elem.Key, new InhertitsTypeComparer());
            var result = coll.Select(elem => new { TableName = DBAttributesManager.GetDBTableName(elem.Key), FieldNames = elem.Value });

            return result.ToDictionary(elem => elem.TableName, elem => elem.FieldNames);
        }

        public static Type GetTableTypeForProperty(Type objectType, PropertyInfo propInfo)
        {
            DBTableAttribute dbTableAttribute = DBAttributesManager.GetDBTableAttribute(objectType);
            if (dbTableAttribute.UseInherits)
            {
                DBTableAttribute declTableAttr = DBAttributesManager.GetDBTableAttribute(propInfo.DeclaringType);
                if (declTableAttr != null)
                    return propInfo.DeclaringType;
            }

            return objectType;
        }

        #endregion
    }
}