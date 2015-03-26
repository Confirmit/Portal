using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IMailManager
    {
        void SendMails(IEnumerable<MailExpire> mailExpirations, IList<MailItem> mails);
        IMailSender MailSender { get; }
        IStorageMail StorageMail { get; }
    }
}
