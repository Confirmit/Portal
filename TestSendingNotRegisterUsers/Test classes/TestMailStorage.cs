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
        public List<string> Addresses = new List<string>();

        public IList<MailItem> GetMails(bool isSent)
        {
            return new List<MailItem> {new MailItem(), new MailItem(), new MailItem()};
        }

        public void SaveMail(MailItem mail)
        {
            Addresses.Add(mail.ToAddress);
            IsSave = true;
            CountSavingLetters++;
        }
    }
}
