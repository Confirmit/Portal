using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class MailProvider
    {
        private readonly string _fromAddress;
        private readonly MailTypes _messageType;
        private readonly IMailStorage _mailStorage;
        
        public MailProvider(string fromAddress, MailTypes messageType, IMailStorage mailStorage)
        {
            _fromAddress = fromAddress;
            _messageType = messageType;
            _mailStorage = mailStorage;
        }

        public void SaveMail(string toAddress, string subject, string body)
        {
            var mail = GetMailForUser(toAddress, subject, body);
            _mailStorage.SaveMail(mail);
        }

        private MailItem GetMailForUser(string toAddress, string subject, string body)
        {
            return new MailItem
            {
                FromAddress = _fromAddress,
                ToAddress = toAddress,
                Subject = subject,
                MessageType = (int)_messageType,
                IsHTML = false,
                Body = body
            };
        }
    }
}
