using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class ExperationTimeFilter : IRuleFilter
    {
        private readonly IExecutedRuleRepository _executedRuleRepository;

        public ExperationTimeFilter(IExecutedRuleRepository executedRuleRepository)
        {
            _executedRuleRepository = executedRuleRepository;
        }

        public bool IsNeccessaryToExecute(Rule rule)
        {
            TimeSpan gap;

            if (rule.RuleDetails.TimeInformation.IsRequired)
            {
                gap = new TimeSpan(1, 0, 0);
            }
            else
            {
                gap = rule.RuleDetails.TimeInformation.ExpirationTime;
            }
            
            var rulesInGap =
                _executedRuleRepository.GetExecutedRuleIds(DateTime.Now - gap, DateTime.Now);
            return !rulesInGap.Contains(rule.ID.Value);
        }
    }
}