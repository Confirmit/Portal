using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IMailManager
    {
        void SendMessages(IEnumerable<MailExpire> mailExpirations, IList<MailItem> letters);
        IMailSender MailSender { get; }
    }
}
