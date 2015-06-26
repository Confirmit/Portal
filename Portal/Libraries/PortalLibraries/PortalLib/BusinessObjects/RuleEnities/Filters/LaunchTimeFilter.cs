using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class LaunchTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime)
        {
            return timeInfo.TimeInformation.LaunchTime <= currentDateTime;
        }
    }
}