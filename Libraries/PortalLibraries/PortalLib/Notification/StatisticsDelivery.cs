using System;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.Notification;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// �����, ���������� �� �������� ���������.
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

        #endregion

        #region Methods

        /// <summary>
        /// �������� ����������.
        /// </summary>
        public void DeliverStatistics()
        {
            // �������� ������ ���� �������������.
            Person[] users = UserList.GetUserList();
            if (users == null || users.Length == 0)
                return;

            // ���������� ���� �������������.
            foreach (Person curUser in users)
            {
                // ������������� ��� ������ ����� �� ����� �������� ��������.
                if (string.IsNullOrEmpty(curUser.PrimaryEMail))
                    continue;

                // �������� �������� �� ������� �������� ������������.
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
        /// �������� ���������������� ����������.
        /// </summary>
        /// <param name="curUser">������������, �������� ���������� ����������.</param>
        /// <param name="delivery">�������� ��������.</param>
        private void SendUserStatistics(Person curUser, UserDelivery delivery)
        {
            if (curUser == null
                || curUser.ID == null
                || delivery == null)
                return;

            try
            {
                // �������� ���������� �� ������.
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

                // ������ ������������, ��� ���������� ������ ���� �������.
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

                // �������� ���������� �� �����.
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
        /// �������� ������� ����������.
        /// </summary>
        /// <param name="curUser">������������, �������� ���������� ����������.</param>
        /// <param name="delivery">�������� ��������.</param>
        private void SendOfficeStatistics(Person curUser, UserDelivery delivery)
        {
            if (curUser == null
                || curUser.ID == null
                || delivery == null)
                return;

            try
            {
                // �������� ���������� �� ������.
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

                // �������� ���������� �� �����.
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
