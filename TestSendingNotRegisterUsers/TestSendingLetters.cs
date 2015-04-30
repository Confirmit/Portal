using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.Test_classes;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class TestSendingLetters
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
