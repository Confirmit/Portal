using System;

namespace Controls.ExtenderBase.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "The composition of baseType and resourceName is available as ResourcePath")]
    public sealed class ClientCssResourceAttribute : Attribute
    {
        #region [ Fields ]

        private readonly string _resourcePath = string.Empty;

        #endregion

        #region [ Properties ]

        public string ResourcePath
        {
            get { return _resourcePath; }
        }

        public int LoadOrder { get; set; }

        #endregion

        #region [ Constructors ]

        public ClientCssResourceAttribute(string fullResourceName)
        {
            if (fullResourceName == null) 
                throw new ArgumentNullException("fullResourceName");

            _resourcePath = fullResourceName;
        }

        public ClientCssResourceAttribute(Type baseType, string resourceName)
        {
            if (baseType == null) 
                throw new ArgumentNullException("baseType");

            if (resourceName == null) 
                throw new ArgumentNullException("resourceName");

            string typeName = baseType.FullName;
            int lastDot = typeName.LastIndexOf('.');
            if (lastDot != -1)
                typeName = typeName.Substring(0, lastDot);

            _resourcePath = typeName + '.' + resourceName;
        }

        #endregion
    }
}