using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    class TestMailManager : IMailManager
    {
        public IMailSender MailSender { get; set; }

        public IMailStorage MailStorage { get; set; }
       
        public void SendMails(IEnumerable<MailExpire> mailExpirations, IList<MailItem> mails)
        {
            foreach (var mailItem in mails)
            {
                MailSender.Send(new MailMessage());
            }
        }
    }
}
