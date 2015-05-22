using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization
{
	public interface IDeserializer
	{
		IEnumerable<DataTable> Deserialize(Stream inputStream);
	}
}
