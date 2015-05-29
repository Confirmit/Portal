using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class InsertTimeOffRuleDetails : DateTimeRuleDetails
    {
        public TimeSpan Interval { get; set; }

        public InsertTimeOffRuleDetails() { }

        public InsertTimeOffRuleDetails(TimeSpan interval, DateTime time, params DayOfWeek[] daysOfWeek) : base(time, daysOfWeek)
        {
            Interval = interval;
        }
    }
}
