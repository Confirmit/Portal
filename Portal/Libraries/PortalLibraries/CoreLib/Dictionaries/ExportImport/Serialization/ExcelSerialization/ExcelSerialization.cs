using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dictionaries.ExportImport.Serialization.ExcelSerialization
{
	public class ExcelSerialization : ISerializationMethod
	{
		#region ISerializationMethod Members

		public ISerializer CreateDataSerializer()
		{
			return new ExcelSerializer();
		}

		public IDeserializer CreateDataDeserializer()
		{
			return new ExcelDeserializer();
		}

		public string FormatExtension
		{
			get { return "xls"; }
		}

		#endregion
	}
}
