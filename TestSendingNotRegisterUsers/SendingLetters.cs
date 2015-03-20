using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;
using Core;
using Core.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                MessageRegisterToday = "",
                MessageRegisterYesterday = "",
                MessageAdminNotRegisterYesterday = "",
                MessageAdminNotRegistredToday = "",
                AddresAdmin = "",
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

        [TestMethod]
        public void AfterInitializeCountSendingEqualsZero()
        {
            var sender = GetMailSender();
            var manager = GetMailManager();
            manager.SendMessages(new List<MailExpire>(), new List<MailItem>());
            const int NumberSendingMessages = 0;

            Assert.AreEqual(sender.countSendingLetters, NumberSendingMessages);
            Assert.IsFalse(sender.IsSend);
        }

        [TestMethod]
        public void AfterAddingFourLettersCountSendinglettersEqualsZero()
        {
            var sender = GetMailSender();
            var manager = GetMailManager();
            manager.MailSender = sender;
            var listMailItems = new List<MailItem>();
            const int NumberSendingMessages = 4;

            for (int i = 0; i < NumberSendingMessages; i++)
            {
                listMailItems.Add(new MailItem());
            }
            manager.SendMessages(new List<MailExpire>(), listMailItems);
            Assert.AreEqual(sender.countSendingLetters, NumberSendingMessages);
            Assert.IsTrue(sender.IsSend);
        }
    }
}
