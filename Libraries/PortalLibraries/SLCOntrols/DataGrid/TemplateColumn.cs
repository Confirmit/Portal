using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace SLControls.DataGrid
{
    /// <summary>
    /// Class providing additional settings fot DataGridTemplateColumn.
    /// </summary>
    public class TemplateColumn : DataGridTemplateColumn
    {
        /// <summary>
        /// Pproperty of data item will use to get information.
        /// </summary>
        public String BindingObjectValue
        {
            get { return m_BindingObjectValue; }
            set { m_BindingObjectValue = value; }
        }
        private String m_BindingObjectValue = "Text";

        /// <summary>
        /// Converter.
        /// </summary>
        public IValueConverter Converter
        {
            get { return m_Converter; }
            set { m_Converter = value; }            
        }
        private IValueConverter m_Converter;

        /// <summary>
        /// Converter parameter.
        /// </summary>
        public Object ConverterParameter
        {
            get { return m_ConverterParameter; }
            set { m_ConverterParameter = value; }
        }
        private Object m_ConverterParameter;
    }
}
