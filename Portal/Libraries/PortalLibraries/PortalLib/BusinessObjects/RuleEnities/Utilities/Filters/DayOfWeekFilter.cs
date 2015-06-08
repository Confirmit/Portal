using System;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class DayOfWeekFilter : IRuleFilter
    {
        public bool NeedToExecute(Rule rule)
        {
            return rule.RuleDetails.TimeInformation.DaysOfWeek.Contains(DateTime.Now.DayOfWeek);
        }
    }
}