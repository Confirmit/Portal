using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestRuleInstanceRepository : IRuleInstanceRepository
    {
        public IList<RuleEntity> GetWaitedRuleEntities()
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastLaunchDateTime(int ruleId)
        {
            throw new NotImplementedException();
        }

        public void SaveRuleInstance(RuleInstance rule)
        {
            throw new NotImplementedException();
        }
    }
}