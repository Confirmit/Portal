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
        private ProviderMethods _providerMethods = new ProviderMethods();

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneCurrentWork()
        {            
            var workEvent = new WorkEvent {BeginTime = DateTime.Now.AddMilliseconds(-1), EndTime = DateTime.Now};
            _providerMethods.ProviderWorkEvent = new TestProviderWorkEvent(workEvent, null);

            var delivery = _providerMethods.GetDelivery();
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers + countMailsToAdmin);
            for (var i = 0; i < (delivery.ProviderUsers as TestProviderUsers).NumberUsers; i++)
            {
                Assert.AreEqual(storage.Addresses[i], i.ToString());
            }
            Assert.AreEqual(storage.Addresses[storage.Addresses.Count-1], delivery.AddresAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneYestMainWork()
        {
            
            var workEvent = new WorkEvent { BeginTime = DateTime.Now.AddMilliseconds(-1), EndTime = DateTime.Now };
            _providerMethods.ProviderWorkEvent = new TestProviderWorkEvent(null, workEvent);
            var delivery = _providerMethods.GetDelivery();
            
            
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsDoubleCountUsersPlusOne()
        {
            
            var delivery = _providerMethods.GetDelivery();            
            delivery.DeliverNotification();
            var countUsers = delivery.ProviderUsers.GetAllEmployees().Count;
            const int countMailsToAdmin = 1;
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, countUsers * 2 + countMailsToAdmin);
            for (var i = 0; i < (delivery.ProviderUsers as TestProviderUsers).NumberUsers*2; i++)
            {
                Assert.AreEqual(storage.Addresses[i], (i%5).ToString());
            }
            Assert.AreEqual(storage.Addresses[storage.Addresses.Count - 1], delivery.AddresAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyWithoutNotifyCountMailsEqualsZero()
        {
                       
            _providerMethods.ControllerNotification = new TestControllerNotification(false, false);
            var delivery = _providerMethods.GetDelivery();

            delivery.DeliverNotification();
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, false);
            Assert.AreEqual(storage.CountSavingLetters, 0);
        }
    }
}
