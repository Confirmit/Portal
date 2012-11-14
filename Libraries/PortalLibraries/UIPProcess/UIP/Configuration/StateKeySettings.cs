using System;
using System.Xml;
using UIPProcess.UIP.Views;

namespace UIProcess
{
	/// <summary>
	/// The StateKeySettings capture the configuration information for a specific view (a view is a class that implements the 
	/// <see cref="IView"/> interface).
	/// </summary>
    public class StateKeySettings : ObjectSettings
	{
        private XmlNode _node;
        
        #region Constructor
		/// <summary>
		/// Overloaded. Default constructor.
		/// </summary>
		public StateKeySettings( ) : base( )
		{
		}

		/// <summary>
		/// Overloaded. Initialized a new instance of the ViewSettings class using the specified configNode.
		/// </summary>
		/// /// <param name="configNode">XmlNode from the configuration file.</param>
		public StateKeySettings(XmlNode configNode) : this(configNode, System.Globalization.CultureInfo.CurrentCulture)
		{
			
		}

		/// <summary>
		/// Creates an instance of ViewSettings using the specified configNode and IFormatProvider. 
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		/// <param name="formatProvider">The IFormatProvider required for globalization.</param>
        public StateKeySettings(XmlNode configNode, IFormatProvider formatProvider)
            : base(configNode)
		{
			LoadAttributes(configNode, formatProvider);
			_node = configNode;
		}		      

		private void LoadAttributes(XmlNode configNode, IFormatProvider formatProvider)
		{
            XmlNode currentAttribute
                = configNode.Attributes.RemoveNamedItem(_atributeBindingPriority);
            if (isValidAttribute(currentAttribute))
                _bindingPriority = 
                    Int32.Parse(currentAttribute.Value.ToString(formatProvider));

            currentAttribute
                = configNode.Attributes.RemoveNamedItem(_atributeStoreInStateOnBackward);
            if (isValidAttribute(currentAttribute))
                _storeInStateOnBackward =
                    Boolean.Parse(currentAttribute.Value.ToString(formatProvider));
        }
        private const string _atributeBindingPriority = "binding-priority";
        private const string _atributeStoreInStateOnBackward = "store-in-state-on-backward";

		#endregion

        public Int32 BindingPriority
        {
            get { return _bindingPriority; }
        }
        private Int32 _bindingPriority = -1;

        public Boolean IsStoreInStateOnBackward
        {
            get { return _storeInStateOnBackward; }
        }
        private Boolean _storeInStateOnBackward = false;
    }
}
