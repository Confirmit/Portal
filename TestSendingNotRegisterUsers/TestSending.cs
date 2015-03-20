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
    public class TestSending
    {
        private NotificationDelivery getDelivery()
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


        [TestMethod]
        public void AfterInitializeCountSendingEqualsZero()
        {
            var sender = new TestSender();
            var manager = new TestMailManager(sender);
            manager.SendMessages(new List<MailExpire>(), new List<MailItem>());
            Assert.AreEqual(sender.countSendingLetters,0);
            Assert.IsFalse(sender.IsSend);
        }

        [TestMethod]
        public void AfterAddingFourLettersCountSendinglettersEqualsZero()
        {
            var sender = new TestSender();
            var manager = new TestMailManager(sender);
            var listMailItems = new List<MailItem>();
            for (int i = 0; i < 4; i++)
            {
                listMailItems.Add(new MailItem());
            }
            manager.SendMessages(new List<MailExpire>(), listMailItems);
            Assert.AreEqual(sender.countSendingLetters, 4);
            Assert.IsTrue(sender.IsSend);
        }
    }
}
