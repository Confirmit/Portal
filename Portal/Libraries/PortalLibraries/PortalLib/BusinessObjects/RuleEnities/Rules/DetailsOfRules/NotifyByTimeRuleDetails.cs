using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class NotifyByTimeRuleDetails : DateTimeRuleDetails
    {
        public string Information { get; set; }
        public string Subject { get; set; }

        public NotifyByTimeRuleDetails(string subject, string information, DateTime time, params DayOfWeek[] daysOfWeek) : base(time, daysOfWeek)
        {
            Subject = subject;
            Information = information;
        }
    }
}