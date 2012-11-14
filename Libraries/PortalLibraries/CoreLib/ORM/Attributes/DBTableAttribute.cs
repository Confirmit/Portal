using System;

namespace Core.ORM.Attributes
{
    /// <summary>
    /// Атрибут, позволяющий указать для класса соответствующую ему таблицу в БД.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DBTableAttribute : Attribute
    {
        #region  [ Fields ]

        private string m_tableName = string.Empty;
        private bool m_UseInherits = false;

        #endregion

        #region [Constructors ]

        public DBTableAttribute(string tableName, bool useInherits)
            : this(tableName)
        {
            m_UseInherits = useInherits;
        }

        public DBTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        #endregion

        #region [ Properties ]

        public string TableName
        {
            get { return m_tableName; }
            set { m_tableName = value; }
        }

        public bool UseInherits
        {
            get { return m_UseInherits; }
        }

        #endregion
    }
}