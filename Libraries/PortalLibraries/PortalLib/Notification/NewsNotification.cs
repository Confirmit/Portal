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
    /// Класс, отвечающий за уведомление о новости.
    /// </summary>
    public class NewsNotification
    {
        #region Конструкторы.

        public NewsNotification(string message, int news_id)
        {
            //m_SMTPServer = Settings.Default.SMTPServer;
            //m_FromAddress = Settings.Default.NewsNotificationFromAddress;
            //m_Subject = Resources.NewsNotificationSubject;
            m_Message = message;
            m_NewsId = news_id;
        }
        #endregion

        #region Поля

        //private string m_Subject;
        private string m_Message;
        private int m_NewsId;

        #endregion

        #region Свойства
    /*    /// <summary>
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
        }*/

        /// <summary>
        /// Тема письма для рассылки статистики пользователя.
        /// </summary>
       /* public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }*/

        /// <summary>
        /// Текст письма.
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

        #region Методы
        /// <summary>
        /// Отсылает новость по электронной почте.
        /// </summary>
        public void SendNews()
        {
            // Получить список всех пользователей.
            Person[] users = UserList.GetUserList();
            if ((users == null) || (users.Length == 0))
                return;

            // Обработать всех пользователей.
            foreach (Person curUser in users)
            {
                // Пользователь без адреса почты не могут получить рассылку.
                if (string.IsNullOrEmpty(curUser.PrimaryEMail))
                    continue;

                // Не оповещать московских служащих.
                if (curUser.EmployeesUlterSYSMoscow)
                    continue;

                // Разослать уведомления.
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
