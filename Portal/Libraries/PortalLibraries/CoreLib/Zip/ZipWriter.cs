using System;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	/// <summary>
	/// �����-��������, �������������� ��� ZIP-���������� ������.
	/// </summary>
	public class ZipWriter : IDisposable
	{
		#region ������������

		/// <summary>
		/// ������� ������, ������� ����� ������ ������ � �������� �����.
		/// </summary>
		/// <param name="dstStream">�����, � ������� ������������ ������.</param>
		public ZipWriter( Stream dstStream )
		{
			Init( dstStream, 6 );
		}

		/// <summary>
		/// ������� ������, ������� ����� ������ ������ � �������� �����.
		/// </summary>
		/// <param name="dstStream">�����, � ������� ������������ ������.</param>
		/// <param name="compressionLevel">������� ������ (1-9).</param>
		public ZipWriter(Stream dstStream, int compressionLevel)
		{
			Init( dstStream, compressionLevel );
		}

		private void Init( Stream dstStream, int compressionLevel )
		{
			m_zipStream = new ZipOutputStream( dstStream );
			// ������� ������
			m_zipStream.SetLevel( compressionLevel );
		}

		#endregion

		#region ����

		protected ZipOutputStream m_zipStream; 
		
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region �������� ������
		
		/// <summary>
		/// ���������� ���������� ���������� ������. ������ ������ ����������� � �����
		/// ��������� ������.
		/// </summary>
		/// <param name="fileName">��� ����� (������ � ZIP-������).</param>
		/// <param name="srcStream">����� ������ ��� ������.</param>
		public void WriteFile( string fileName, Stream srcStream )
		{
			byte[] buffer = new byte[srcStream.Length];
			srcStream.Read( buffer, 0, buffer.Length );

			// ����������� �����
			Crc32 crc = new Crc32();
			crc.Reset();
			crc.Update( buffer );
			
			// �������� ������ � ����� � �������������� �����
			ZipEntry entry = new ZipEntry( fileName );
			entry.DateTime = DateTime.Now;
			entry.Size = srcStream.Length;
			entry.Crc = crc.Value;
			m_zipStream.PutNextEntry( entry );

			// ����� � �������� ����� ������ � �������������� �����
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
