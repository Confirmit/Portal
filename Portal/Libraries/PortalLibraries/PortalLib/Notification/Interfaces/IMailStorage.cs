using System.Collections.Generic;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface IMailStorage
    {
        IList<MailItem> GetMails(bool isSent);
        void SaveMail(MailItem mail);
    }
}
