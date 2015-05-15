using System;

namespace UIPProcess.DataBinding
{
    public class DataBindingRecursiveAttribute : Attribute
    {
        public DataBindingRecursiveAttribute()
        {
        }

        public DataBindingRecursiveAttribute(String dataFieldName)
        {
            _dataFieldName = dataFieldName;
        }

        public String DataFieldName
        {
            get { return _dataFieldName; }
        }
        private String _dataFieldName = String.Empty;
    }
}