using System;
using System.Web.UI;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public partial class InsertTimeOffRuleConfigurationControl : UserControl
    {
        public TimeSelectorControl TimeIntervalSelector
        {
            get { return TimeIntervalSelectorControl; }
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