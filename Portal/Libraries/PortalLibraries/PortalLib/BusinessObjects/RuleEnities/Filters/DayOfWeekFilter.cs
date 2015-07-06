using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class DayOfWeekFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime)
        {
            return rule.TimeInformation.DaysOfWeek.Contains(currentDateTime.DayOfWeek);
        }
    }
}