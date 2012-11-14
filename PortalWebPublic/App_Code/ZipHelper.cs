using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

/// <summary>
/// Класс для вспомогательных методов работы с ZIP.
/// </summary>
public static class ZipHelper
{
	/// <summary>
	/// Архивирует данные одного потока в другой поток.
	/// </summary>
	/// <param name="inputStream">Входной поток.</param>
	/// <param name="outputStream">Выходной поток.</param>
	/// <param name="entryFileName">Имя файла, которое будет помещено в выходном архиве.</param>
	public static void ZipData( Stream inputStream, Stream outputStream, string entryFileName )
	{
		Crc32 crc = new Crc32();
		ZipOutputStream zipStream = new ZipOutputStream( outputStream );
		// начинаем архивировать
		zipStream.SetLevel( 9 ); // уровень сжатия

		long length = inputStream.Length;
		byte[] buffer = new byte[length];
		inputStream.Read( buffer, 0, buffer.Length );

		ZipEntry entry = new ZipEntry( entryFileName );
		entry.DateTime = DateTime.Now;
		entry.Size = length;
		crc.Reset();
		crc.Update( buffer );
		entry.Crc = crc.Value;

		zipStream.PutNextEntry( entry );

		zipStream.Write( buffer, 0, buffer.Length );

		zipStream.Finish();
	}

	/// <summary>
	/// Архивирует файл в Zip-архив.
	/// </summary>
	/// <param name="srcFileName">Имя исходного файла.</param>
	/// <param name="zipFileName">Имя архива.</param>
	public static void ZipFile( string srcFileName, string zipFileName )
	{
		using(FileStream inputStream = new FileInfo( srcFileName ).OpenRead())
		using(FileStream outputStream = File.Create( zipFileName ))
		{
			ZipData( inputStream, outputStream, srcFileName );
		}
	}
}
