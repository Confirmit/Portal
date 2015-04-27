using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization.XmlSerialization
{
	public class XmlDeserializer : IDeserializer
	{
		#region IDeserializer Members

		public IEnumerable<DataTable> Deserialize( Stream inputStream )
		{
			DataSet ds = new DataSet();
			ds.ReadXml( inputStream );
			foreach (DataTable table in ds.Tables)
				yield return table;			
		}

		#endregion
	}
}
