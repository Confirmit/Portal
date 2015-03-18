using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ConfirmIt.PortalLib.Properties;

namespace ConfirmIt.PortalLib.Notification
{
    public class SmtpSender: IMailSender
    {
        private SmtpClient _client;

        public SmtpSender(string smtpServer)
        {
            if (string.IsNullOrEmpty(smtpServer))
                throw new ArgumentNullException("smtpServer", Resources.SMTPServerIsNotSet);

            _client = new SmtpClient(smtpServer);
        }
        public void Send(MailMessage message)
        {
            _client.Send(message);
        }
    }
}
