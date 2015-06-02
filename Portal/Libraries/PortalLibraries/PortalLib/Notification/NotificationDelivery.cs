using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification.Interfaces;
using ConfirmIt.PortalLib.Notification.NotRegisterNotification;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.Notification;

namespace UlterSystems.PortalLib.Notification
{
    /// <summary>
    /// ����� �������� ����������� �� ���������� �������� ���������.
    /// </summary>
    public class NotificationDelivery
    {
        #region Properties
        public IMailStorage MailStorage { get; private set; }
        public INotRegisterUserProvider NotRegisterUsersProvider { get; private set; }
        public INotificationController NotificationController { get; private set; }
       
        
        /// <summary>
        /// ����������� ����� ������ ���������� � �����, ����� ����� ������� ��� ��������������
        /// </summary>
        public TimeSpan MinWorkTime { get; set; }
        
        /// <summary>
        /// �������� �����.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// ���� ������ ��� �������� ���������� ������������.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// ���� ������ ��� �������� ���������� �����.
        /// </summary>
        public string SubjectAdmin { get; set; }

        /// <summary>
        /// ����� ������ �� �� ������� �������.
        /// </summary>
        public string MailRegisterToday { get; set; }

        /// <summary>
        /// ����� ������ �� �� ������� �����.
        /// </summary>
        public string MailRegisterYesterday { get; set; }

        /// <summary>
        /// ����� ������ ��� ��������������.
        /// </summary>
        public string MailAdminNotRegisterYesterday { get; set; }

        /// <summary>
        /// ����������� ��� ��� ����� ������, � ������� ��������� ������ �� ������������ �������
        /// </summary>
        public string MailAdminNotRegistredToday { get; set; }

        /// <summary>
        /// ������ ��������������, ��� ����������� ������ �� ������������
        /// </summary>
        public string AddresAdmin { get; set; }

        #endregion


        public NotificationDelivery(INotRegisterUserProvider notRegisteredUsersProvider, INotificationController notificatioControler, IMailStorage mailStorage)
        {
            NotRegisterUsersProvider = notRegisteredUsersProvider;
            NotificationController = notificatioControler;
            MailStorage = mailStorage;
        }
        
        /// <summary>
        /// ��������� ����������� �� ���������� ������� ����������.
        /// </summary>
        public void DeliverNotification()
        {
            var notRegisterTodayUsers = NotRegisterUsersProvider.GetNotRegisterUsers(DateTime.Now);
            var notRegisterYesterdayUsers = NotRegisterUsersProvider.GetUsersWithShortMainWork(DateTime.Now.AddDays(-1),
                MinWorkTime);

            RemoveAllUserWhichNotNotified(notRegisterTodayUsers);
            RemoveAllUserWhichNotNotified(notRegisterYesterdayUsers);

            CreateAndSaveBodyMailToAdmin(notRegisterTodayUsers, notRegisterYesterdayUsers);
            CreateAndSaveMailsForNotRegister(notRegisterTodayUsers, DateTime.Now, MailRegisterToday);
            CreateAndSaveMailsForNotRegister(notRegisterYesterdayUsers, DateTime.Now.AddDays(-1), MailRegisterYesterday);
        }

        private void RemoveAllUserWhichNotNotified(List<Person> users)
        {
            users.RemoveAll(user => !NotificationController.IsNotified(user));
        }

        private void CreateAndSaveBodyMailToAdmin(IList<Person> notRegisterTodayUsers, IList<Person> notRegisterYesterdayUsers)
        {
            var adminMailBuilder = new AdminMailBodyBuilder();

            if (notRegisterYesterdayUsers.Count != 0)
            {
                var mailToAdminYesterday = Regex.Replace(MailAdminNotRegisterYesterday, "_Date_",
                    DateTime.Today.AddDays(-1).ToLongDateString());
                adminMailBuilder.AddSubject(mailToAdminYesterday);
                foreach (var user in notRegisterYesterdayUsers)
                {
                    adminMailBuilder.AddUserNote(user.FullName, user.ID.Value);
                }
            }

            if (notRegisterTodayUsers.Count != 0)
            {
                var mailToAdminToday = Regex.Replace(MailAdminNotRegistredToday, "_Date_",
                    DateTime.Today.ToLongDateString());
                adminMailBuilder.AddSubject(mailToAdminToday);
                foreach (var user in notRegisterTodayUsers)
                {
                    adminMailBuilder.AddUserNote(user.FullName, user.ID.Value);
                }
            }
            if (!notRegisterTodayUsers.Any() && !notRegisterYesterdayUsers.Any()) return;

            Logger.Instance.Info("Notice sending to administrator E-Mail " + AddresAdmin + ".");
            SaveMailItem(AddresAdmin, adminMailBuilder.ToString(), SubjectAdmin);
        }

        /// <summary>
        /// ������ � ��������� ��������� ��� ������� ��� �� ��������� �����
        /// </summary>
        private void CreateAndSaveMailsForNotRegister(IList<Person> users, DateTime date, string notificationText)
        {
            foreach (var person in users)
            {
                Logger.Instance.Info("Notice sending to " + person.FullName + ".");
                string bodyMail = GetMailAfterChanging(notificationText, person, date);

                SaveMailItem(person.PrimaryEMail, bodyMail, Subject);
            }
        }

        private string GetMailAfterChanging(string text, Person person, DateTime date)
        {
            text = Regex.Replace(text, "_UserName_", person.FullName);
            text = Regex.Replace(text, "_Date_", date.ToLongDateString());
            return text;
        }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        /// <param name="toAddress">������ ����������</param>
        /// <param name="bodyMail">���� ���������</param>
        /// <param name="subject">���� ���������</param>
        private void SaveMailItem(string toAddress, string bodyMail, string subject)
        {
            var item = new MailItem
            {
                FromAddress = this.FromAddress,
                ToAddress = toAddress,
                Subject = subject,
                Body = bodyMail,
                MessageType = ((int)MailTypes.NRNotification)
            };
            MailStorage.SaveMail(item);
        }

        
       
    }
}





