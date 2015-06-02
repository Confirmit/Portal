using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers.TestClasses
{
    class TestMailStorage : IMailStorage
    {
        public int CountSavingLetters;
        public bool IsSave;
        private List<MailItem> Mails = new List<MailItem>();

        public IList<MailItem> GetMails(bool isSent)
        {
            return Mails;
        }

        public void SaveMail(MailItem mail)
        {
            Mails.Add(mail);
            IsSave = true;
            CountSavingLetters++;
        }
    }
}
