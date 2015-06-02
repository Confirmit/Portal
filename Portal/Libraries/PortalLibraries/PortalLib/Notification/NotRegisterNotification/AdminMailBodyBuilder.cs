using System.Text;

namespace ConfirmIt.PortalLib.Notification.NotRegisterNotification
{
    public class AdminMailBodyBuilder
    {
        private StringBuilder _mailBody;
        private int _userCount = 1;

        public AdminMailBodyBuilder()
        {
            _mailBody = new StringBuilder();
        }

        public void AddSubject(string subject)
        {
            _userCount = 1;
            _mailBody.AppendLine();
            _mailBody.AppendLine(subject);
        }

        public void AddUserNote(string fullName, int userId)
        {
            _mailBody.AppendLine(string.Format("{0}) FullName: {1}, ID: {2}", _userCount, fullName, userId));
            _userCount++;
        }

        public override string ToString()
        {
            return _mailBody.ToString();
        }
    }
}
