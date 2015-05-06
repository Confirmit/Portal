using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using Core.DB;
using Core.DB.QueryStatement;
using Core.Dictionaries;
using Core.Exceptions;
using Core.Resources;
using Core.ORM.Attributes;

namespace Core.ORM
{
    /// <summary>
    /// Класс, позволяющий заполнять свойства объекта
    /// </summary>
    public static class ObjectMapper
    {
        #region [ Статические данные ]

        /// <summary>
        /// Окончание названия полей с русским значением.
        /// </summary>
        public static readonly string RussianEnding = "_ru";

        /// <summary>
        /// Окончание названия полей с английским значением.
        /// </summary>
        public static readonly string EnglishEnding = "_en";

        #endregion

        #region [ Вспомогательные методы генерации SQL-запросов ]

        #region [ GetSelectByIDQueryStatement and GetSelectQueryStatement ]

        /// <summary>
        /// Возвращает текст SELECT-запроса для получения объектов данного типа из БД (без фильтрации).
        /// </summary>
        /// <param name="type">Тип объекта, для которого строится запрос.</param>
        /// <returns>
        /// Текст запроса вида:
        ///		SELECT Field1, Field2 FROM TableName
        /// </returns>
        public static QueryStatement GetSelectByIDQueryStatement(Type type)
        {
            var objectInfo = new Dictionary<string, List<string>>();
            var queryStatement = GetSelectQueryStatement(type, out objectInfo);

            string baseTable = objectInfo.Keys.First();
            var bitWiseOperator = queryStatement.Clauses.Count == 0 ? string.Empty : "AND";
            queryStatement.Clauses.Add(new QueryStatementClause(bitWiseOperator, "=")
                                           {
                                               FieldName = string.Format("[{0}].ID", baseTable),
                                               Value = "@ID"
                                           });
            return queryStatement;
        }

        /// <summary>
        /// Возвращает текст SELECT-запроса для получения объектов данного типа из БД (без фильтрации).
        /// </summary>
        /// <param name="type">Тип объекта, для которого строится запрос.</param>
        /// <returns>
        /// Текст запроса вида:
        ///		SELECT Field1, Field2 FROM TableName
        /// </returns>
        public static QueryStatement GetSelectQueryStatement(Type type)
        {
            var objectMappingData = new Dictionary<string, List<string>>();
            return GetSelectQueryStatement(type, out objectMappingData);
        }

        public static QueryStatement GetSelectQueryStatement(Type type, out Dictionary<string, List<string>> objectMappingData)
        {
            objectMappingData = ObjectPropertiesMapper.GetObjectMappingData(type, true);
            if (objectMappingData.Count == 0)
                return null;

            DBTableAttribute dbTabeAttr = DBAttributesManager.GetDBTableAttribute(type);
            var queryStatement = new QueryStatement(QueryStatementType.SELECT);
            if (dbTabeAttr != null && dbTabeAttr.UseInherits)
            {
                string baseID = string.Format("[{0}].ID", objectMappingData.Keys.First());
                foreach (var tableName in objectMappingData.Keys.Skip(1))
                {
                    var bitWiseOperator = queryStatement.Clauses.Count == 0 ? string.Empty : "AND";
                    queryStatement.Clauses.Add(new QueryStatementClause(bitWiseOperator, "=")
                                                   {
                                                       FieldName = string.Format("[{0}].ID", tableName),
                                                       Value = baseID
                                                   });
                }
            }

            foreach (var keyValuePair in objectMappingData)
            {
                queryStatement.Tables.Add(keyValuePair.Key);
                foreach (var fieldName in keyValuePair.Value)
                {
                    queryStatement.Fields.Add(string.Format("[{0}].{1}", keyValuePair.Key, fieldName));
                }
            }

            return queryStatement;
        }  

        #endregion

        /// <summary>
        /// Возвращает сгенерированный SQL-запрос на добавление данного объекта.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetInsertStatements(object obj, out Dictionary<string, List<FieldData>> objMappingData)
        {
            Dictionary<string, string> statements = new Dictionary<string, string>();
            objMappingData = ObjectPropertiesMapper.GetDBColumnsValues(obj, false, false);
            string baseTableName = objMappingData.Keys.First();
            string ident_current = string.Format("IDENT_CURRENT('{0}')", baseTableName);

            foreach (var tabelData in objMappingData)
            {
                var columns = tabelData.Value.Select(elem => elem.FieldName).ToList();
                var parameters = tabelData.Value.Select(elem => string.Format("@{0}", elem.FieldName)).ToList();

                if (statements.Keys.Count != 0)
                {
                    columns.Insert(0, "ID");
                    parameters.Insert(0, ident_current);
                }

                string query = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2}); SELECT {3} "
                                    , tabelData.Key
                                    , string.Join(", ", columns.ToArray())
                                    , string.Join(", ", parameters.ToArray())
                                    , ident_current);

                statements.Add(tabelData.Key, query);
            }

            return statements;
        }

        /// <summary>
        /// Возвращает сгенерированный SQL-запрос на изменение данного объекта.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetUpdateStatements(object obj, out Dictionary<string, List<FieldData>> objMappingData)
        {
            objMappingData = ObjectPropertiesMapper.GetDBColumnsValues(obj, false, false);
            Dictionary<string, string> statements = new Dictionary<string, string>();

            foreach (var mappData in objMappingData)
            {
                var fieldNameList = mappData.Value.Select(fieldData => string.Format("{0} = @{0}", fieldData.FieldName));
                statements.Add(mappData.Key, string.Format("UPDATE [{0}] SET {1} WHERE ID = @ID;", mappData.Key, string.Join(", ", fieldNameList.ToArray())));
            }

            return statements;
        }

        #region [ GetDeleteStatement ]

        /// <summary>
        /// Возвращает сгенерированный SQL-запрос на изменение данного объекта.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDeleteStatement(Type type)
        {
            List<string> dbTables = ObjectPropertiesMapper.GetObjectMappingDBTableData(type, true);
            string query = string.Empty;

            foreach (string tableName in dbTables)
            {
                query += string.Format("DELETE FROM [{0}] WHERE ID = @ID;", tableName);
            }
            return query;
        }

        #endregion

        #endregion

        #region Методы для работы с параметрами команд
        /// <summary>
        /// Заполняет параметры команды в соответствии со значениями свойств, помеченных DBReadAttribute.
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="obj">Объект</param>
        /// <param name="withPrimaryKey">Добавлять ли параметр, соотв. первичному ключу.</param>
        ///
        /// TODO: Переписать с использованием GetDBColumnsValues
        private static void FillCommandParameters(IBaseCommand command, object obj, bool withPrimaryKey)
        {
            // получаем тип объекта
            Type objType = obj.GetType();
            // перебираем его свойства (публичные нестатические)
            foreach (PropertyInfo prop in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DBReadAttribute dbReadAttribute = DBAttributesManager.GetDBReadAttribute(prop);
                if (dbReadAttribute != null
                        && (!dbReadAttribute.PrimaryKey || dbReadAttribute.PrimaryKey && withPrimaryKey))
                {
                    if (prop.PropertyType == typeof(MLString))
                    {
                        MLString value = (MLString)ObjectPropertiesMapper.GetPropertyValue(prop, obj);
                        command.Add("@" + dbReadAttribute.FieldName + RussianEnding, value[CultureManager.Languages.Russian]);
                        command.Add("@" + dbReadAttribute.FieldName + EnglishEnding, value[CultureManager.Languages.English]);
                    }
                    else
                        if (prop.PropertyType == typeof(TNKBPIdentifier))
                        {
                            TNKBPIdentifier value = (TNKBPIdentifier)ObjectPropertiesMapper.GetPropertyValue(prop, obj);
                            command.Add("@" + dbReadAttribute.FieldName, value.ToString());
                        }
                        else
                        {
                            command.Add("@" + dbReadAttribute.FieldName, ObjectPropertiesMapper.GetPropertyValue(prop, obj));
                        }
                }
            }
        }

        /// <summary>
        /// Заполняет параметры команды на добавление в соответствии со значениями свойств, помеченных DBReadAttribute.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <param name="obj">Объект</param>
        public static void FillInsertCommandParameters(IBaseCommand command, object obj)
        {
            FillCommandParameters(command, obj, false);
        }

        /// <summary>
        /// Заполняет параметры команды на обновление в соответствии со значениями свойств, помеченных DBReadAttribute.
        /// </summary>
        /// <param name="command">Хранимая процедура</param>
        /// <param name="obj">Объект</param>
        public static void FillUpdateCommandParameters(IBaseCommand command, object obj)
        {
            FillCommandParameters(command, obj, true);
        }

        /// <summary>
        /// Устанавливает значения для поля объекта, помеченное как первичный ключ
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="value">Значение первичного ключа</param>
        public static void SetPrimaryKeyPropertyValue(object obj, object value)
        {
            // получаем тип объекта
            Type objType = obj.GetType();
            // перебираем его свойства (публичные нестатические)
            foreach (PropertyInfo prop in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DBReadAttribute dbReadAttribute = DBAttributesManager.GetDBReadAttribute(prop);
                if (dbReadAttribute != null && dbReadAttribute.PrimaryKey)
                    ObjectPropertiesMapper.SetPropertyValue(prop, obj, value);
            }
        }

        #endregion

        #region Вспомогательные методы для работы с командой
        /// <summary>
        /// Возвращает полный текст команды с подставленными параметрами
        /// </summary>
        /// <returns></returns>
        public static string GetCommandFullText(IBaseCommand command)
        {
            var text = new StringBuilder(command.Command.CommandText);
            foreach (IDataParameter prm in command.Command.Parameters)
            {
                if (prm.DbType == DbType.AnsiString ||
                    prm.DbType == DbType.AnsiStringFixedLength ||
                    prm.DbType == DbType.String ||
                    prm.DbType == DbType.StringFixedLength)
                {
                    text.Replace(prm.ParameterName, "'" + prm.Value + "'");
                }
                else
                    text.Replace(prm.ParameterName, prm.Value.ToString());
            }
            return text.ToString();
        }
        #endregion

        #region Методы управления объектом

        /// <summary>
        /// Вставляет объект в БД.
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="spName">Имя хранимой процедуры, отвечающей за добавление объекта.</param>
        public static void InsertObject(object obj, string spName)
        {
            DB.Procedure p = new DB.Procedure(spName);
            FillInsertCommandParameters(p, obj);
            p.AddReturnValueParameter();
            p.ExecNonQuery();
            SetPrimaryKeyPropertyValue(obj, p.GetReturnValue());
        }

        /// <summary>
        /// Обновляет объект в БД.
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="spName">Имя хранимой процедуры, отвечающей за обновление объекта.</param>
        public static void UpdateObject(object obj, string spName)
        {
            DB.Procedure p = new DB.Procedure(spName);
            FillUpdateCommandParameters(p, obj);
            p.ExecNonQuery();
        }

        #endregion

        #region Метод CopyObject()

        /// <summary>
        /// Копирует значения всех полей с аттрибутом DBRead (кроме primary key) из одного
        /// объекта в другой.
        /// </summary>
        /// <param name="copyFrom">Объекто-источник</param>
        /// <param name="copyTo">Объект-назначение</param>
        public static void CopyObject(object copyFrom, object copyTo)
        {
            Type copyFromType = copyFrom.GetType();
            Type copyToType = copyTo.GetType();
            if (copyFromType != copyToType)
                throw new CoreArgumentException(Resources.ResourceManager.GetString("ObjectsTypesException"));

            List<PropertyInfo> properties = ObjectPropertiesMapper.GetDBReadProperties(copyFromType);
            Debug.Assert(properties != null);

            if (properties.Count == 0)
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("ObjectsCopyException"));

            object val;
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name != "ID")
                {
                    val = properties[i].GetValue(copyFrom, null);
                    properties[i].SetValue(copyTo, val, null);
                }
            }
        }

        #endregion

        #region Методы работы со словарями

        /// <summary>
        /// Возвращает отображение связанных словарей в массив свойств данного словаря, связанных с этим словарём.
        /// </summary>
        /// <param name="type">Словарь.</param>
        /// <returns>Отображение.</returns>
        public static Dictionary<IDictionary, PropertyInfo[]> GetLinkedDictionariesAndProperties(Type type)
        {
            Dictionary<IDictionary, PropertyInfo[]> result =
                new Dictionary<IDictionary, PropertyInfo[]>(new DictionaryEqualityComparer());
            Dictionary<IDictionary, Dictionary<string, PropertyInfo>> tmp =
                new Dictionary<IDictionary, Dictionary<string, PropertyInfo>>(new DictionaryEqualityComparer());

            foreach (PropertyInfo prop in ObjectPropertiesMapper.GetDBReadProperties(type))
            {
                DictionaryLinkAttribute attr = DBAttributesManager.GetDictionaryLinkAttribute(prop);
                if (attr != null)
                {
                    // сначала просто заполняем для каждого словаря, связанного с нашим
                    // список свойств нашего словаря, связынных с указанным
                    IDictionary dict = (IDictionary)Activator.CreateInstance(attr.DictionaryLinkType);
                    if (!tmp.ContainsKey(dict))
                    {
                        Dictionary<string, PropertyInfo> propList = new Dictionary<string, PropertyInfo>();
                        propList.Add(attr.PropertyName, prop);

                        tmp.Add(dict, propList);
                    }
                    else
                    {
                        tmp[dict].Add(attr.PropertyName, prop);
                    }
                }
            }

            // теперь для каждого словаря список связанных свойств нашего словаря нужно отсортировать 
            // в соответствии с массивом Keys связанного словаря
            if (tmp.Count > 0)
            {
                foreach (KeyValuePair<IDictionary, Dictionary<string, PropertyInfo>> keyValue in tmp)
                {
                    IDictionary dict = keyValue.Key;
                    Dictionary<string, PropertyInfo> props = keyValue.Value;
                    string[] keys = dict.Keys;

                    if (props.Count != keys.Length)
                    {
                        throw new CoreException(ResourceManager.GetString("WrongKeyValueListException", dict.DictionaryName));
                    }

                    PropertyInfo[] propArray = new PropertyInfo[props.Count];
                    for (int i = 0; i < keys.Length; i++)
                    {
                        propArray[i] = props[keys[i]];
                    }

                    result.Add(dict, propArray);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает коллекцию свойств словаря, на который ссылается данный словарь.
        /// </summary>		
        /// <param name="dictionary">Словарь.</param>
        /// <param name="type">Тип ссылающегося словаря.</param>
        /// <returns>Коллекция свойств.</returns>
        public static PropertyInfo[] GetLinkedProperties(IDictionary dictionary, Type type)
        {
            Dictionary<IDictionary, PropertyInfo[]> dictionaries = GetLinkedDictionariesAndProperties(type);

            PropertyInfo[] properties = null;

            foreach (KeyValuePair<IDictionary, PropertyInfo[]> pair in dictionaries)
            {
                if (pair.Key.DictionaryName == dictionary.DictionaryName)
                {
                    properties = pair.Value;
                }
            }

            return properties;
        }

        #endregion
    }
}