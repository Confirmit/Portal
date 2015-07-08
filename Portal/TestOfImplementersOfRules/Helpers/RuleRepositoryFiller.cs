using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using TestOfImplementersOfRules.Factories;

namespace TestOfImplementersOfRules.Helpers
{
    public class RuleRepositoryFiller
    {
        public void FillRuleRepository(IRuleRepository ruleRepository, IEnumerable<Rule> rules)
        {
            var groupIds = ruleRepository.GroupRepository.GetAllGroups().Select(group => group.ID.Value).ToArray();

            foreach (var rule in rules)
            {
                ruleRepository.SaveRule(rule);
                ruleRepository.AddGroupIdsToRule(rule.ID.Value, groupIds);
            }
        }
    }
}