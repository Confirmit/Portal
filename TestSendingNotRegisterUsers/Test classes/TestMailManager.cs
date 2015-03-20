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

        public IStorageMail StorageMail { get; set; }
       
        public void SendMessages(IEnumerable<MailExpire> mailExpirations, IList<MailItem> letters)
        {
            foreach (var mailItem in letters)
            {
                MailSender.Send(new MailMessage());
            }
        }
    }
}
