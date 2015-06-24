using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ExperationTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime)
        {
            return timeInfo.TimeInformation.LaunchTime + timeInfo.TimeInformation.ExpirationTime > currentDateTime;
        }
    }
}