using System;

namespace UIPProcess.DataBinding
{
    public class DataBindingAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataFieldName">The name of the field of Business Entity (Domain object) to 
        /// which the data should be binded.</param>
        /// <param name="defaultValue"></param>
        /// <param name="isObjectProperty"></param>
        public DataBindingAttribute(String dataFieldName, Object defaultValue, Boolean isObjectProperty)
        {
            _dataFieldName = dataFieldName;
            _isObjectProperty = isObjectProperty;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFieldName">The name of the field of Business Entity (Domain object) to 
        /// which the data should be binded.</param>
        /// <param name="defaultValue"></param>
        /// <param name="isObjectProperty"></param>
        /// <param name="propType"></param>
        public DataBindingAttribute(String dataFieldName, Object defaultValue, Boolean isObjectProperty, Type propType)
            : this(dataFieldName, defaultValue, isObjectProperty)
        {
            _propType = propType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFieldName">The name of the field of Business Entity (Domain object) to 
        /// which the data should be binded.</param>
        /// <param name="defaultValue"></param>
        /// <param name="isObjectProperty"></param>
        /// <param name="bindOnlyForNotNullEntity"></param>
        public DataBindingAttribute(String dataFieldName, Object defaultValue, Boolean isObjectProperty
                                    , Boolean bindOnlyForNotNullEntity)
            : this(dataFieldName, defaultValue, isObjectProperty)
        {
            _bindOnlyForNotNullEntity = bindOnlyForNotNullEntity;
        }

        /// <summary>
        /// The name of the field of Business Entity (Domain object) to 
        /// which the data should be binded.
        /// </summary>
        public String DataFieldName
        {
            get { return _dataFieldName; }
        }
        private String _dataFieldName = String.Empty;

        public Object DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        private Object _defaultValue = null;

        public int BindingPriority
        {
            get { return _bindingPriority; }
            set { _bindingPriority = value; }
        }
        private int _bindingPriority = 0;

        public Boolean IsObjectProperty
        {
            get { return _isObjectProperty; }
        }
        private Boolean _isObjectProperty = false;

        public Type ObjectPropType
        {
            get { return _propType; }
        }
        Type _propType = null;

        public Boolean BindOnlyForNotNullEntity
        {
            get { return _bindOnlyForNotNullEntity; }
            set { _bindOnlyForNotNullEntity = value; }
        }
        private Boolean _bindOnlyForNotNullEntity = false;
    }
}