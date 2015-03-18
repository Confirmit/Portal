using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;

namespace TestSendingNotRegisterUsers
{
    internal class TestedSender: IMailSender
    {
        public int countSendingLetters;
        public void Send(MailMessage message)
        {
            countSendingLetters++;
        }
    }
}
