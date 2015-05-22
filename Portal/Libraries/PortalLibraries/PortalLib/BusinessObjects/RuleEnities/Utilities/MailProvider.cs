using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class MailProvider
    {
        private readonly string _fromAddress;
        private readonly MailTypes _messageType;
        
        public MailProvider(string fromAddress, MailTypes messageType)
        {
            _fromAddress = fromAddress;
            _messageType = messageType;
        }

        public MailItem GetMailForUser(string toAddress, string subject, string body)
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
