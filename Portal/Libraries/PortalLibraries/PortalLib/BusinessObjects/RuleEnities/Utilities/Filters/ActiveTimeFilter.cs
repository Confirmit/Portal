using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;


namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class ActiveTimeFilter : RuleFilter
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public ActiveTimeFilter(DateTime beginTime, DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public override bool IsNeccessaryToExecute(Rule rule)
        {
            return base.IsNeccessaryToExecute(rule) && (rule.RuleDetails.TimeInformation.LaunchTime > BeginTime &&
                   rule.RuleDetails.TimeInformation.LaunchTime < EndTime);
        }
    }
}