using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Core.Dictionaries.ExportImport.Serialization.XmlSerialization
{
	public class XmlSerializer : ISerializer 
	{
		#region Свойства
		
		private DataSet m_resultDataSet = new DataSet();

		protected DataSet ResultDataSet
		{
			get { return m_resultDataSet; }
		}

		#endregion

		#region ISerializer Members
		
		public void SerializePart( DataTable data )
		{
			// TODO: ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ. УБРАТЬ!
			// data.TableName += Guid.NewGuid().ToString();
			
			
			ResultDataSet.Tables.Add( data.Copy() );
		}

		public void WriteSerializationResult( Stream stream )
		{
			ResultDataSet.WriteXml( stream, XmlWriteMode.WriteSchema );
			ResultDataSet.Tables.Clear();
		}

		#endregion
	}
}
