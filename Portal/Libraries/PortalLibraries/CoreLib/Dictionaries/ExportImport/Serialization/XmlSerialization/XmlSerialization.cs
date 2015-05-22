using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization.XmlSerialization
{
	public class XmlSerialization : ISerializationMethod
	{
		#region ISerializationMethod Members

		public ISerializer CreateDataSerializer()
		{
			return new XmlSerializer();
		}

		public IDeserializer CreateDataDeserializer()
		{
			return new XmlDeserializer();
		}

		public string FormatExtension
		{
			get { return "xml"; }
		}

		#endregion
	}
}
