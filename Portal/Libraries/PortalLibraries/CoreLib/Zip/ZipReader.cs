using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Core.Zip
{
	public class ZipReader : IDisposable
	{	
		#region ������������
		
		public ZipReader( Stream srcStream )
		{
			zipStream = new ZipInputStream( srcStream );
		}

		#endregion

		#region ����

		private ZipInputStream zipStream;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region �������� ������

		/// <summary>
		/// ������ �� ������ ��������� ����.
		/// </summary>
		/// <param name="outputStream">
		/// �����, � ������� ������������ ���������� ������������ �����.
		/// </param>
		/// <returns>��� ������������ ����� ��� null, ���� �������� ������.</returns>
		public string ReadFile( Stream outputStream )
		{
			// ��������� ��������� ������ �� ������
			ZipEntry entry = zipStream.GetNextEntry();
			
			// ������� ������ ���
			if (entry == null)
				return null;

			// ��������� ��� �����
			string fileName = entry.Name;

			byte[] buffer = new byte[4096];
			while (true)
			{
				// ��������� ���� �� ������
				int bytesRead = zipStream.Read( buffer, 0, buffer.Length );

				string str = Encoding.UTF8.GetString(buffer);
			
				// ���� ������ �� ���������, ���������� ����������
				if (bytesRead == 0)
					break;
				// �����, �������� ���� � �������������� �����
				outputStream.Write( buffer, 0, bytesRead );
			}

			// ����������� ��������� ��������������� ������ �� ������
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
