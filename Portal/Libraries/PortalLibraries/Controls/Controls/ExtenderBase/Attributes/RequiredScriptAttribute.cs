// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;

namespace Controls.ExtenderBase.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class RequiredScriptAttribute : Attribute
    {
        #region [ Fields ]

        private readonly int _order;
        private readonly Type _extenderType;
        private readonly string _scriptName = string.Empty;

        #endregion

        #region [ Properties ]

        public Type ExtenderType
        {
            get { return _extenderType; }
        }

        public string ScriptName
        {
            get { return _scriptName; }
        }

        public int LoadOrder
        {
            get { return _order; }
        }

        #endregion

        #region [ Constructors ]

        public RequiredScriptAttribute()
        {}

        public RequiredScriptAttribute(string scriptName)            
        {
            _scriptName = scriptName;
        }

        public RequiredScriptAttribute(Type extenderType)
            : this(extenderType, 0) 
        {}

        public RequiredScriptAttribute(Type extenderType, int loadOrder) 
        {
            _extenderType = extenderType;
            _order = loadOrder;
        }

        #endregion

        #region [ Methods ]

        public override bool IsDefaultAttribute()
        {
            return _extenderType == null;
        }

        #endregion
    }
}