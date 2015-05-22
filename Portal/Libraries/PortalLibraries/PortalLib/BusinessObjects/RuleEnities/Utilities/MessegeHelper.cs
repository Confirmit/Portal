using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class MessageHelper
    {
        private StringBuilder _body;

        private int countNotes = 1;

        public string Subject { get; set; }

        public string Body
        {
            get { return _body.ToString(); }
        }

        public MessageHelper()
        {
            _body = new StringBuilder();
        }

        public MessageHelper(string subject)
            : this()
        {
            Subject = subject;
        }

        public void AddNote(string note)
        {
            _body.AppendLine(string.Format("{0}) {1}", countNotes, note));
            countNotes++;
        }
    }
}