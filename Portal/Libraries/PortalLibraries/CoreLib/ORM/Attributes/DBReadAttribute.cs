using System;

namespace Core.ORM.Attributes
{
    /// <summary>
    /// Атрибут, позволяющий для свойства объекта указать имя поля записи БД, 
    /// из которого можно прочитать его значение.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DBReadAttribute : Attribute
    {
        #region [ Fields ]

        private string m_FieldName = string.Empty;
        private bool m_PrimaryKey = false;

        #endregion

        #region [ Constructors ]

        public DBReadAttribute(string fieldName)
        {
            m_FieldName = fieldName;
        }

        public DBReadAttribute(string fieldName, bool primaryKey)
        {
            m_FieldName = fieldName;
            m_PrimaryKey = primaryKey;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Имя поля.
        /// </summary>
        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        /// <summary>
        /// Является ли поле первичным ключом
        /// </summary>
        public bool PrimaryKey
        {
            get { return m_PrimaryKey; }
            set { m_PrimaryKey = value; }
        }

        #endregion
    }
}