using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    class TestMailManager : IMailManager
    {
        public IMailSender MailSender { get; private set; }

        public IStorageMail StorageMail { get; set; }

        public TestMailManager(IMailSender mailSender)
        {
            MailSender = mailSender;
        }
        public void SendMessages(IEnumerable<MailExpire> mailExpirations, IList<MailItem> letters)
        {
            foreach (var mailItem in letters)
            {
                MailSender.Send(mailItem.GetMailMessage());
            }
        }
    }
}
