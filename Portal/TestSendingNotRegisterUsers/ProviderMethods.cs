using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using ConfirmIt.PortalLib.Notification;
using Core;
using TestSendingNotRegisterUsers.Test_classes;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    public class ProviderMethods
    {
        public IMailStorage MailStorage { get; set; }
        public IUsersProvider ProviderUsers { get; set; }
        public INotificationController ControllerNotification { get; set; }
        public IWorkEventProvider ProviderWorkEvent { get; set; }

        public const int NumberUsers = 5;

        public MailItem GetMailForUserNotRegisterToday(Person user)
        {
             var delivery = GetDelivery();
            var mail = GetMailItem(user);
            mail.Body = Regex.Replace(delivery.MailRegisterToday, "_UserName_", user.FullName);
            return mail;
        }

        public MailItem GetMailForUserNotRegisterYesterday(Person user)
        {
            var delivery = GetDelivery();
            var mail = GetMailItem(user);
            mail.Body = Regex.Replace(delivery.MailRegisterYesterday, "_UserName_", user.FullName);
            return mail;
        }

        private MailItem GetMailItem(Person user)
        {
            var delivery = GetDelivery();
            var mail = new MailItem
            {
                FromAddress = delivery.FromAddress,
                Subject = delivery.Subject,
                ToAddress = user.FullName.Trim(),
                MessageType = (int) MailTypes.NRNotification
            };
            return mail;
        }

        public MailItem GetMailForAdmin()
        {
            var mail = GetMailItemAdmin();
            var users = ProviderUsers.GetAllEmployees();
            var delivery = GetDelivery();
            var body = new StringBuilder();
            body.AppendLine(delivery.MailAdminNotRegisterYesterday);
            
            for (int i = 0; i < users.Count; i++)
            {
                body.AppendLine(string.Format("{0}) FullName: {1}, ID: {2}", i + 1, users[i].FullName, users[i].ID));
            }
            body.AppendLine();
            body.AppendLine(delivery.MailAdminNotRegistredToday);
            
            for (int i = 5; i < users.Count*2; i++)
            {
                body.AppendLine(string.Format("{0}) FullName: {1}, ID: {2}", (i % users.Count + 1), users[i % users.Count].FullName, users[i % users.Count].ID));
            }
            body.AppendLine();
            mail.Body = body.ToString();
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
            ProviderUsers = new TestProviderUsers(NumberUsers);
            ProviderWorkEvent = new TestProviderWorkEvent(null, null);
            ControllerNotification = new TestControllerNotification(true, true);
        }

        internal NotificationDelivery GetDelivery()
        {            
            var delivery = new NotificationDelivery(ProviderUsers, ControllerNotification,ProviderWorkEvent,MailStorage)
            {
                SmtpServer = "",
                FromAddress = "TestFromAddress",
                Subject = "TestSubject",
                SubjectAdmin = "TestSubjectAdmin",
                MailRegisterToday = "TestRegisterToday_UserName_",
                MailRegisterYesterday = "TestRegisterYesterday_UserName_",
                MailAdminNotRegisterYesterday = "TestAdminNotRegisterYesterday",
                MailAdminNotRegistredToday = "TestAdminNotRegisterToday",
                AddresAdmin = "TestAddressAdmin",
                MinTimeWork = new TimeSpan(0)                
            };
            return delivery;
        }
    }
}