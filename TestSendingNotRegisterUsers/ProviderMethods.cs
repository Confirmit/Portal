using System;
using ConfirmIt.PortalLib.Notification;
using TestSendingNotRegisterUsers.Test_classes;
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
                FromAddress = "",
                Subject = "",
                SubjectAdmin = "",
                MailRegisterToday = "",
                MailRegisterYesterday = "",
                MailAdminNotRegisterYesterday = "",
                MailAdminNotRegistredToday = "",
                AddresAdmin = "",
                MinTimeWork = new TimeSpan(0)                
            };
            return delivery;
        }
    }
}