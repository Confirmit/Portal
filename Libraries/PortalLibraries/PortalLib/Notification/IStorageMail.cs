using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IStorageMail
    {
        IList<MailItem> GetLetters(bool isSend);
        void SaveMail(MailItem letter);
    }
}
