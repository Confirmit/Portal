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
/// ����� ��� ��������������� ������� ������ � ZIP.
/// </summary>
public static class ZipHelper
{
	/// <summary>
	/// ���������� ������ ������ ������ � ������ �����.
	/// </summary>
	/// <param name="inputStream">������� �����.</param>
	/// <param name="outputStream">�������� �����.</param>
	/// <param name="entryFileName">��� �����, ������� ����� �������� � �������� ������.</param>
	public static void ZipData( Stream inputStream, Stream outputStream, string entryFileName )
	{
		Crc32 crc = new Crc32();
		ZipOutputStream zipStream = new ZipOutputStream( outputStream );
		// �������� ������������
		zipStream.SetLevel( 9 ); // ������� ������

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
	/// ���������� ���� � Zip-�����.
	/// </summary>
	/// <param name="srcFileName">��� ��������� �����.</param>
	/// <param name="zipFileName">��� ������.</param>
	public static void ZipFile( string srcFileName, string zipFileName )
	{
		using(FileStream inputStream = new FileInfo( srcFileName ).OpenRead())
		using(FileStream outputStream = File.Create( zipFileName ))
		{
			ZipData( inputStream, outputStream, srcFileName );
		}
	}
}
