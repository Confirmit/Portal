using System.Web.UI;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public partial class NotifyByTimeRuleConfigurationControl : UserControl
    {
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
    }
}