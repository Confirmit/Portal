using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	public class ZipReader : IDisposable
	{	
		#region  онструкторы
		
		public ZipReader( Stream srcStream )
		{
			zipStream = new ZipInputStream( srcStream );
		}

		#endregion

		#region ѕол€

		private ZipInputStream zipStream;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region ќсновные методы

		/// <summary>
		/// „итает из архива очередной файл.
		/// </summary>
		/// <param name="outputStream">
		/// ѕоток, в который записываетс€ содержимое прочитанного файла.
		/// </param>
		/// <returns>»м€ прочитанного файла или null, если прочесть нечего.</returns>
		public string ReadFile( Stream outputStream )
		{
			// прочитать очередную запись из архива
			ZipEntry entry = zipStream.GetNextEntry();
			
			// записей больше нет
			if (entry == null)
				return null;

			// прочитать им€ файла
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
