using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class NotifyByTimeRuleControl : BaseRuleControl, IRuleInitializer, IRuleCreator, IRuleInitializable
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
            var subjectLabel = new Label { Text = labelContent };
            var tableRow = new TableRow();
            var cellWithSubjectLabel = new TableCell { Width = Unit.Percentage(50) };
            cellWithSubjectLabel.Controls.Add(subjectLabel);
            tableRow.Cells.Add(cellWithSubjectLabel);

            var cellWithSubjectTextBox = new TableCell { Width = Unit.Percentage(50) };
            cellWithSubjectTextBox.Controls.Add(textBox);
            tableRow.Cells.Add(cellWithSubjectTextBox);

            table.Rows.Add(tableRow);
        }

        public Rule InitializeRule(Rule rule, string description, TimeEntity timeInformation)
        {
            var notifyByTimeRule = (NotifyByTimeRule)rule;
            notifyByTimeRule.Description = description;
            notifyByTimeRule.TimeInformation = timeInformation;
            notifyByTimeRule.Subject = Subject;
            notifyByTimeRule.Information = Information;
            return notifyByTimeRule;
        }

        public Rule CreateRule(string description, TimeEntity timeInformation)
        {
            var notifyByTimeRule = new NotifyByTimeRule(description, Subject, Information, timeInformation);
            return notifyByTimeRule;
        }

        public void InitializeRuleControl(Rule rule)
        {
            var notifyByTimeRule = (NotifyByTimeRule)rule;
            Subject = notifyByTimeRule.Subject;
            Information = notifyByTimeRule.Information;
        }

        public override void Accept(RuleControlVisitor ruleControlVisitor)
        {
            ruleControlVisitor.Visit(this);
        }

        public override void DataBind(Rule rule)
        {
            //TODO
        }
    }
}