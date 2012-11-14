using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Core.Dictionaries.ExportImport.Packing
{
	public interface IPacker
	{
		void StartBatch( Stream outputStream );
		void AddToBatch( DataTable data );
		void FinishBatch();

		IEnumerable<DataTable> UnPack( Stream stream );
	}
}
