using System;
using System.Collections.Generic;
using System.Reflection;

using Core.Exceptions;
using Core.Dictionaries;
using Core.ORM.Attributes;

namespace Core.ORM
{
    public static class DBAttributesManager
    {
        #region Методы для работы с атрибуами маппинга

        /// <summary>
        /// Возвращает ссылку на атрибут DBNullable, или null, если его нет.
        /// </summary>
        /// <param name="prop">Свойство.</param>
        /// <returns></returns>
        public static DBNullableAttribute GetDBNullableAttribute(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(typeof(DBNullableAttribute), true);
            return attrs.Length > 0 ? (DBNullableAttribute)attrs[0] : null;
        }

        /// <summary>
        /// Указывает, содержит свойство аттрибут DBReadAttribute или нет.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>True, если содержит, иначе False.</returns>
        public static bool HasDBReadAttribute(PropertyInfo property)
        {
            return (GetDBReadAttribute(property) != null);
        }

        /// <summary>
        /// Возвращает ссылку на атрибут DBRead, или null, если его нет.
        /// </summary>
        /// <param name="prop">Свойство.</param>
        /// <returns></returns>
        public static DBReadAttribute GetDBReadAttribute(PropertyInfo prop)
        {
            var attrs = prop.GetCustomAttributes(typeof(DBReadAttribute), true);
            return attrs.Length > 0 ? (DBReadAttribute)attrs[0] : null;
        }

        /// <summary>
        /// Указывает, содержит свойство аттрибут primary key или нет.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>True, если содержит, иначе False.</returns>
        public static bool HasPrimaryKeyAttribute(PropertyInfo property)
        {
            DBReadAttribute attr = GetDBReadAttribute(property);
            return (attr != null && attr.PrimaryKey ? true : false);
        }

        /// <summary>
        /// Указывает, содержит свойство аттрибут DBReadOnlyAttribute или нет.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>True, если содержит, иначе False.</returns>
        public static bool HasDBReadOnlyAttribute(PropertyInfo property)
        {
            return (GetDBReadOnlyAttribute(property) != null);
        }

        /// <summary>
        /// Возвращает DBReadOnlyAttribute аттрибут для свойства.
        /// </summary>
        /// <param name="prop">Свойство.</param>
        /// <returns>Аттрибут, если содержит, иначе Null.</returns>
        public static DBReadOnlyAttribute GetDBReadOnlyAttribute(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(typeof(DBReadOnlyAttribute), true);
            return attrs.Length > 0 ? (DBReadOnlyAttribute)attrs[0] : null;
        }

        /// <summary>
        /// Указывает, содержит свойство аттрибут DictionaryLinkAttribute или нет.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>True, если содержит, иначе False.</returns>
        public static bool HasDictionaryLinkAttribute(PropertyInfo property)
        {
            return (GetDictionaryLinkAttribute(property) != null);
        }

        /// <summary>
        /// Возвращает ссылку на аттрибут DictionaryLink или null, если его нет.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>Аттрибут, если найден, иначе null.</returns>
        public static DictionaryLinkAttribute GetDictionaryLinkAttribute(PropertyInfo property)
        {
            object[] attrs = property.GetCustomAttributes(typeof(DictionaryLinkAttribute), true);
            return (attrs.Length > 0 ? (DictionaryLinkAttribute)attrs[0] : null);
        }

        /// <summary>
        /// Указывает, содержит тип DBTable аттрибут ил нет.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>True, если содержит, иначе False.</returns>
        public static bool HasDBTableAttribute(Type type)
        {
            return (GetDBTableAttribute(type) != null);
        }

        /// <summary>
        /// Возвращает ссылку на атрибут DBTable, или null, если его нет.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns></returns>
        public static DBTableAttribute GetDBTableAttribute(Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeof(DBTableAttribute), false);
            return attrs.Length > 0 ? (DBTableAttribute)attrs[0] : null;
        }

        /// <summary>
        /// Возвращает название таблицы из атрибута DBTable, которым помечен данный класс.
        /// </summary>
        /// <returns>Возвращает имя таблицы из атрибута DBTable. 
        /// Если атрибут не указан, то генерируется исключение.</returns>
        public static string GetDBTableName(Type type)
        {
            DBTableAttribute dbTableAttribute = GetDBTableAttribute(type);

            if (dbTableAttribute == null)
            {
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("DBTableAttributeException", type.FullName));
            }

            return dbTableAttribute.TableName;
        }

        /// <summary>
        /// Возвращает коллекцию атрибутов DBReadAttribute, которыми помечены свойства заданного типа.
        /// Для каждого свойства возвращается не больше одного атрибута.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>Коллекция атрибутов DBReadAttribute.</returns>
        public static IEnumerable<DBReadAttribute> GetDBReadAttributes(Type type)
        {
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                DBReadAttribute dbReadAttribute = GetDBReadAttribute(pi);
                if (dbReadAttribute != null)
                    yield return dbReadAttribute;
            }
        }

        #endregion
    }
}