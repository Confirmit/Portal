using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class NotifyLastUserRuleControl : UserControl
    {
        public NotifyLastUserRuleControl()
        {
            var table = new Table();
            var subjectLabel = new Label { Text = "Subject:" };
            var tableRow = new TableRow();
            var cellWithSubjectLabel = new TableCell();
            cellWithSubjectLabel.Controls.Add(subjectLabel);
            tableRow.Cells.Add(cellWithSubjectLabel);

            SubjectTextBox = new TextBox();
            var cellWithSubjectTextBox = new TableCell();
            cellWithSubjectTextBox.Controls.Add(SubjectTextBox);
            tableRow.Cells.Add(cellWithSubjectTextBox);

            table.Rows.Add(tableRow);
            Controls.Add(table);
        }

        public string Subject
        {
            get { return SubjectTextBox.Text; }
            set { SubjectTextBox.Text = value; }
        }

        public TextBox SubjectTextBox { get; set; }
    }
}