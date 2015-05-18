using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule
{
    public class AddWorkTimeRuleInfo : RuleInfo
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public AddWorkTimeRuleInfo(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
        }
    }
}
