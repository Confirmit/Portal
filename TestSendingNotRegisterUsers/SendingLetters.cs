using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class SendingLetters
    {
        private readonly ProviderMethods _providerMethods = new ProviderMethods();

        [TestMethod]
        public void AfterInitializeCountSendingEqualsZero()
        {
            var sender = _providerMethods.GetMailSender();
            var manager = _providerMethods.GetMailManager();
            manager.SendMails(new List<MailExpire>(), new List<MailItem>());
            const int NumberSendingMails = 0;

            Assert.AreEqual(sender.CountSendingMails, NumberSendingMails);
            Assert.IsFalse(sender.IsSend);
        }

        [TestMethod]
        public void AfterAddingFourLettersCountSendinglettersEqualsZero()
        {
            var sender = _providerMethods.GetMailSender();
            var manager = _providerMethods.GetMailManager();
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
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneCurrentWork()
        {
            var delivery = _providerMethods.GetDeliveryOnlyCurrentWorkEvent();
            var storage = _providerMethods.GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneYestMainWork()
        {
            var delivery = _providerMethods.GetDeliveryOnlyYestMainWorkEvent();
            var storage = _providerMethods.GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsDoubleCountUsersPlusOne()
        {
            var delivery = _providerMethods.GetDeliveryYestAndCurrent();
            var storage = _providerMethods.GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers*2 + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyWithoutNotifyCountMailsEqualsZero()
        {
            var delivery = _providerMethods.GetDeliveryWithoutNotify();
            var storage = _providerMethods.GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            Assert.AreEqual(storage.IsSave, false);
            Assert.AreEqual(storage.CountSavingLetters, 0);
        }
    }
}
