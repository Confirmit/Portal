using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ActiveTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime)
        {
            return (timeInfo.TimeInformation.LaunchTime > timeInfo.TimeInformation.BeginTime &&
                   timeInfo.TimeInformation.LaunchTime < timeInfo.TimeInformation.EndTime);
        }
    }
}