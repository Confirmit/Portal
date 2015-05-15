using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.RuleInfo
{
    public class NotifyByTimeRuleInfo : IRuleInfo
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

    }
}