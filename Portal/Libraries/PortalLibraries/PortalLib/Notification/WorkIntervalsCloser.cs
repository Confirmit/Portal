using System;
using System.Text.RegularExpressions;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Logger;
using UlterSystems.PortalLib.BusinessObjects;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// Класс для закрытия рабочих интервалов.
	/// </summary>
	public class WorkIntervalsCloser
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
		/// Закрывает незакрытые рабочие интервалы.
		/// </summary>
        public void CloseWorkIntervals()
		{
		    Logger.Instance.Info("Procedure of closing non closed work intervals is started.");

		    // Получить список всех пользователей.
		    var users = UserList.GetUserList();
		    if (users == null || users.Length == 0)
		        return;

		    // Получить список рассылки.
		    var nList = NotificationList.GetNotificationList(NotificationType.CloseEvents);
		    
			Logger.Instance.Info("The list of dispatch is loaded.");

		    foreach (var curUser in users)
		    {
		        try
		        {
		            var todayWorkEvent = WorkEvent.GetMainWorkEvent(curUser.ID.Value, DateTime.Today);

		            // Не обрабатывать уже закрытые рабочие интервалы.
		            if (todayWorkEvent == null
		                || todayWorkEvent.EndTime != todayWorkEvent.BeginTime)
		                continue;

		            // Получить последнее событие.
		            var lastEvent = WorkEvent.GetCurrentEventOfDate(curUser.ID.Value, DateTime.Today);
		            if (lastEvent.EventType == WorkEventType.TimeOff
		                || lastEvent.EventType == WorkEventType.LanchTime)
		            {
		                WorkEvent.DeleteEvent(lastEvent.ID);

		                // Закрыть рабочий интервал.
		                todayWorkEvent.EndTime = lastEvent.BeginTime;

		                WorkEvent.UpdateEvent(
		                    todayWorkEvent.ID,
		                    todayWorkEvent.Name,
		                    todayWorkEvent.BeginTime,
		                    todayWorkEvent.EndTime,
		                    todayWorkEvent.UserID,
		                    todayWorkEvent.ProjectID,
		                    todayWorkEvent.WorkCategoryID,
		                    todayWorkEvent.EventTypeID);

		                Logger.Instance.Info("The work interval for user " + curUser.FullName + " was closed.");
		            }
		            else
		            {
		                // Разослать уведомления.
		                if (!string.IsNullOrEmpty(curUser.PrimaryEMail))
		                {
		                    Logger.Instance.Info("Sending notice to user " + curUser.FullName + ".");

		                    string message = Message;
		                    message = Regex.Replace(message, "_UserName_", curUser.FullName);
		                    message = Regex.Replace(message, "_Date_", DateTime.Today.ToLongDateString());

		                	var item = new MailItem
		                	           	{
		                	           		FromAddress = FromAddress,
		                	           		ToAddress = curUser.PrimaryEMail,
		                	           		Subject = Subject,
		                	           		Body = message,
		                	           		MessageType = ((int) MailTypes.CENotification)
		                	           	};
		                    item.Save();
		                }

		                if (nList != null)
		                {
		                    foreach (string eMail in nList)
		                    {
		                        Logger.Instance.Info("Sending notice to administrator E-Mail " + eMail + ".");

		                        var message = MessageAdmin;
		                        message = Regex.Replace(message, "_UserName_", curUser.FullName);
		                        message = Regex.Replace(message, "_Date_", DateTime.Today.ToLongDateString());

		                    	var adminItem = new MailItem
		                    	                	{
		                    	                		FromAddress = FromAddress,
		                    	                		ToAddress = eMail,
		                    	                		Subject =
		                    	                			Regex.Replace(SubjectAdmin, "_UserName_",
		                    	                			              curUser.FullName),
		                    	                		Body = message,
		                    	                		MessageType = ((int) MailTypes.CENotification)
		                    	                	};
		                        adminItem.Save();
		                    }
		                }
		            }
		        }
		        catch (Exception ex)
		        {
		            Logger.Instance.Error("During working with user " + curUser.FullName + " information occured error.", ex);
		        }
		    }
		}

	    #endregion
	}
}