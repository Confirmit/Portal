using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.TestClasses;

namespace TestSendingNotRegisterUsers
{
    [TestClass]
    public class TestEqualsMails
    {
        private ProviderMethods _providerMethods = new ProviderMethods();

        [TestMethod]
        public void AfterDeliverNotifyAdreessesMustMatch()
        {
            var delivery = _providerMethods.GetDelivery();
            delivery.DeliverNotification();
            var storage = _providerMethods.MailStorage as TestMailStorage;
            var notRegisteredUserProvider = _providerMethods.NotRegisterUserProvider as TestNotRegisteredUserProvider;
            var mails = storage.GetMails(true);
            var numberOfUsers = _providerMethods.NumberUsers;

            for (var i = 0; i < numberOfUsers; i++)
            {
                var neccessaryMail = mails[i];
                var expectedMail =
                    _providerMethods.GetMailForUserNotRegisterToday(notRegisteredUserProvider.NRTodayUserIds[i]);
                Assert.IsTrue(AreEqual(neccessaryMail, expectedMail));
            }
            for (var i = numberOfUsers; i < numberOfUsers * 2; i++)
            {
                var neccessaryMail = mails[i];
                var expectedMail = _providerMethods.GetMailForUserNotRegisterYesterday(notRegisteredUserProvider.NRYesterdayUserIds[(i % 5)]);
                Assert.IsTrue(AreEqual(neccessaryMail, expectedMail));
            }

            var expectedMailAdmin = _providerMethods.GetMailForAdmin();
            Assert.IsTrue(AreEqual(mails.Last(), expectedMailAdmin));
        }

        private bool AreEqual(MailItem mail1, MailItem mail2)
        {
            return mail1.Body == mail2.Body &&
                   mail1.Subject == mail2.Subject &&
                   mail1.FromAddress == mail2.FromAddress &&
                   mail1.ToAddress == mail2.ToAddress &&
                   mail1.MessageType == mail2.MessageType;
        }
    }
}
//"\r\nTestAdminNotRegisterYesterday\r\n1) FullName:  0 , ID: 0\r\n2) FullName:  1 , ID: 1\r\n3) FullName:  2 , ID: 2\r\n4) FullName:  3 , ID: 3\r\n5) FullName:  4 , ID: 4\r\n\r\nTestAdminNotRegisterToday\r\n1) FullName:  0 , ID: 0\r\n2) FullName:  1 , ID: 1\r\n3) FullName:  2 , ID: 2\r\n4) FullName:  3 , ID: 3\r\n5) FullName:  4 , ID: 4\r\n"
//"\r\nTestAdminNotRegisterYesterday\r\n1) FullName: 0 , ID: 0\r\n2) FullName: 1 , ID: 1\r\n3) FullName: 2 , ID: 2\r\n4) FullName: 3 , ID: 3\r\n5) FullName: 4 , ID: 4\r\n\r\nTestAdminNotRegisterToday\r\n1) FullName: 0 , ID: 0\r\n2) FullName: 1 , ID: 1\r\n3) FullName: 2 , ID: 2\r\n4) FullName: 3 , ID: 3\r\n5) FullName: 4 , ID: 4\r\n"