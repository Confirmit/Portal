using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ExperationTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime)
        {
            return rule.RuleDetails.TimeInformation.LaunchTime + rule.RuleDetails.TimeInformation.ExpirationTime < currentDateTime;
        }
    }
}