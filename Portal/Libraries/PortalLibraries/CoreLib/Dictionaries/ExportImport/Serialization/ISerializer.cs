using System;
using System.Data;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization
{
	public interface ISerializer
	{
		void SerializePart( DataTable data );
		void WriteSerializationResult( Stream stream );
	}
}
