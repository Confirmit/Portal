using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestRuleInstanceRepository : IRuleInstanceRepository
    {
        private List<RuleInstance> _ruleInstances = new List<RuleInstance>();
        private IRuleRepository _ruleRepository;
        private int ruleInstanceCount = 0;

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
            var rules = _ruleInstances.Where(ruleInstance => ruleInstance.RuleId == ruleId).OrderBy(instance => instance.LaunchTime);
            if(!rules.Any()) return null;

            return rules.Last().LaunchTime;
        }

        public void SaveRuleInstance(RuleInstance ruleInstance)
        {
            if (!ruleInstance.ID.HasValue)
                ruleInstance.ID = ruleInstanceCount++;
            
            _ruleInstances.RemoveAll(instance => ruleInstance.ID.Value == instance.ID.Value);
            _ruleInstances.Add(ruleInstance);
        }
    }
}