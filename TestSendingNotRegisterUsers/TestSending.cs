using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class TestSending
    {
        [TestMethod]
        public void AfterInitializeCountSendingEqualsZero()
        {
            var sender = new TestedSender();
            var manager = new TestMailManager(sender);
            manager.SendMessages(new List<MailExpire>(), new List<MailItem>());
            Assert.AreEqual(sender.countSendingLetters,0);
            Assert.IsFalse(sender.IsSend);
        }

        [TestMethod]
        public void AfterAddingFourLettersCountSendinglettersEqualsZero()
        {
            var sender = new TestedSender();
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
