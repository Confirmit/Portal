using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestOfImplementersOfRules.Helpers
{
    public class RuleInstanceRepositoryFiller
    {
        public void FillRuleInstanceRepository(IRuleInstanceRepository ruleInstanceRepository, IEnumerable<Rule> rules, DateTime lastLaunchDateTime)
        {
            foreach (var rule in rules)
            {
                var ruleInstance = new RuleInstance(rule, lastLaunchDateTime) { Status = RuleStatus.Success };
                ruleInstanceRepository.SaveRuleInstance(ruleInstance);
            }
        }
    }
}