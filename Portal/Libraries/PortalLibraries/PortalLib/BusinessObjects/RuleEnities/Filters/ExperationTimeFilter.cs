using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ExperationTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime)
        {
            return currentDateTime.Date +  rule.TimeInformation.LaunchTime + rule.TimeInformation.ExpirationTime > DateTime.Now;
        }
    }
}