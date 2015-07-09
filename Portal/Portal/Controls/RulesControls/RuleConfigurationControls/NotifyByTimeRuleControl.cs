using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class NotifyByTimeRuleControl : UserControl
    {
        public NotifyByTimeRuleControl()
        {
            var table = new Table { Width = Unit.Percentage(100) };
            SubjectTextBox = new TextBox();
            AddRow("Subject:", SubjectTextBox, table);
            InformationTextBox = new TextBox();
            AddRow("Information:", InformationTextBox, table);
            Controls.Add(table);
        }

        private TextBox SubjectTextBox { get; set; }
        private TextBox InformationTextBox { get; set; }

        public string Subject
        {
            get { return SubjectTextBox.Text; }
            set { SubjectTextBox.Text = value; }
        }

        public string Information
        {
            get { return InformationTextBox.Text; }
            set { InformationTextBox.Text = value; }
        }

        private void AddRow(string labelContent, TextBox textBox, Table table)
        {
            var subjectLabel = new Label {Text = labelContent};
            var tableRow = new TableRow();
            var cellWithSubjectLabel = new TableCell { Width = Unit.Percentage(50) };
            cellWithSubjectLabel.Controls.Add(subjectLabel);
            tableRow.Cells.Add(cellWithSubjectLabel);

            var cellWithSubjectTextBox = new TableCell { Width = Unit.Percentage(50) };
            cellWithSubjectTextBox.Controls.Add(textBox);
            tableRow.Cells.Add(cellWithSubjectTextBox);

            table.Rows.Add(tableRow);
        }
    }
}