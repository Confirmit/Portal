using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ActiveTimeFilter : IRuleFilter
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public ActiveTimeFilter(DateTime beginTime, DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public bool IsNeccessaryToExecute(Rule rule)
        {
            return(rule.RuleDetails.TimeInformation.LaunchTime > BeginTime &&
                   rule.RuleDetails.TimeInformation.LaunchTime < EndTime);
        }
    }
}