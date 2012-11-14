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
	/// Класс сообщения об ошибке, которое посылается администратору по e-mail.
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
		/// Посылает уведомление администратору по почте.
		/// </summary>
		/// <param name="url">Url страницы, на которой произошла ошибка.</param>
		public void Report()
		{
			try
			{
				// формируем сообщение об ошибке
				StringBuilder message = new StringBuilder();
				message
					.AppendFormat( "Пользователь: {0}\n", User.Current.Login )
					.AppendFormat( "Текущая страница: {0}\n", m_Url )
					.Append( "-----------------------------------\n" )
					.Append( "Ошибка " )
					.Append( m_Exception.ToString().Replace( '\r', ' ' ) );

				// формируем и посылаем письмо
				using(MailMessage mail = new MailMessage())
				{
					// формируем тему
					mail.Subject = "Error Notification";
					// формируем тело письма
					mail.Body = message.ToString();
					// заполняем адрес назначения
					mail.To.Add( m_AddressTo );
					// устанавливаем кодировку
					// Используем кодировку koi8-r, т.к. у кодировки windows-1251 в .NET 2.0 BodyName="koi8-r"
					// (по-моему, это баг .NET 2.0!!!)
					mail.BodyEncoding = Encoding.GetEncoding( "koi8-r" );
					// устанавливаем формат письма
					mail.IsBodyHtml = false;
					// создаем SMTP-клиента и посылаем письмо
					SmtpClient client = new SmtpClient();
					client.Send( mail );
				}
			}
			catch
			{
				// НЕ ХВАТАЛО ЕЩЕ ОШИБКИ ВО ВРЕМЯ ПОСЫЛАНИЯ СООБЩЕНИЯ ОБ ОШИБКЕ.
			}
		}
	}
}
