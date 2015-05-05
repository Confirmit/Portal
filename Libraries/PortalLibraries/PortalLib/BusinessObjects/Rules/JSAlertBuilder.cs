using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class JSAlertBuilder
    {
        private StringBuilder _body;

        private const string BEGIN = "<script type='text/javascript'> alert('";
        private const string END = "')</script>";
        private int countNotes = 1;
 
        public string Subject { get; set; }

        public string Body
        {
            get { return _body.ToString(); }
        }

        public JSAlertBuilder()
        {
            _body = new StringBuilder();
        }

        public JSAlertBuilder(string subject) : this()
        {
            Subject = subject;
        }

        public void AddNote(string note)
        {
            _body.AppendLine(string.Format("{0}) {1}",countNotes,note));
        }

        public override string ToString()
        {
            return BEGIN + Subject + Body + END;
        }
    }
}