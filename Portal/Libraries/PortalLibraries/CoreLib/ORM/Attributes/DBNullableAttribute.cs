using System;

namespace Core.ORM.Attributes
{
    /// <summary>
    /// Атрибут, позволяющий для свойства объекта указать, является ли поле Nullable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DBNullableAttribute : Attribute
    {
        #region [ Fields ]
        
        private bool m_allowNulls = true;

        #endregion

        #region [ Constructors]

        public DBNullableAttribute() : this(true)
        {}

        public DBNullableAttribute(bool allowNulls)
        {
            m_allowNulls = allowNulls;
        }

        #endregion

        #region [Properties ]

        /// <summary>
        /// Является ли поле nullable
        /// </summary>
        public bool AllowNulls
        {
            get { return m_allowNulls; }
        }

        #endregion
    }
}
