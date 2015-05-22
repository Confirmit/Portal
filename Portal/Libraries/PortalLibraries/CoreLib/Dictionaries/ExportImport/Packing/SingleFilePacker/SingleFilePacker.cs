using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Core.Dictionaries.ExportImport.Serialization;

namespace Core.Dictionaries.ExportImport.Packing.SingleFilePacker
{
	public class SingleFilePacker : PackerWithSerializationBase
	{
		#region Конструкторы

		public SingleFilePacker( params ISerializationMethod[] serializationMethods )
			: base( serializationMethods )
		{
		}
		
		#endregion
		
		#region IPacker Members

		#region Упаковка

		#region Serializer
		
		private ISerializer m_serializer = null;
		private ISerializer Serializer
		{
			get
			{
				if (m_serializer == null)
					m_serializer = SerializationMethods[0].CreateDataSerializer();

				return m_serializer;
			}
		}

		#endregion		
		
		public override void AddToBatch( DataTable data )
		{
			// сериализуем часть данных
			Serializer.SerializePart( data );
		}

		public override void FinishBatch()
		{
			// записываем данные в выходной поток
			Serializer.WriteSerializationResult( OutputStream );				
		}

		#endregion

		#region Распаковка

		public override IEnumerable<DataTable> UnPack( Stream inputStream )
		{
			IDeserializer deserializer = SerializationMethods[0].CreateDataDeserializer();
			return deserializer.Deserialize( inputStream );
		}

		#endregion
		
		#endregion
	}
}
