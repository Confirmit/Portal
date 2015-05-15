using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	public class ZipReader : IDisposable
	{	
		#region Конструкторы
		
		public ZipReader( Stream srcStream )
		{
			zipStream = new ZipInputStream( srcStream );
		}

		#endregion

		#region Поля

		private ZipInputStream zipStream;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region Основные методы

		/// <summary>
		/// Читает из архива очередной файл.
		/// </summary>
		/// <param name="outputStream">
		/// Поток, в который записывается содержимое прочитанного файла.
		/// </param>
		/// <returns>Имя прочитанного файла или null, если прочесть нечего.</returns>
		public string ReadFile( Stream outputStream )
		{
			// прочитать очередную запись из архива
			ZipEntry entry = zipStream.GetNextEntry();
			
			// записей больше нет
			if (entry == null)
				return null;

			// прочитать имя файла
			string fileName = entry.Name;

			byte[] buffer = new byte[4096];
			while (true)
			{
				// прочитать блок из архива
				int bytesRead = zipStream.Read( buffer, 0, buffer.Length );

				string str = Encoding.UTF8.GetString(buffer);
			
				// если ничего не прочитано, прекратить считывание
				if (bytesRead == 0)
					break;
				// иначе, записать блок в результирующий поток
				outputStream.Write( buffer, 0, bytesRead );
			}

			// переместить указатель результирующего потока на начало
			outputStream.Flush();
			outputStream.Seek( 0, SeekOrigin.Begin );

			return fileName;
		}

		public void Close()
		{
			zipStream.Close();
		}
 
		#endregion		
	}
}
