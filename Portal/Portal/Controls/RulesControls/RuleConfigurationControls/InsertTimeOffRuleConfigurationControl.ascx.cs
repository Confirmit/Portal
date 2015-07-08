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
            get
            {
                return new TimeSpan(TimeIntervalSelectorControl.Hours, TimeIntervalSelectorControl.Minutes, TimeIntervalSelectorControl.Seconds);
            }
            set
            {
                TimeIntervalSelectorControl.Hours = value.Hours;
                TimeIntervalSelectorControl.Minutes = value.Minutes;
                TimeIntervalSelectorControl.Seconds = value.Seconds;
            }
        }
    }
}