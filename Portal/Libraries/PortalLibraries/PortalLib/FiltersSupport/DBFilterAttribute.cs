using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.FiltersSupport
{
    public class DBFilterFieldAttribute : Attribute
    {
        #region [ Constructors ]

        public DBFilterFieldAttribute()
        {
            m_IsObjectProperty = true;
        }

        public DBFilterFieldAttribute(string fieldName)
            : this(fieldName, string.Empty)
        {}

        public DBFilterFieldAttribute(string fieldName, object defaultValue)
        {
            m_FieldName = fieldName;
            m_DefaultValue = defaultValue;
        }

        #endregion

        #region [ Fields ]

        private string m_FieldName = string.Empty;
        private string m_Operator = string.Empty;
        private object m_DefaultValue = null;
        private bool m_IsObjectProperty = false;

        #endregion

        #region [ Properties ]

        public bool IsObjectProperty
        {
            get { return m_IsObjectProperty; }
            set { m_IsObjectProperty = value; }
        }

        public string FieldName
        {
            get { return m_FieldName; }
        }

        public string Operator
        {
            get { return m_Operator; }
            set { m_Operator = value; }
        }

        public object DefaultValue
        {
            get { return m_DefaultValue; }
        }

        #endregion
    }

    public class DBFilterTableAttribute : DBFilterFieldAttribute
    {
        #region [ Constructors ]

        public DBFilterTableAttribute(string fieldName, string fromTableName
                                    , string selectFieldName, string selectFilterFieldName)
            : base (fieldName)
        {
            m_FromTableName = fromTableName;
            m_selectFieldName = selectFieldName;
            m_selectFilterFieldName = selectFilterFieldName;
        }

        #endregion

        #region Fields

        private string m_FromTableName = string.Empty;
        private string m_selectFieldName = string.Empty;
        private string m_selectFilterFieldName = string.Empty;

        #endregion

        #region Properties

        public string FromTableName
        {
            get { return m_FromTableName; }
        }

        public string SelectFieldName
        {
            get { return m_selectFieldName; }
        }

        public string SelectFilterFieldName
        {
            get { return m_selectFilterFieldName; }
        }

        #endregion
    }
}