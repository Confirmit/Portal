using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class AddWorkTimeRuleDetails : RuleDetails
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public AddWorkTimeRuleDetails(TimeSpan interval, DayOfWeek dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
        }
    }
}
