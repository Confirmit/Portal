using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mail;
using Core.Security;
using Core.Exceptions;

namespace Core
{
	/// <summary>
	/// ����� ��������� �� ������, ������� ���������� �������������� �� e-mail.
	/// </summary>
	public class ErrorNotification
	{
		private Exception m_Exception;
		private string m_AddressTo;
		private string m_Url;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="addressTo"></param>
		/// <param name="url"></param>
		public ErrorNotification( Exception exception, string addressTo, string url )
		{
			m_Exception = exception;

            if (String.IsNullOrEmpty(addressTo))
                throw new CoreArgumentNullException(addressTo);

			m_AddressTo = addressTo;
			m_Url = url;
		}

		/// <summary>
		/// �������� ����������� �������������� �� �����.
		/// </summary>
		/// <param name="url">Url ��������, �� ������� ��������� ������.</param>
		public void Report()
		{
			try
			{
				// ��������� ��������� �� ������
				StringBuilder message = new StringBuilder();
				message
					.AppendFormat( "������������: {0}\n", User.Current.Login )
					.AppendFormat( "������� ��������: {0}\n", m_Url )
					.Append( "-----------------------------------\n" )
					.Append( "������ " )
					.Append( m_Exception.ToString().Replace( '\r', ' ' ) );

				// ��������� � �������� ������
				using(MailMessage mail = new MailMessage())
				{
					// ��������� ����
					mail.Subject = "Error Notification";
					// ��������� ���� ������
					mail.Body = message.ToString();
					// ��������� ����� ����������
					mail.To.Add( m_AddressTo );
					// ������������� ���������
					// ���������� ��������� koi8-r, �.�. � ��������� windows-1251 � .NET 2.0 BodyName="koi8-r"
					// (��-�����, ��� ��� .NET 2.0!!!)
					mail.BodyEncoding = Encoding.GetEncoding( "koi8-r" );
					// ������������� ������ ������
					mail.IsBodyHtml = false;
					// ������� SMTP-������� � �������� ������
					SmtpClient client = new SmtpClient();
					client.Send( mail );
				}
			}
			catch
			{
				// �� ������� ��� ������ �� ����� ��������� ��������� �� ������.
			}
		}
	}
}
