using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class MessageHelper
    {
        private StringBuilder _body;

        private int _countNotes;

        public string Subject { get; set; }

        public string Body
        {
            get { return _body.ToString(); }
        }

        public MessageHelper()
        {
            _body = new StringBuilder();
            _countNotes = 1;
        }

        public MessageHelper(string subject)
            : this()
        {
            Subject = subject;
        }

        public void AddNote(string note)
        {
            _body.AppendLine(string.Format("{0}) {1}", _countNotes, note));
            _countNotes++;
        }
    }
}