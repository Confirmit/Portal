using System;
using System.Text.RegularExpressions;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;

namespace ConfirmIt.PortalLib.Notification.NotRegisterNotification
{
    public class MailRepository
    {
        public IMailStorage MailStorage { get; private set; }

        public MailRepository(IMailStorage mailStorage)
        {
            MailStorage = mailStorage;
        }

        public void CreateAndSaveMail(Person user, string fromAddress, string notificationText, string subjectForMail, DateTime dateTime)
        {
            Logger.Logger.Instance.Info("Notice sending to " + user.FullName + ".");

            string bodyMail = GetMailAfterChanging(notificationText, user, dateTime);
            var item = new MailItem
            {
                FromAddress = fromAddress,
                ToAddress = user.PrimaryEMail,
                Subject = subjectForMail,
                Body = bodyMail,
                MessageType = ((int)MailTypes.NRNotification)
            };
            MailStorage.SaveMail(item);
        }

        private string GetMailAfterChanging(string text, Person person, DateTime date)
        {
            text = Regex.Replace(text, "_UserName_", person.FullName);
            text = Regex.Replace(text, "_Date_", date.ToLongDateString());
            return text;
        }
    }
}