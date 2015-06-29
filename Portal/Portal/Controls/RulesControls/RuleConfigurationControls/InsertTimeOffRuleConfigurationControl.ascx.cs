using System;
using System.Web.UI;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public partial class InsertTimeOffRuleConfigurationControl : UserControl
    {
        public int RuleId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        public TimeSpan TimeInterval
        {
            get { return new TimeSpan(0, int.Parse(TimeIntervalTextBox.Text), 0, 0, 0); }
            set { TimeIntervalTextBox.Text = value.ToString(); }
        }
    }
}