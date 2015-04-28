using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    internal class TestSender: IMailSender
    {
        public int CountSendingMails;
        public bool IsSent;
        public void Send(MailMessage mail)
        {
            CountSendingMails++;
            IsSent = true;
        }
    }
}
