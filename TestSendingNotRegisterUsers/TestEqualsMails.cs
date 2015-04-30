using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSendingNotRegisterUsers.Test_classes;

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
            var providerUsers = delivery.ProviderUsers as TestProviderUsers;
            var countUser = providerUsers.NumberUsers;
            var mails = storage.GetMails(true);

            for (var i = 0; i < countUser; i++)
            {
                var neccessaryMail = mails[i];
                var expectedMail = _providerMethods.GetMailForUser(providerUsers.GetTestPerson(i));
                Assert.IsTrue(AreEqual(neccessaryMail, expectedMail));
            }
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
