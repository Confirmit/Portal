using System;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.Notification;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// Класс, отвечающий за рассылку статистик.
	/// </summary>
    public class StatisticsDelivery
    {
        #region Fields

        private string m_SMTPServer;
        private string m_FromAddress;
        private string m_Subject;
        private string m_SubjectAdmin;

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

        #endregion

        #region Methods

        /// <summary>
        /// Отсылает статистики.
        /// </summary>
        public void DeliverStatistics()
        {
            // Получить список всех пользователей.
            Person[] users = UserList.GetUserList();
            if (users == null || users.Length == 0)
                return;

            // Обработать всех пользователей.
            foreach (Person curUser in users)
            {
                // Пользователям без адреса почты не могут получить рассылку.
                if (string.IsNullOrEmpty(curUser.PrimaryEMail))
                    continue;

                // Получить рассылки на которые подписан пользователь.
                UserDelivery[] deliveries = UserDelivery.GetUserDeliveries(curUser.ID.Value);
                if (deliveries == null || deliveries.Length == 0)
                    continue;

                foreach (UserDelivery delivery in deliveries)
                {
                    switch (delivery.Delivery)
                    {
                        case Delivery.User:
                            SendUserStatistics(curUser, delivery);
                            break;

                        case Delivery.Office:
                            SendOfficeStatistics(curUser, delivery);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Отсылает пользовательскую статистику.
        /// </summary>
        /// <param name="curUser">Пользователь, которому отсылается статистика.</param>
        /// <param name="delivery">Описание рассылки.</param>
        private void SendUserStatistics(Person curUser, UserDelivery delivery)
        {
            if (curUser == null
                || curUser.ID == null
                || delivery == null)
                return;

            try
            {
                // Отослать статистику за неделю.
                DateTime end = DateTime.Today.AddDays(-1);
                DateTime begin = DateClass.WeekBegin(end);
                
                if (begin.Month < end.Month)
                    begin = new DateTime(end.Year, end.Month, 1, 0, 0, 0);

                MailItem item = new MailItem
                                    {
                                        FromAddress = FromAddress,
                                        ToAddress = curUser.PrimaryEMail,
                                        Subject = String.Format(Subject, curUser.FullName, begin, end),
                                        MessageType = ((int) MailTypes.UserStatistics)
                                    };

                // Узнать пользователя, чья статистика должна быть послана.
                Person statUser = curUser;
                if (delivery.StatisticsUserID != null)
                {
                    statUser = new Person();
                    if (!statUser.Load(delivery.StatisticsUserID.Value))
                        statUser = curUser;
                }

                PeriodUserStatistics stat = PeriodUserStatistics.GetUserStatistics(statUser, begin, end);
                switch (delivery.DeliveryPresentation)
                {
                    case DeliveryPresentation.XML:
                        item.IsHTML = false;
                        item.Body = stat.GetXMLPresentation();
                        break;

                    case DeliveryPresentation.HTML:
                        item.IsHTML = true;
                        item.Body = stat.GetHTMLPresentation();
                        break;
                }
                item.Save();

                // Отослать статистику за месяц.
                if (DateTime.Today.Day == 1)
                {
                    begin = new DateTime(end.Year, end.Month, 1, 0, 0, 0);
                    item.Subject = String.Format(Subject, curUser.FullName, begin, end);

                    stat = PeriodUserStatistics.GetUserStatistics(curUser, begin, end);
                    switch (delivery.DeliveryPresentation)
                    {
                        case DeliveryPresentation.XML:
                            item.IsHTML = false;
                            item.Body = stat.GetXMLPresentation();
                            break;

                        case DeliveryPresentation.HTML:
                            item.IsHTML = true;
                            item.Body = stat.GetHTMLPresentation();
                            break;
                    }

                    item.ID = null;
                    item.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Can't send user " + curUser.FullName + " statistcs.", ex);
            }
        }

        /// <summary>
        /// Отсылает офисную статистику.
        /// </summary>
        /// <param name="curUser">Пользователь, которому отсылается статистика.</param>
        /// <param name="delivery">Описание рассылки.</param>
        private void SendOfficeStatistics(Person curUser, UserDelivery delivery)
        {
            if (curUser == null
                || curUser.ID == null
                || delivery == null)
                return;

            try
            {
                // Отослать статистику за неделю.
                DateTime end = DateTime.Today.AddDays(-1);
                DateTime begin = DateClass.WeekBegin(end);
                
                if (begin.Month < end.Month)
                    begin = new DateTime(end.Year, end.Month, 1, 0, 0, 0);

                MailItem item = new MailItem
                                    {
                                        FromAddress = FromAddress,
                                        ToAddress = curUser.PrimaryEMail,
                                        Subject = String.Format(SubjectAdmin, begin, end),
                                        MessageType = ((int) MailTypes.OfficeStatistics)
                                    };

                PeriodOfficeStatistics stat = PeriodOfficeStatistics.GetOfficeStatistics(begin, end);
                switch (delivery.DeliveryPresentation)
                {
                    case DeliveryPresentation.XML:
                        item.IsHTML = false;
                        item.Body = stat.GetXMLPresentation();
                        break;

                    case DeliveryPresentation.HTML:
                        item.IsHTML = true;
                        item.Body = stat.GetHTMLPresentation();
                        break;
                }
                item.Save();

                // Отослать статистику за месяц.
                if (DateTime.Today.Day == 1)
                {
                    begin = new DateTime(end.Year, end.Month, 1, 0, 0, 0);
                    item.Subject = String.Format(SubjectAdmin, begin, end);

                    stat = PeriodOfficeStatistics.GetOfficeStatistics(begin, end);
                    switch (delivery.DeliveryPresentation)
                    {
                        case DeliveryPresentation.XML:
                            item.IsHTML = false;
                            item.Body = stat.GetXMLPresentation();
                            break;

                        case DeliveryPresentation.HTML:
                            item.IsHTML = true;
                            item.Body = stat.GetHTMLPresentation();
                            break;
                    }

                    item.ID = null;
                    item.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Can't send user " + curUser.FullName + " office statistics.", ex);
            }
        }

	    #endregion
    }
}
