using System;
using ConfirmIt.PortalLib.Notification;
using TestSendingNotRegisterUsers.Test_classes;
using UlterSystems.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    public class ProviderMethods
    {
        internal NotificationDelivery GetDelivery(IProviderWorkEvent providerEvent)
        {
            const int numberUsers = 5;
            var delivery = new NotificationDelivery
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
                MinTimeWork = new TimeSpan(0),
                ProviderUsers = new TestProviderUsers(numberUsers),
                ControllerNotification = new TestControllerNotification(true,true),
                ProviderWorkEvent = providerEvent
            };
            return delivery;
        }

        internal NotificationDelivery GetDelivery(IControllerNotification controller)
        {
            var delivery = GetDelivery(new TestProviderWorkEvent(null,null));
            delivery.ControllerNotification = controller;
            return delivery;
        }

        internal TestSender GetMailSender()
        {
            return new TestSender();
        }

        internal TestMailManager GetMailManager()
        {
            return new TestMailManager();
        }

        internal TestMailStorage GetMailStorage()
        {
            return new TestMailStorage();
        }
    }
}