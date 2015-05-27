using System;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class AddWorkTimeRuleExecutor
    {
        private readonly IRuleRepository<AddWorkTimeRule> _ruleRepository;
        private readonly IExecutedRulesInspector<AddWorkTimeRule> _rulesInspector;
        private readonly IExecutedRuleRepository _executedRuleRepository;

        public AddWorkTimeRuleExecutor(IRuleRepository<AddWorkTimeRule> ruleRepository, IExecutedRulesInspector<AddWorkTimeRule> rulesInspector, IExecutedRuleRepository executedRuleRepository)
        {
            _ruleRepository = ruleRepository;
            _rulesInspector = rulesInspector;
            _executedRuleRepository = executedRuleRepository;
        }

        public void AddWorkTime(DateTime beginTime, DateTime endTime)
        {
            var rules = _ruleRepository.GetAllRules().Where(rule => _rulesInspector.IsExecute(rule, beginTime, endTime)).ToList();
            foreach (var rule in rules)
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
}
