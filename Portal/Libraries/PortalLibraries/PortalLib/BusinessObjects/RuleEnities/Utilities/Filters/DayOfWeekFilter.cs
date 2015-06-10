using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class DayOfWeekFilter : RuleFilter
    {
        public override bool IsNeccessaryToExecute(Rule rule)
        {
            return base.IsNeccessaryToExecute(rule) && rule.RuleDetails.TimeInformation.DaysOfWeek.Contains(DateTime.Now.DayOfWeek);
        }
    }
}