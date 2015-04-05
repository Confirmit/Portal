using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.Test_classes;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class SendingLetters
    {
        private readonly ProviderMethods _providerMethods = new ProviderMethods();

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneCurrentWork()
        {
            var workEvent = new WorkEvent {BeginTime = DateTime.Now.AddMilliseconds(-1), EndTime = DateTime.Now};
            
            var delivery = _providerMethods.GetDelivery(new TestProviderWorkEvent(workEvent, null));
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
            var workEvent = new WorkEvent { BeginTime = DateTime.Now.AddMilliseconds(-1), EndTime = DateTime.Now };

            var delivery = _providerMethods.GetDelivery(new TestProviderWorkEvent(null, workEvent));
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
            var delivery = _providerMethods.GetDelivery(new TestProviderWorkEvent(null,null));
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
            var delivery = _providerMethods.GetDelivery(new TestControllerNotification(false,false));
            var storage = _providerMethods.GetMailStorage();
            delivery.MailStorage = storage;
            delivery.DeliverNotification();
            Assert.AreEqual(storage.IsSave, false);
            Assert.AreEqual(storage.CountSavingLetters, 0);
        }
    }
}
