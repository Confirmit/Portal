using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Notification.Interfaces;
using ConfirmIt.PortalLib.Notification.NotRegisterNotification;
using Core;
using Core.Security;
using TestSendingNotRegisterUsers.TestClasses;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    public class ProviderMethods
    {
        public IMailStorage MailStorage { get; set; }
        public INotRegisterUserProvider NotRegisterUserProvider { get; set; }
        public INotificationController NotificationController { get; set; }

        public int NumberUsers = 5;

        public MailItem GetMailForUserNotRegisterToday(int userId)
        {
             var delivery = GetDelivery();
             var mail = GetMailItem(userId);
             mail.Body = Regex.Replace(delivery.MailRegisterToday, "_UserName_", " " + userId + " ");
            return mail;
        }

        public MailItem GetMailForUserNotRegisterYesterday(int userId)
        {
            var delivery = GetDelivery();
            var mail = GetMailItem(userId);
            mail.Body = Regex.Replace(delivery.MailRegisterYesterday, "_UserName_", " " + userId + " ");
            return mail;
        }

        private MailItem GetMailItem(int userId)
        {
            var delivery = GetDelivery();
            var mail = new MailItem
            {
                FromAddress = delivery.FromAddress,
                Subject = delivery.Subject,
                ToAddress = userId.ToString(),
                MessageType = (int) MailTypes.NRNotification
            };
            return mail;
        }

        public MailItem GetMailForAdmin()
        {
            var mail = GetMailItemAdmin();
            var delivery = GetDelivery();
            var notRegisteredYesterdayusers = NotRegisterUserProvider.GetNotRegisterUsers(DateTime.Now);
            var notRegisteredTodayusers = NotRegisterUserProvider.GetUsersWithShortMainWork(DateTime.Now, new TimeSpan());

            var adminMailBuilder = new AdminMailBodyBuilder();
            adminMailBuilder.AddSubject(delivery.MailAdminNotRegisterYesterday);

            foreach (var user in notRegisteredYesterdayusers)
            {
                adminMailBuilder.AddUserNote(" " + user.ID.Value + " ", user.ID.Value);
            }
            adminMailBuilder.AddSubject(delivery.MailAdminNotRegistredToday);
            foreach (var user in notRegisteredTodayusers)
            {
                adminMailBuilder.AddUserNote(" " + user.ID.Value + " ", user.ID.Value);
            }
            mail.Body = adminMailBuilder.ToString();
            return mail;

        }

        private MailItem GetMailItemAdmin()
        {
            var delivery = GetDelivery();
            var mail = new MailItem
            {
                FromAddress = delivery.FromAddress,
                Subject = delivery.SubjectAdmin,
                ToAddress = delivery.AddresAdmin,
                MessageType = (int)MailTypes.NRNotification
            };
            return mail;
        }

        public ProviderMethods()
        {
            MailStorage = new TestMailStorage();
            NotificationController = new TestControllerNotification(true, true);
            NotRegisterUserProvider = new TestNotRegisteredUserProvider(NumberUsers, NumberUsers);
        }

        internal NotificationDelivery GetDelivery()
        {
            var delivery = new NotificationDelivery(NotRegisterUserProvider, NotificationController, MailStorage)
            {
                FromAddress = "TestFromAddress",
                Subject = "TestSubject",
                SubjectAdmin = "TestSubjectAdmin",
                MailRegisterToday = "TestRegisterToday_UserName_",
                MailRegisterYesterday = "TestRegisterYesterday_UserName_",
                MailAdminNotRegisterYesterday = "TestAdminNotRegisterYesterday",
                MailAdminNotRegistredToday = "TestAdminNotRegisterToday",
                AddresAdmin = "TestAddressAdmin",
                MinWorkTime = new TimeSpan(0)                
            };
            return delivery;
        }
    }
}