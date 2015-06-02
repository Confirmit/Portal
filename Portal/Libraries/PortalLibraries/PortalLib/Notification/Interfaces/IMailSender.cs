using System.Net.Mail;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface IMailSender
    {
        void Send(MailMessage mail);
    }
}
