//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// ControllerFactory.cs
//
// This file contains the implementations of the ControllerFactory class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;
using System.Collections.Generic;
using System.Xml;

namespace UIProcess
{
    public class ControllerSettings : ObjectTypeSettings
    {
        #region Declares Variables

        private const string AttributeInterceptor = "interceptor";
        private String _interceptor = String.Empty;

        private IDictionary<String, String> _mapKeysProps =
            new Dictionary<String, String>();

        #endregion
        
        #region Constructor
		/// <summary>
		/// Overloaded. Default constructor.
		/// </summary>
		public ControllerSettings( ) : base( )
		{}

		/// <summary>
		/// Overloaded. Initialized a new instance of the ViewSettings class using the specified configNode.
		/// </summary>
		/// /// <param name="configNode">XmlNode from the configuration file.</param>
        public ControllerSettings(XmlNode configNode, IFormatProvider formatProvider)
            : base(configNode)
		{
            XmlNode currentAttribute;
            currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeInterceptor);
            if (isValidAttribute(currentAttribute))
                _interceptor = currentAttribute.Value;

            loadKeys(configNode);        
        }

        private void loadKeys(XmlNode configNode)
        {
            foreach (XmlNode keyNode in configNode.ChildNodes)
            {
                if (!keyNode.Name.Equals("map-key"))
                    throw new Exception(string.Format("ExceptionInvalidViewSubkey - {0}", Name));

                XmlAttribute attrKey = keyNode.Attributes["key"];
                XmlAttribute attrProperty = keyNode.Attributes["property"];
                if (attrKey == null || attrProperty == null
                 || String.IsNullOrEmpty(attrKey.Value)
                 || String.IsNullOrEmpty(attrProperty.Value))
                {
                    throw new Exception(string.Format("ExceptionConfigIncorrectControllerKeyMapping - {0}.", Name));
                }

                _mapKeysProps.Add(attrKey.Value, attrProperty.Value);
            }
        }


        #endregion

        public String Interceptor
        {
            get { return _interceptor; }
        }

        public IDictionary<String, StateKeySettings> MapPropsKeys
        {
            get
            {
                IDictionary<String, StateKeySettings> mapKeys = new Dictionary<String, StateKeySettings>();

                IDictionary<String, String> mapKeysProps = _mapKeysProps;
                foreach (KeyValuePair<String, String> key_value in mapKeysProps)
                {
                    StateKeySettings keySettings = UIPConfiguration.Config.GetStateKeySettingsFromName(key_value.Key);
                    if (keySettings == null)
                        throw new Exception(string.Format("ExceptionConfigNullStateKey {0} - {1}.", key_value.Key, Name));

                    mapKeys.Add(key_value.Value, keySettings);
                }

                return mapKeys;
            }
        }
    }
}
