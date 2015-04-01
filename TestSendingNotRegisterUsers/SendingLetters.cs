using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;
using Core;
using Core.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.Test_classes;
using UlterSystems.PortalLib.Notification;
using System.Configuration;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class SendingLetters
    {
        private NotificationDelivery GetDelivery()
        {
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
                MinTimeWork = new TimeSpan(0,1,0),
                ProviderUsers = new TestProviderUsers(),
                ProviderWorkEvent = new TestProviderWorkEvent(),
                ControllerNotification = new TestControllerNotification()
            };
            return delivery;
        }

        private TestSender GetMailSender()
        {
            return new TestSender();
        }

        private TestMailManager GetMailManager()
        {
            return new TestMailManager();
        }

        private TestMailStorage GetMailStorage()
        {
            return new TestMailStorage();
        }

        [TestMethod]
        public void AfterInitializeCountSendingEqualsZero()
        {
            var sender = GetMailSender();
            var manager = GetMailManager();
            manager.SendMails(new List<MailExpire>(), new List<MailItem>());
            const int NumberSendingMails = 0;

            Assert.AreEqual(sender.CountSendingMails, NumberSendingMails);
            Assert.IsFalse(sender.IsSend);
        }

        [TestMethod]
        public void AfterAddingFourLettersCountSendinglettersEqualsZero()
        {
            var sender = GetMailSender();
            var manager = GetMailManager();
            manager.MailSender = sender;
            var listMailItems = new List<MailItem>();
            const int NumberSendingMails = 4;

            for (int i = 0; i < NumberSendingMails; i++)
            {
                listMailItems.Add(new MailItem());
            }
            manager.SendMails(new List<MailExpire>(), listMailItems);
            Assert.AreEqual(sender.CountSendingMails, NumberSendingMails);
            Assert.IsTrue(sender.IsSend);
        }


        [TestMethod]
        public void DeliverUsers()
        {
            var delivery = GetDelivery();
            var storage = GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.countSavingLetters, countUsers*2 +1);
        }
    }
}
