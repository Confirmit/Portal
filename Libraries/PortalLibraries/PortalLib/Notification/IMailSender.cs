using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IMailSender
    {
        void Send(MailMessage message);
    }
}
