using System;
using System.Data;
using System.Configuration;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	/// <summary>
	/// ����� ��� ��������������� ������� ������ � ZIP.
	/// </summary>
	public static class ZipHelper
	{
		/// <summary>
		/// ���������� ������ ������ ������ � ������ �����.
		/// </summary>
		/// <param name="entryFileName">��� �����, ������� ����� �������� � �������� ������.</param>
		/// <param name="inputStream">������� �����.</param>
		/// <param name="outputStream">�������� �����.</param>
		public static void ZipData(string entryFileName , Stream inputStream, Stream outputStream)
		{
			using (ZipWriter writer = new ZipWriter( outputStream ))
			{
				writer.WriteFile( entryFileName, inputStream );
				writer.Finish();
			}
		}

		/// <summary>
		/// ���������� ���� � Zip-�����.
		/// </summary>
		/// <param name="srcFileName">��� ��������� �����.</param>
		/// <param name="zipFileName">��� ������.</param>
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