using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestRuleInstanceRepository : IRuleInstanceRepository
    {
        private HashSet<RuleInstance> _ruleInstances = new HashSet<RuleInstance>();
        private IRuleRepository _ruleRepository;

        public TestRuleInstanceRepository(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }


        public IList<RuleEntity> GetWaitedRuleEntities()
        {
            var ruleInstances = _ruleInstances.Where(ruleInstance => ruleInstance.Status == RuleStatus.Waiting);
            
            return (from ruleInstance in ruleInstances
                let rule = _ruleRepository.GetRuleById(ruleInstance.RuleId)
                select new RuleEntity(rule, ruleInstance)).ToList();
        }

        public DateTime? GetLastLaunchDateTime(int ruleId)
        {
            return
                _ruleInstances.Where(ruleInstance => ruleInstance.RuleId == ruleId)
                    .OrderBy(instance => instance.LaunchTime)
                    .Last()
                    .LaunchTime;
        }

        public void SaveRuleInstance(RuleInstance ruleInstance)
        {
            _ruleInstances.RemoveWhere(instance => ruleInstance.ID.Value == ruleInstance.ID.Value);
        }
    }
}