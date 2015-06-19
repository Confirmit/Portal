using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class ExperationTimeFilter : IRuleFilter
    {
        private readonly IInstanceRuleRepository _instanceRuleRepository;

        public ExperationTimeFilter(IInstanceRuleRepository instanceRuleRepository)
        {
            _instanceRuleRepository = instanceRuleRepository;
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
                _instanceRuleRepository.GetExecutedRuleIds(DateTime.Now - gap, DateTime.Now);
            return !rulesInGap.Contains(rule.ID.Value);
        }
    }
}