// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;

namespace Controls.ExtenderBase.Attributes
{
    /// <summary>
    /// Allows the mapping of a property declared in managed code to a property
    /// declared in client script.  For example, if the client script property is named "handle" and you
    /// prefer the name on the TargetProperties object to be "Handle", you would apply this attribute with the value "handle."
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ClientPropertyNameAttribute : Attribute
    {
        #region [ Fields ]

        private readonly string _propertyName = string.Empty;

        #endregion

        #region [ Constructors ]

        public ClientPropertyNameAttribute()
        {}

        /// <summary>
        /// Creates an instance of the ClientPropertyNameAttribute and initializes
        /// the PropertyName value.
        /// </summary>
        /// <param name="propertyName">The name of the property in client script that you wish to map to.</param>
        public ClientPropertyNameAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The name of the property in client script code that you wish to map to.
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        #endregion

        #region [ Methods ]

        public override bool IsDefaultAttribute()
        {
            return string.IsNullOrEmpty(PropertyName);
        }

        #endregion
    }
}