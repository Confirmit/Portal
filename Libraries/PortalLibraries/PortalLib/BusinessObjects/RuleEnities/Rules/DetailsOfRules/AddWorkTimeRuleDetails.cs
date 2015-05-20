using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class AddWorkTimeRuleDetails : RuleDetails
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public AddWorkTimeRuleDetails(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
        }
    }
}
