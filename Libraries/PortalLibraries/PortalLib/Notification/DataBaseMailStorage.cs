using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace ConfirmIt.PortalLib.Notification
{
    public class DataBaseMailStorage : IMailStorage
    {
        public IList<MailItem> GetMails(bool isSend)
        {
            return (BaseObjectCollection<MailItem>)BasePlainObject.GetObjects(typeof(MailItem), "IsSend", (object)isSend);
        }

        public void SaveMail(MailItem mail)
        {
            mail.Save();
        }
    }
}
