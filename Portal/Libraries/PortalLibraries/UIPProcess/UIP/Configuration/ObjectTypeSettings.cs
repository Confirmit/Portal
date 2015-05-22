using System;
using System.Configuration;
using System.Xml;

namespace UIProcess
{
	/// <summary>
	/// Base class for all providers settings within the UIP configuration settings in the configuration file.
	/// </summary>
    public class ObjectTypeSettings : ObjectSettings
	{
		#region Declares Variables
		
        private const string AttributeType = "type";
		private const string Comma = ",";

		private string _type;
		private string _assembly;
		
        #endregion

		#region Constructors
		/// <summary>
		/// Overloaded. Default constructor.
		/// </summary>
		public ObjectTypeSettings() : base() {}

		/// <summary>
		/// Overloaded. Initialized an instance of the ObjectTypeSettings class using the specified configNode.
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		public ObjectTypeSettings(XmlNode configNode)
            : base(configNode)
		{
			LoadAttributes(configNode);
		}

		private void LoadAttributes(XmlNode configNode)
		{
            XmlNode currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeType);
            
			//Gets the typed object attributes
			string fullType;
            if (currentAttribute.Value.Trim().Length > 0)
                fullType = currentAttribute.Value;
            else
                throw new ConfigurationErrorsException(string.Format("ExceptionInvalidXmlAttributeValue {0} - {1}.",
                                                                     AttributeType, configNode.Name));

			//  fix up type/asm strings
			splitType(fullType);			
		}

        public ObjectTypeSettings(String strFullType)
        {
            splitType(strFullType);
        }

        #endregion

        #region Helpers

        /// <summary>
		/// Takes the incoming full type string, defined as: 
		/// "UIProcess.WinFormViewManager,   UIProcess, 
		///			Version=1.0.0.4, Culture=neutral, PublicKeyToken=d69d63db1380c14d"
		///  and splits the type into two strings: the typeName and the assemblyName. These are passed as OUT params. 
		///  This routine also cleans up any extra white space and throws an exception if the full type string
		///  does not have five comma-delimited parts. It expects the true full name, complete with version and publicKeyToken.
		/// </summary>
		/// <param name="fullType">The full type string defined in the configuration file.</param>
		protected void splitType( string fullType )
		{
			string[] parts = fullType.Split( Comma.ToCharArray() );

			if( parts.Length == 1 )
				_type = fullType;
            else if (parts.Length == 2)
            { 
                //  set the object type name
                this._type = parts[0].Trim();

                //  set the object assembly name
                this._assembly = parts[1].Trim();
            }
            else if (parts.Length == 5)
            {
                //  set the object type name
                this._type = parts[0].Trim();

                //  set the object assembly name
                this._assembly = String.Concat(parts[1].Trim() + Comma,
                    parts[2].Trim() + Comma,
                    parts[3].Trim() + Comma,
                    parts[4].Trim());
            }
            else
                throw new ArgumentException(string.Format("RES_ExceptionBadTypeArgumentInFactory {0}.", "fullType"));
		}

        protected bool getBooleanAttribute(XmlNode configNode, string attributeName)
        {
            XmlNode currentAttribute = configNode.Attributes.RemoveNamedItem(attributeName);
            if (isValidAttribute(currentAttribute))
                return XmlConvert.ToBoolean(currentAttribute.Value);
            return false;
        }

        #endregion

        #region Properties

        /// <summary>
		/// Gets the object fully qualified type name.
		/// </summary>
		public String Type
		{
			get{ return _type; }
		}

		/// <summary>
		/// Gets the object fully qualified assembly name.
		/// </summary>
		public String Assembly 
		{
			get { return _assembly; }
		}


		#endregion
	}
}
