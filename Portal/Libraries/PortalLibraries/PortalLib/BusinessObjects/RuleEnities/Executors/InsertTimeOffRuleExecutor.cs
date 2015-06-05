using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class InsertTimeOffRuleExecutor
    {
        private readonly IRuleRepository<InsertTimeOffRule> _ruleRepository;
        private readonly IExecutedRulesInspector<InsertTimeOffRule> _rulesInspector;
        private readonly IExecutedRuleRepository _executedRuleRepository;

        public InsertTimeOffRuleExecutor(IRuleRepository<InsertTimeOffRule> ruleRepository, 
            IExecutedRulesInspector<InsertTimeOffRule> rulesInspector, IExecutedRuleRepository executedRuleRepository)
        {
            _ruleRepository = ruleRepository;
            _rulesInspector = rulesInspector;
            _executedRuleRepository = executedRuleRepository;
        }

        public void InsertTimeOff(DateTime beginTime, DateTime endTime)
        {
            var rules = _ruleRepository.GetAllRules().Where(rule => 
                _rulesInspector.IsExecute(rule, beginTime, endTime)).ToList();

            foreach (var rule in rules)
            {
                ExecuteRule(rule);
            }
        }

        private void ExecuteRule(InsertTimeOffRule rule)
        {
            var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
            foreach (var user in users)
            {
                var userEvents = new UserWorkEvents(user.ID.Value);
                userEvents.AddLatestClosedWorkEvent(rule.Interval, WorkEventType.TimeOff);
            }
            _executedRuleRepository.SaveAsExecuted(rule);
        }
    }
}
