using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

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

        public bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime)
        {
            return(timeInfo.TimeInformation.LaunchTime > BeginTime &&
                   timeInfo.TimeInformation.LaunchTime < EndTime);
        }
    }
}