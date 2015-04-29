using System;
using System.Configuration;
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

        public MailItem GetMailForUser(Person user)
        {
            var delivery = GetDelivery();
            var mail = new MailItem
            {
                Body = Regex.Replace(delivery.MailRegisterToday, "_UserName_", string.Format(" {0} ",user.FirstName["en"])),
                FromAddress = delivery.FromAddress,
                Subject = delivery.Subject,
                ToAddress = user.FullName.Trim(),
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
                FromAddress = "TestAddress",
                Subject = "TestSubject",
                SubjectAdmin = "TestSubjectAdmin",
                MailRegisterToday = "TestRegisterToday_UserName_",
                MailRegisterYesterday = "TestRegisterYesterday _UserName_",
                MailAdminNotRegisterYesterday = "TestAdminNotRegisterYesterday",
                MailAdminNotRegistredToday = "TestAdminNotRegisterToday",
                AddresAdmin = "TestAddressAdmin",
                MinTimeWork = new TimeSpan(0)                
            };
            return delivery;
        }
    }
}