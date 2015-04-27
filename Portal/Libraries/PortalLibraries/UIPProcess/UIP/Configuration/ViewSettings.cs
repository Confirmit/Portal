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
using System.Configuration;
using System.Xml;
using System.Xml.XPath;

using UIPProcess.UIP.Views;

namespace UIProcess
{
	/// <summary>
	/// The ViewSettings capture the configuration information for a specific view (a view is a class that implements the 
	/// <see cref="IView"/> interface).
	/// </summary>
	public class ViewSettings : ObjectTypeSettings
	{
		#region Declares Variables
		private const string AttributeController = "controller";
		private const string AttributeStayOpen = "stayOpen";        
		private const string AtributeLayoutManager = "layoutManager";

        private const string AttributeOpenModal = "openModal";
        private const string AttributeFloatable = "floatable";
		
        private const string AttributeCanHaveFloatingWindows = "canHaveFloatingWindows";
        private const string AttributeIsAJAXWebService = "isajaxwebservice";
        private string _controller = null;
		private string _layoutManager;
		private bool _canHaveFloatingWindows = false;
		private bool _isFloatable = false;
		private bool _isStayOpen = false;       
		private bool _isOpenModal = false;
        private bool _isajaxwebservice = false;
        private XPathNavigator _navigator;
		private XmlNode _node;

        private IDictionary<String, String> _mapKeysProps = 
            new Dictionary<String, String>();

        #endregion

		#region Constructors

		/// <summary>
		/// Overloaded. Default constructor.
		/// </summary>
		public ViewSettings( ) : base( )
		{
		}

		/// <summary>
		/// Creates an instance of ViewSettings using the specified configNode and IFormatProvider. 
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		/// <param name="formatProvider">The IFormatProvider required for globalization.</param>
		public ViewSettings(XmlNode configNode, IFormatProvider formatProvider) : base(configNode)
		{
			LoadAttributes(configNode, formatProvider);
            loadKeys(configNode);
            _node = configNode;
		}

        private void loadKeys(XmlNode configNode)
        {
            foreach (XmlNode keyNode in configNode.ChildNodes)
            {
                if (!keyNode.Name.Equals("map-key"))
                    throw new Exception(string.Format("ExceptionInvalidViewSubkey - {0}.", Name));

                XmlAttribute attrKey = keyNode.Attributes["key"];
                XmlAttribute attrProperty = keyNode.Attributes["property"];
                if (attrKey == null || attrProperty == null
                 || String.IsNullOrEmpty(attrKey.Value) 
                 || String.IsNullOrEmpty(attrProperty.Value))
                {
                    throw new Exception(string.Format("ExceptionConfigIncorrectKeyViewMapping - {0}.", Name));
                }

                MapPropertyToKey(attrKey.Value, attrProperty.Value);
            }
        }

		private void LoadAttributes(XmlNode configNode, IFormatProvider formatProvider)
		{
		    _isFloatable = getBooleanAttribute(configNode, AttributeFloatable);
		    _canHaveFloatingWindows = getBooleanAttribute(configNode, AttributeCanHaveFloatingWindows);
            if (_isFloatable && _canHaveFloatingWindows)
                throw new ConfigurationErrorsException(
                    string.Format("ExceptionConflictingFloatingWindowsAttribute {0} - {1}.", AttributeFloatable,
                                  AttributeCanHaveFloatingWindows));

			LoadControllerAttribute(configNode);
			LoadStayOpenAttribute(configNode);
			LoadOpenModalAttribute(configNode);
			LoadLayoutManagerAttribute(configNode,formatProvider);
            LoadAjaxWebServiceTypeAttribute(configNode);
			_navigator = configNode.CreateNavigator();
		}

		private void LoadLayoutManagerAttribute(XmlNode configNode, IFormatProvider formatProvider)
		{
			XmlNode currentAttribute;
			currentAttribute = configNode.Attributes.RemoveNamedItem(AtributeLayoutManager);
			if (isValidAttribute(currentAttribute))
				_layoutManager = currentAttribute.Value.ToString(formatProvider);
		}

		private void LoadOpenModalAttribute(XmlNode configNode)
		{
			XmlNode currentAttribute;
			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeOpenModal);
			if (isValidAttribute(currentAttribute))
				_isOpenModal = XmlConvert.ToBoolean(currentAttribute.Value);
		}

		private void LoadStayOpenAttribute(XmlNode configNode)
		{
			XmlNode currentAttribute;
			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeStayOpen);
			if (isValidAttribute(currentAttribute))
				_isStayOpen = XmlConvert.ToBoolean(currentAttribute.Value);
		}

		private void LoadControllerAttribute(XmlNode configNode)
		{
			XmlNode currentAttribute;
			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeController);
            if (currentAttribute == null)
                return;

            if (isValidAttribute(currentAttribute))
                _controller = currentAttribute.Value;
            else
                throw new ConfigurationErrorsException(string.Format("ExceptionInvalidXmlAttributeValue {0} - {1}.",
                                                                     AttributeController, configNode.Name));
		}

        private void LoadAjaxWebServiceTypeAttribute(XmlNode configNode)
        {
            XmlNode currentAttribute;
            currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeIsAJAXWebService);
            if (isValidAttribute(currentAttribute))
                _isajaxwebservice = XmlConvert.ToBoolean(currentAttribute.Value);
        }

		#endregion

		#region Properties
		
        /// <summary>
		/// Gets the controller name related to this view.
		/// </summary>
		public string Controller
		{
			get { return _controller; }
		}

		/// <summary>
		/// Specifies if the windows should stay open when the Navigate
		/// method is invoked.
		/// </summary>
		public bool IsStayOpen
		{
			get { return _isStayOpen; }
		}

		/// <summary>
		/// Gets a value that indicates if this view is displayed modally.
		/// </summary>
		public bool IsOpenModal
		{
			get { return _isOpenModal; }
		}

		/// <summary>
		/// Gets a value that indicates the layout manager for this view.
		/// </summary>
		public string LayoutManager
		{
			get {return _layoutManager;}
		}

		/// <summary>
		/// Gets the XPathNavigator that can be used to traverse the XmlNode that defines the view. 
		/// </summary>
		public XPathNavigator Navigator
		{
			get { return _navigator; }
		}
		
		/// <summary>
		/// Returns true if the view can be a floating window; if not, returns false.
		/// </summary>
		public bool IsFloatable
		{
		   	get { return _isFloatable; }
		}

		/// <summary>
		/// Returns true if the view can be a parent for floating windows; if not, returns false.
		/// </summary>
		public bool CanHaveFloatingWindows
		{
		   	get { return _canHaveFloatingWindows; }
		}

		/// <summary>
		/// Gets the collection of custom attributes defined in the app.config (web.config). 
		/// </summary>
		public XmlAttributeCollection CustomAttributes 
		{
			get { return _node.Attributes; }
		}

		/// <summary>
		/// Gets the child nodes that make up the View element in the app.config.
		/// </summary>
		public XmlNodeList ChildNodes 
		{
			get { return _node.ChildNodes; }
		}

        /// <summary>
        /// AJAX
        /// </summary>
	    public bool IsAjaxWebService
	    {
            get { return _isajaxwebservice; }
	    }

        /// <summary>
        /// <b>True</b> if view is configured to start the new application transaction, 
        /// <b>false</b> otherwise.
        /// </summary>
        public Boolean AppTransactionStartConfigured
        {
            get 
            {
                XmlAttributeCollection customAttributes = CustomAttributes;
                XmlAttribute app_transaction = customAttributes["app_transaction_start"];
                if (app_transaction == null || String.IsNullOrEmpty(app_transaction.Value))
                    return false;

                Boolean start_app_transaction = Boolean.Parse(app_transaction.Value);
                return start_app_transaction;
            }
        }

        /// <summary>
        /// Map the property of the view to particular key on the state.
        /// </summary>
        /// <param name="key">The name of the key.</param>
        /// <param name="property">The name of the property.</param>
        public void MapPropertyToKey(String key, String property)
        {
            String existingKey = GetKeyByProperty(property);
            if (!String.IsNullOrEmpty(existingKey))
                _mapKeysProps.Remove(existingKey);

            _mapKeysProps.Add(key, property);
        }

        /// <summary>
        /// Return the name of the key to which this property is mapped to.
        /// </summary>
        /// <param name="property">The name of the key.</param>
        /// <returns></returns>
        public String GetKeyByProperty(String property)
        {
            foreach (KeyValuePair<String, String> key_value in _mapKeysProps)
            {
                if (property.Equals(key_value.Value))
                    return key_value.Key;
            }

            return String.Empty;
        }

        /// <summary>
        /// The mapping of state keys to the properties of the view.
        /// </summary>
        public IDictionary<StateKeySettings, String> MapKeysProperties
        {
            get
            {
                IDictionary<StateKeySettings, String> mapKeys = new SortedDictionary
                    <StateKeySettings, String>(new StateKeysComparer());

                IDictionary<String, String> mapKeysProps = _mapKeysProps;
                foreach (KeyValuePair<String, String> key_value in mapKeysProps)
                {
                    StateKeySettings settings = UIPConfiguration.Config.GetStateKeySettingsFromName(key_value.Key);

                    if (settings == null)
                    {
                        throw new ConfigurationErrorsException(
                            string.Format("ExceptionUIPConfigStateKeyNotFound: {0}.", key_value.Key));
                    }
                    
                    mapKeys.Add(settings, key_value.Value);
                }

                return mapKeys;
            }
        }

        #endregion
	}

    internal class StateKeysComparer : IComparer<StateKeySettings>
    {
        public virtual int Compare(StateKeySettings x, StateKeySettings y)
        {
            if (x.BindingPriority > y.BindingPriority)
                return -1;
            else if (x.BindingPriority < y.BindingPriority)
                return 1;
            else return String.Compare(x.Name, y.Name);
        }
    }
}
