using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Notification.NotRegisterNotification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.TestClasses;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class TestSendingLetters
    {
        private ProviderMethods _providerMethods = new ProviderMethods();

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsCountUsersPlusOneCurrentWork()
        {
            const int numberOfUsers = 5;
            const int countMailsToAdmin = 1;

            _providerMethods.NotRegisterUserProvider = new TestNotRegisteredUserProvider(numberOfUsers, 0);
            var delivery = _providerMethods.GetDelivery();
            delivery.DeliverNotification();
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, numberOfUsers + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyCountMailsEqualsDoubleCountUsersPlusOne()
        {
            const int numberOfUsers = 5;
            const int countMailsToAdmin = 1;

            _providerMethods.NotRegisterUserProvider = new TestNotRegisteredUserProvider(numberOfUsers, numberOfUsers);
            var delivery = _providerMethods.GetDelivery();
            delivery.DeliverNotification();

            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, true);
            Assert.AreEqual(storage.CountSavingLetters, numberOfUsers * 2 + countMailsToAdmin);
        }

        [TestMethod]
        public void AfterDeliverNotifyWithoutNotifyCountMailsEqualsZero()
        {
            _providerMethods.NotificationController = new TestControllerNotification(false, false);
            var delivery = _providerMethods.GetDelivery();
            delivery.DeliverNotification();
            var storage = _providerMethods.MailStorage as TestMailStorage;
            Assert.AreEqual(storage.IsSave, false);
            Assert.AreEqual(storage.CountSavingLetters, 0);
        }
    }
}
