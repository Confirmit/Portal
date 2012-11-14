using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Core.Dictionaries.ExportImport.Serialization;
using Core.Zip;
using Core.Exceptions;

namespace Core.Dictionaries.ExportImport.Packing.MultiFilePacker
{
	public class MultiFilePacker : PackerWithSerializationBase
	{
		#region  онструкторы

		public MultiFilePacker( params ISerializationMethod[] serializationMethods )
			: base( serializationMethods )
		{
		}
		
		#endregion

		#region IPacker members

		#region ”паковка

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

		private ZipWriter m_zipWriter = null;

		public override void StartBatch( Stream outputStream )
		{
			base.StartBatch( outputStream );
			
			m_zipWriter = new ZipWriter( outputStream );
		}

		public override void AddToBatch( DataTable data )
		{
			using (Stream stream = new MemoryStream())
			{
				// сериализовать данные в поток
				Serializer.SerializePart( data );
				Serializer.WriteSerializationResult( stream );

				// добавить полученный поток в архив
				stream.Flush();
				stream.Seek( 0, SeekOrigin.Begin );
				string fileName = String.Format( "{0}.{1}", data.TableName, SerializationMethods[0].FormatExtension );
				m_zipWriter.WriteFile( fileName, stream );
			}
		}

		public override void FinishBatch()
		{
			m_zipWriter.Finish();
			m_zipWriter.Close();
		}

		#endregion

		#region –аспаковка

		#region ¬спомогательные методы дл€ распаковки
		
		private Dictionary<string, IDeserializer> m_extensionToDeserializerMap;
		private Dictionary<string, IDeserializer> ExtensionToDeserializerMap
		{
			get
			{
				if (m_extensionToDeserializerMap == null)
				{
					m_extensionToDeserializerMap = new Dictionary<string, IDeserializer>();

					foreach (ISerializationMethod method in SerializationMethods)
						m_extensionToDeserializerMap[method.FormatExtension.ToLower()] = method.CreateDataDeserializer();
				}
				return m_extensionToDeserializerMap;
			}
		}

		private IDeserializer GetDeserializer( string fileName )
		{
			IDeserializer deserializer;
			string extension = "";
			extension	= Path.GetExtension( fileName ).ToLower();
			if (!String.IsNullOrEmpty( extension ) && extension[0] == '.' && extension.Length > 1)
				extension = extension.Substring( 1 );

            if (!ExtensionToDeserializerMap.TryGetValue(extension, out deserializer))
            {
                throw new CoreInvalidDataException(Resources.ResourceManager.GetString("DeserializeFileException", fileName));
            }

			return deserializer;
		}

		#endregion
		
		public override IEnumerable<DataTable> UnPack( Stream inputStream )
		{
			using (ZipReader reader = new ZipReader( inputStream ))
			{
				while (true)
				{
					using (Stream stream = new MemoryStream())
					{
						// прочитать очередной файл во временный поток.
						// ≈сли прочесть нечего, прервать цикл
						string file = reader.ReadFile( stream );
						if (null == file)
							break;

						// десериализировать этот поток
						// и вернуть прочитанные таблицы
						IDeserializer deserializer = GetDeserializer( file );
						foreach (DataTable table in deserializer.Deserialize( stream ))
							yield return table;
					}
				}
			}
		}

		#endregion
		
		#endregion
	}
}
