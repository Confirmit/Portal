using System;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	/// <summary>
	/// Класс-писатель, использующийся для ZIP-компрессии данных.
	/// </summary>
	public class ZipWriter : IDisposable
	{
		#region Конструкторы

		/// <summary>
		/// Создает объект, который пишет сжатые данные в выходной поток.
		/// </summary>
		/// <param name="dstStream">Поток, в который производится запись.</param>
		public ZipWriter( Stream dstStream )
		{
			Init( dstStream, 6 );
		}

		/// <summary>
		/// Создает объект, который пишет сжатые данные в выходной поток.
		/// </summary>
		/// <param name="dstStream">Поток, в который производится запись.</param>
		/// <param name="compressionLevel">Уровень сжатия (1-9).</param>
		public ZipWriter(Stream dstStream, int compressionLevel)
		{
			Init( dstStream, compressionLevel );
		}

		private void Init( Stream dstStream, int compressionLevel )
		{
			m_zipStream = new ZipOutputStream( dstStream );
			// уровень сжатия
			m_zipStream.SetLevel( compressionLevel );
		}

		#endregion

		#region Поля

		protected ZipOutputStream m_zipStream; 
		
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region Основные методы
		
		/// <summary>
		/// Производит компрессию указанного потока. Сжатые данные добавляются в конец
		/// выходного потока.
		/// </summary>
		/// <param name="fileName">Имя файла (записи в ZIP-потоке).</param>
		/// <param name="srcStream">Поток данных для сжатия.</param>
		public void WriteFile( string fileName, Stream srcStream )
		{
			byte[] buffer = new byte[srcStream.Length];
			srcStream.Read( buffer, 0, buffer.Length );

			// контрольная сумма
			Crc32 crc = new Crc32();
			crc.Reset();
			crc.Update( buffer );
			
			// добавить запись о файле в результирующий поток
			ZipEntry entry = new ZipEntry( fileName );
			entry.DateTime = DateTime.Now;
			entry.Size = srcStream.Length;
			entry.Crc = crc.Value;
			m_zipStream.PutNextEntry( entry );

			// сжать и записать буфер данных в результирующий поток
			m_zipStream.Write( buffer, 0, buffer.Length );
		}

		public void Flush()
		{
			m_zipStream.Flush();
		}

		public void Finish()
		{
			m_zipStream.Finish();
		}

		public void Close()
		{
			m_zipStream.Close();
		}

		#endregion
	}
}
