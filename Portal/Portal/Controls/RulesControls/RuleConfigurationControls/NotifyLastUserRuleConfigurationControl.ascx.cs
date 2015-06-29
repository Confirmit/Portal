using System.Web.UI;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public partial class NotifyLastUserRuleConfigurationControl : UserControl
    {
        public int RuleId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        public string Subject
        {
            get { return SubjectTextBox.Text; }
            set { SubjectTextBox.Text = value; }
        }
    }
}