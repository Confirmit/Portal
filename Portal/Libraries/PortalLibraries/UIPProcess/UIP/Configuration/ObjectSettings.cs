using System;
using System.Configuration;
using System.Xml;

namespace UIProcess
{
	/// <summary>
	/// Base class for all providers settings within the UIP configuration settings in the configuration file.
	/// </summary>
	public class ObjectSettings
	{
		private const string AttributeName = "name";	
		private String _name = String.Empty;

		/// <summary>
		/// Overloaded. Default constructor.
		/// </summary>
		public ObjectSettings(){}

		/// <summary>
		/// Overloaded. Initialized an instance of the ObjectTypeSettings class using the specified configNode.
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		public ObjectSettings( XmlNode configNode )
		{
			LoadAttributes(configNode);
		}

		private void LoadAttributes(XmlNode configNode)
		{
			XmlNode currentAttribute;
            
			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeName);
            if (currentAttribute.Value.Trim().Length > 0)
                _name = currentAttribute.Value;
            else
                throw new ConfigurationErrorsException(string.Format("ExceptionInvalidXmlAttributeValue {0} - {1}.",
                                                                     AttributeName, configNode.Name));
		}

        protected bool isValidAttribute(XmlNode currentAttribute)
        {
            return currentAttribute != null && currentAttribute.Value.Trim().Length > 0;
        }

        /// <summary>
		/// Gets the object name.
		/// </summary>
		public String Name
		{
			get{ return _name; }
		}
	}
}
