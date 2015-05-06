using System;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization
{
	public interface ISerializationMethod
	{
		ISerializer CreateDataSerializer();
		
		IDeserializer CreateDataDeserializer();
		
		string FormatExtension
		{
			get;
		}
	}
}
