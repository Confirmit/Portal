using System;
using System.Text.RegularExpressions;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.Logger;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// ����� �������� ����������� �� ���������� �������� ���������.
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
		/// ����� SMTP-�������.
		/// </summary>
		public string SmtpServer
		{
			get { return m_SMTPServer; }
			set { m_SMTPServer = value; }
		}

		/// <summary>
		/// �������� �����.
		/// </summary>
		public string FromAddress
		{
			get { return m_FromAddress; }
			set { m_FromAddress = value; }
		}

		/// <summary>
		/// ���� ������ ��� �������� ���������� ������������.
		/// </summary>
		public string Subject
		{
			get { return m_Subject; }
			set { m_Subject = value; }
		}

		/// <summary>
		/// ���� ������ ��� �������� ���������� �����.
		/// </summary>
		public string SubjectAdmin
		{
			get { return m_SubjectAdmin; }
			set { m_SubjectAdmin = value; }
		}

		/// <summary>
		/// ����� ������.
		/// </summary>
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}

		/// <summary>
		/// ����� ������ ��� ��������������.
		/// </summary>
		public string MessageAdmin
		{
			get { return m_MessageAdmin; }
			set { m_MessageAdmin = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// ��������� ����������� �� ���������� ������� ����������.
		/// </summary>
        public void DeliverNotification()
		{
		    // �� ��������� �� ����������.
		    if (CalendarItem.GetHoliday(DateTime.Now))
		        return;

		    // �������� ������ ���� �������������.
		    Person[] users = UserList.GetEmployeeList();
		    if (users == null || users.Length == 0)
		        return;

		    // �������� ������ ��������.
		    NotificationList nList = NotificationList.GetNotificationList(NotificationType.NotRegistered);
		    Logger.Instance.Info("Getting noting list is success.");

		    foreach (Person curUser in users)
		    {
		        try
		        {
		            // �� ��������� �� ��������.
		            // �� ��������� ��������, �� ������� ������ ����������� �����.
		            // �� ��������� ���������� ��������.
		            if (!curUser.IsInRole("Employee")
		                || string.IsNullOrEmpty(curUser.PrimaryEMail)
		                || curUser.EmployeesUlterSYSMoscow)
		                continue;

		            // �������� ��������� ������� �� �������.
		            WorkEvent lastEvent = WorkEvent.GetCurrentEventOfDate(curUser.ID.Value, DateTime.Today);

		            if (lastEvent == null)
		            {
		                Logger.Instance.Info("Notice sending to " + curUser.FullName + ".");

                        string message = Message;
                        message = Regex.Replace(message, "_UserName_", curUser.FullName);
                        message = Regex.Replace(message, "_Date_", DateTime.Today.ToLongDateString());

		                MailItem item = new MailItem
		                                    {
		                                        FromAddress = FromAddress,
		                                        ToAddress = curUser.PrimaryEMail,
		                                        Subject = Subject,
		                                        Body = message,
		                                        MessageType = ((int) MailTypes.NRNotification)
		                                    };
		                item.Save();

		                if (nList != null)
		                {
		                    foreach (string eMail in nList)
		                    {
                                Logger.Instance.Info("Notice sending to administrator E-Mail " + eMail + ".");

                                message = MessageAdmin;
                                message = Regex.Replace(message, "_UserName_", curUser.FullName);
                                message = Regex.Replace(message, "_Date_", DateTime.Today.ToLongDateString());

		                        MailItem adminItem = new MailItem
		                                                 {
		                                                     FromAddress = FromAddress,
		                                                     ToAddress = eMail,
		                                                     Subject =
		                                                         Regex.Replace(SubjectAdmin, "_UserName_", curUser.FullName),
		                                                     Body = message,
		                                                     MessageType = ((int) MailTypes.NRNotification)
		                                                 };
		                        adminItem.Save();
		                    }
		                }
		            }
		        }
		        catch (Exception ex)
		        {
		            Logger.Instance.Error("��� ��������� ���������� � ������������ " + curUser.FullName + " ��������� ������.", ex);
		        }
		    }
		}

	    #endregion
	}
}
