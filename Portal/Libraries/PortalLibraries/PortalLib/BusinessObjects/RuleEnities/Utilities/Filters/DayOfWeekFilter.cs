using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class DayOfWeekFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(Rule rule)
        {
            return rule.RuleDetails.TimeInformation.DaysOfWeek.Contains(DateTime.Now.DayOfWeek);
        }
    }
}