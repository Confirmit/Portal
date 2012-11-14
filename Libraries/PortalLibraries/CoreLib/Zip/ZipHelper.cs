using System;
using System.Data;
using System.Configuration;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	/// <summary>
	/// Класс для вспомогательных методов работы с ZIP.
	/// </summary>
	public static class ZipHelper
	{
		/// <summary>
		/// Архивирует данные одного потока в другой поток.
		/// </summary>
		/// <param name="entryFileName">Имя файла, которое будет помещено в выходном архиве.</param>
		/// <param name="inputStream">Входной поток.</param>
		/// <param name="outputStream">Выходной поток.</param>
		public static void ZipData(string entryFileName , Stream inputStream, Stream outputStream)
		{
			using (ZipWriter writer = new ZipWriter( outputStream ))
			{
				writer.WriteFile( entryFileName, inputStream );
				writer.Finish();
			}
		}

		/// <summary>
		/// Архивирует файл в Zip-архив.
		/// </summary>
		/// <param name="srcFileName">Имя исходного файла.</param>
		/// <param name="zipFileName">Имя архива.</param>
		public static void ZipFile( string srcFileName, string zipFileName )
		{
			using (FileStream inputStream = new FileInfo( srcFileName ).OpenRead())
			using (FileStream outputStream = File.Create( zipFileName ))
			{
				ZipData(srcFileName , inputStream, outputStream);
			}
		}
	}
}