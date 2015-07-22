using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class NotifyLastUserRuleControl : UserControl, IRuleInitializer, IRuleCreator, IRuleInitializable
    {
        public NotifyLastUserRuleControl()
        {
            var table = new Table { Width = Unit.Percentage(100) };
            var subjectLabel = new Label { Text = "Subject:" };
            var tableRow = new TableRow();
            var cellWithSubjectLabel = new TableCell { Width = Unit.Percentage(50) };
            cellWithSubjectLabel.Controls.Add(subjectLabel);
            tableRow.Cells.Add(cellWithSubjectLabel);

            SubjectTextBox = new TextBox();
            var cellWithSubjectTextBox = new TableCell { Width = Unit.Percentage(50) };
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

        public Rule InitializeRule(Rule rule, string description, TimeEntity timeInformation)
        {
            var notifyLastUserRule = (NotifyLastUserRule)rule;
            notifyLastUserRule.Description = description;
            notifyLastUserRule.TimeInformation = timeInformation;
            notifyLastUserRule.Subject = Subject;
            return notifyLastUserRule;
        }

        public Rule CreateRule(string description, TimeEntity timeInformation)
        {
            var notifyLastUserRule = new NotifyLastUserRule(description, Subject, timeInformation);
            return notifyLastUserRule;
        }

        public void InitializeRuleControl(Rule rule)
        {
            var notifyLastUserRule = (NotifyLastUserRule)rule;
            Subject = notifyLastUserRule.Subject;
        }
    }
}