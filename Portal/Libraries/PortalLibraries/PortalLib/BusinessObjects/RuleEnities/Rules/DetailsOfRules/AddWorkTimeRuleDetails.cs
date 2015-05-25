using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class AddWorkTimeRuleDetails : RuleDetails
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime Time { get; set; }

        public AddWorkTimeRuleDetails(TimeSpan interval, DayOfWeek dayOfWeek, DateTime time)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            Time = time;
        }
    }
}
