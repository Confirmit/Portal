using System;
using System.Collections.Generic;
using System.Text;
using ConfirmIt.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;
using System.Text.RegularExpressions;
using ConfirmIt.PortalLib.Properties;

namespace UlterSystems.PortalLib.Notification
{
    /// <summary>
    /// �����, ���������� �� ����������� � �������.
    /// </summary>
    public class NewsNotification
    {
        #region ������������.

        public NewsNotification(string message, int news_id)
        {
            //m_SMTPServer = Settings.Default.SMTPServer;
            //m_FromAddress = Settings.Default.NewsNotificationFromAddress;
            //m_Subject = Resources.NewsNotificationSubject;
            m_Message = message;
            m_NewsId = news_id;
        }
        #endregion

        #region ����

        //private string m_Subject;
        private string m_Message;
        private int m_NewsId;

        #endregion

        #region ��������
    /*    /// <summary>
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
        }*/

        /// <summary>
        /// ���� ������ ��� �������� ���������� ������������.
        /// </summary>
       /* public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }*/

        /// <summary>
        /// ����� ������.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int NewsId
        {
            get { return m_NewsId; }
            set { m_NewsId = value; }
        }

        #endregion

        #region ������
        /// <summary>
        /// �������� ������� �� ����������� �����.
        /// </summary>
        public void SendNews()
        {
            // �������� ������ ���� �������������.
            Person[] users = UserList.GetUserList();
            if ((users == null) || (users.Length == 0))
                return;

            // ���������� ���� �������������.
            foreach (Person curUser in users)
            {
                // ������������ ��� ������ ����� �� ����� �������� ��������.
                if (string.IsNullOrEmpty(curUser.PrimaryEMail))
                    continue;

                // �� ��������� ���������� ��������.
                if (curUser.EmployeesUlterSYSMoscow)
                    continue;

                // ��������� �����������.
                string message = Message;
                message = Regex.Replace(message, "_UserName_", curUser.FullName);
                message = Regex.Replace(message, "_Date_", DateTime.Today.ToLongDateString());

                MailItem item = new MailItem
                                    {
                                        FromAddress = Settings.Default.NewsNotificationFromAddress,
                                        ToAddress = curUser.PrimaryEMail,
                                        Subject = Resources.NewsNotificationSubject + "(" + NewsId + ")",
                                        Body = message,
                                        MessageType = ((int) MailTypes.NewsNotification),
                                        IsHTML = true
                                    };
                item.Save();
            }
        }
        #endregion
    }
}
