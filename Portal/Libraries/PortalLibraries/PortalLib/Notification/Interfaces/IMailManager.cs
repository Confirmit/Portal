using System.Collections.Generic;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IMailManager
    {
        void SendMails(IEnumerable<MailExpire> mailExpirations, IList<MailItem> mails);
        IMailSender MailSender { get; }
        IMailStorage MailStorage { get; }
    }
}
