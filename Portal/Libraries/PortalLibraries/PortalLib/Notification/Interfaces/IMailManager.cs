using System.Collections.Generic;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface IMailManager
    {
        void SendMails(IEnumerable<MailExpire> mailExpirations, IList<MailItem> mails);
        IMailSender MailSender { get; }
        IMailStorage MailStorage { get; }
    }
}
