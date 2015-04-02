using System;
using TestSendingNotRegisterUsers.Test_classes;
using UlterSystems.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    public class ProviderMethods
    {
        internal NotificationDelivery GetDelivery()
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
                ControllerNotification = new TestControllerNotificationWithNotify()
            };
            return delivery;
        }

        internal NotificationDelivery GetDeliveryOnlyCurrentWorkEvent()
        {
            var delivery = GetDelivery();
            delivery.ProviderWorkEvent = new TestProviderWorkEventOnlyCurrent();
            return delivery;
        }

        internal NotificationDelivery GetDeliveryOnlyYestMainWorkEvent()
        {
            var delivery = GetDelivery();
            delivery.ProviderWorkEvent = new TestProviderWorkEventOnlyYesterdayMissingMainWork();
            return delivery;
        }

        internal NotificationDelivery GetDeliveryYestAndCurrent()
        {
            var delivery = GetDelivery();
            delivery.ProviderWorkEvent = new TestProviderWorkEventYestAndCurrent();
            return delivery;
        }

        internal NotificationDelivery GetDeliveryWithoutNotify()
        {
            var delivery = GetDelivery();
            delivery.ControllerNotification = new TestControllerNotificationWithoutNotify();
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