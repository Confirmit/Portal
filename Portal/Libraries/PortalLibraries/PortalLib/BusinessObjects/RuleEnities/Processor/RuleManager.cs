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
                DateTime? lastLaunchDateTime = _ruleInstanceRepository.GetLastLaunchDateTime(rule.ID.Value);
                DateTime launchTime;

                //the rule with this id never launched
                if (!lastLaunchDateTime.HasValue)
                {
                    launchTime = DateTime.Today + rule.RuleDetails.TimeInformation.LaunchTime.TimeOfDay;
                }
                else
                {
                    launchTime = lastLaunchDateTime.Value.Date + rule.RuleDetails.TimeInformation.LaunchTime.TimeOfDay;
                    launchTime = launchTime.AddDays(1);
                }

                for (; launchTime < DateTime.Now; launchTime = launchTime.AddDays(1))
                {
                    if(_ruleFilter.IsNeccessaryToExecute(rule, launchTime))
                        ruleInstances.Add(new RuleInstance(rule.ID.Value, launchTime));
                }
            }

            foreach (var ruleInstance in ruleInstances)
            {
                _ruleInstanceRepository.SaveRuleInstance(ruleInstance);
            }
        }

        public IList<RuleEntity> GetFilteredRules(DateTime currentDateTime)
        {
            var ruleEntities = _ruleInstanceRepository.GetWaitedRuleEntities();
            var filteredRules = new List<RuleEntity>();

            foreach (var ruleEntity in ruleEntities)
            {
                if (_ruleFilter.IsNeccessaryToExecute(ruleEntity.Rule, currentDateTime))
                {
                    filteredRules.Add(ruleEntity);
                }
                else
                {
                    ruleEntity.RuleInstance.Status = RuleStatus.Missed;
                    _ruleInstanceRepository.SaveRuleInstance(ruleEntity.RuleInstance);
                }
            }
            return filteredRules;
        }
    }
}