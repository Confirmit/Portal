using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
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
