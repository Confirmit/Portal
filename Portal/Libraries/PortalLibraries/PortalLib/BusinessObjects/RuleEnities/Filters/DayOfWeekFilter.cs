using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class DayOfWeekFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime)
        {
            return timeInfo.TimeInformation.DaysOfWeek.Contains(currentDateTime.DayOfWeek);
        }
    }
}