using System;

namespace UIPProcess.DataValidating
{
    public class DataValidatingAttribute : Attribute
    {
        public DataValidatingAttribute()
        {
        }

        public DataValidatingAttribute(Boolean isClearingMethod)
        {
            _isClearingMethod = isClearingMethod;
        }

        public Boolean IsClearingMethod
        {
            get { return _isClearingMethod; }
        }
        private Boolean _isClearingMethod = false;
    }
}