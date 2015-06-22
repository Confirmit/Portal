using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor
{
    public class RuleManager
    {
        private readonly IRuleInstanceRepository _ruleInstanceRepository;
        private readonly IRuleRepository _ruleRepository;
        private readonly IRuleFilter _ruleFilter;

        public RuleManager(IRuleInstanceRepository ruleInstanceRepository, IRuleRepository ruleRepository, IRuleFilter ruleFilter)
        {
            _ruleInstanceRepository = ruleInstanceRepository;
            _ruleRepository = ruleRepository;
            _ruleFilter = ruleFilter;
        }

        public void SaveValidRuleInstances()
        {
            var allRules = _ruleRepository.GetAllRules();
            var ruleInstances = new List<RuleInstance>();

            foreach (var rule in allRules)
            {
                var lastLaunchDateTime = _ruleInstanceRepository.GetLastLaunchDateTime(rule.ID.Value);
                for (DateTime currentDateTime = lastLaunchDateTime; currentDateTime < DateTime.Now; currentDateTime = currentDateTime.AddDays(1))
                {
                    if(_ruleFilter.IsNeccessaryToExecute(rule, currentDateTime))
                        ruleInstances.Add(new RuleInstance(rule.ID.Value, currentDateTime));
                }
            }

            foreach (var ruleInstance in ruleInstances)
            {
                _ruleInstanceRepository.SaveRuleInstance(ruleInstance);
            }
        }
    }
}