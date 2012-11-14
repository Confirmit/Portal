using System;
using System.Text.RegularExpressions;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.Logger;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// Класс рассылки уведомлений об отсутствии рабочего интервала.
	/// </summary>
	public class NotificationDelivery
	{
		#region Fields

		private string m_SMTPServer;
		private string m_FromAddress;
		private string m_Subject;
		private string m_SubjectAdmin;
		private string m_Message;
		private string m_MessageAdmin;

		#endregion

		#region Properties

		/// <summary>
		/// Адрес SMTP-сервера.
		/// </summary>
		public string SmtpServer
		{
			get { return m_SMTPServer; }
			set { m_SMTPServer = value; }
		}

		/// <summary>
		/// Обратный адрес.
		/// </summary>
		public string FromAddress
		{
			get { return m_FromAddress; }
			set { m_FromAddress = value; }
		}

		/// <summary>
		/// Тема письма для рассылки статистики пользователя.
		/// </summary>
		public string Subject
		{
			get { return m_Subject; }
			set { m_Subject = value; }
		}

		/// <summary>
		/// Тема письма для рассылки статистики офиса.
		/// </summary>
		public string SubjectAdmin
		{
			get { return m_SubjectAdmin; }
			set { m_SubjectAdmin = value; }
		}

		/// <summary>
		/// Текст письма.
		/// </summary>
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}

		/// <summary>
		/// Текст письма для администратора.
		/// </summary>
		public string MessageAdmin
		{
			get { return m_MessageAdmin; }
			set { m_MessageAdmin = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Рассылает уведомления об отсутствии рабочих интервалах.
		/// </summary>
        public void DeliverNotification()
		{
		    // Не оповещать по праздникам.
		    if (CalendarItem.GetHoliday(DateTime.Now))
		        return;

		    // Получить список всех пользователей.
		    Person[] users = UserList.GetEmployeeList();
		    if (users == null || users.Length == 0)
		        return;

		    // Получить список рассылки.
		    NotificationList nList = NotificationList.GetNotificationList(NotificationType.NotRegistered);
		    Logger.Instance.Info("Getting noting list is success.");

		    foreach (Person curUser in users)
		    {
		        try
		        {
		            // Не оповещать не слущажих.
		            // Не оповещать служащих, не имеющих адреса электронной почты.
		            // Не оповещать московских служащих.
		            if (!curUser.IsInRole("Employee")
		                || string.IsNullOrEmpty(curUser.PrimaryEMail)
		                || curUser.EmployeesUlterSYSMoscow)
		                continue;

		            // Получить последнее событие за сегодня.
		            WorkEvent lastEvent = WorkEvent.GetCurrentEventOfDate(curUser.ID.Value, DateTime.Today);

		            if (lastEvent == null)
		            {
		                Logger.Instance.Info("Notice sending to " + curUser.FullName + ".");

		                MailItem item = new MailItem
		                                    {
		                                        FromAddress = FromAddress,
		                                        ToAddress = curUser.PrimaryEMail,
		                                        Subject = Subject,
		                                        Body = Regex.Replace(Message, "_UserName_", curUser.FullName),
		                                        MessageType = ((int) MailTypes.NRNotification)
		                                    };
		                item.Save();

		                if (nList != null)
		                {
		                    foreach (string eMail in nList)
		                    {
                                Logger.Instance.Info("Notice sending to administrator E-Mail " + eMail + ".");

		                        MailItem adminItem = new MailItem
		                                                 {
		                                                     FromAddress = FromAddress,
		                                                     ToAddress = eMail,
		                                                     Subject =
		                                                         Regex.Replace(SubjectAdmin, "_UserName_", curUser.FullName),
		                                                     Body =
		                                                         Regex.Replace(MessageAdmin, "_UserName_", curUser.FullName),
		                                                     MessageType = ((int) MailTypes.NRNotification)
		                                                 };
		                        adminItem.Save();
		                    }
		                }
		            }
		        }
		        catch (Exception ex)
		        {
		            Logger.Instance.Error("При обработке информации о пользователе " + curUser.FullName + " произошла ошибка.", ex);
		        }
		    }
		}

	    #endregion
	}
}
