using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyLastUserExecutor
    {
        private readonly IActivityRuleChecking _checkingRule;
        private readonly IWorkEventTypeRecognizer _eventTypeRecognizer;
        private readonly IRuleRepository<NotifyLastUserRule> _ruleRepository;

        public NotifyLastUserExecutor(IRuleRepository<NotifyLastUserRule> ruleRepository, 
            IActivityRuleChecking ruleChecking,
            IWorkEventTypeRecognizer eventRecognizer)
        {
            _ruleRepository = ruleRepository;
            _checkingRule = ruleChecking;
            _eventTypeRecognizer = eventRecognizer;
        }
       

        public IList<NotifyLastUserRule> GetRulesForLastUser(int userId)
        {
            var activeRules = new List<NotifyLastUserRule>();

            foreach (var rule in _ruleRepository.GetAllRules())
            {
                if (_checkingRule.IsActive(rule) 
                    && _ruleRepository.IsUserExistsInRule(rule.ID.Value, userId) 
                    && IsLastActiveUser(rule))
                    activeRules.Add(rule);
            }
            return activeRules;
        }

        private bool IsLastActiveUser(Rule rule)
        {
            var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
            var countUsers = GetActiveUsersCount(users);
            if (countUsers == 1) return true;

            return false;
        }

        private int GetActiveUsersCount(IEnumerable<Person> users)
        {
            int countActiveUser = 0;
            foreach (var user in users)
            {
                if (_eventTypeRecognizer.GetType(user.ID.Value) == WorkEventType.TimeOff)
                    countActiveUser++;
            }
            return countActiveUser;
        }
    }
}
