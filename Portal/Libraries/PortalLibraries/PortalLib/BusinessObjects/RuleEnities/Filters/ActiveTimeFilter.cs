using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ActiveTimeFilter : IRuleFilter
    {
        public bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime)
        {
            var launchTime = currentDateTime.Date + rule.TimeInformation.LaunchTime;
            return (launchTime > rule.TimeInformation.BeginTime && launchTime < rule.TimeInformation.EndTime);
        }
    }
}