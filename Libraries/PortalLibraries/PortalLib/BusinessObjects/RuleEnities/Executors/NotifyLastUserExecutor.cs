using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyLastUserExecutor
    {
        private IActivityRuleChecking _checkingRule;
        private IWorkEventTypeRecognizer _eventTypeRecognizer;
        private IGroupProvider _groupProvider;
        private IRuleProvider<NotifyLastUserRule> _ruleProvider;
        public string Subject { get; set; }
        

        public NotifyLastUserExecutor(IRuleProvider<NotifyLastUserRule> ruleProvider, IActivityRuleChecking ruleChecking,
            IWorkEventTypeRecognizer eventRecognizer, IGroupProvider groupProvider)
        {
            _ruleProvider = ruleProvider;
            _checkingRule = ruleChecking;
            _groupProvider = groupProvider;
            _eventTypeRecognizer = eventRecognizer;
        }
       

        public IList<NotifyLastUserRule> GetRulesForLastUser(int userId)
        {
            var activeRules = new List<NotifyLastUserRule>();

            foreach (var rule in _ruleProvider.GetAllRules())
            {
                if (_checkingRule.IsActive(rule) 
                    && _ruleProvider.IsUserExists(rule.ID.Value, userId) 
                    && IsLastUser(rule))
                    activeRules.Add(rule);
            }
            return activeRules;
        }

        public bool IsLastUser(int userId)
        {
            foreach (var rule in _ruleProvider.GetAllRules())
            {
                if (_checkingRule.IsActive(rule) 
                    && _ruleProvider.IsUserExists(rule.ID.Value, userId) 
                    && IsLastUser(rule))
                    return true;
            }
            return false;
        }

        private bool IsLastUser(Rule rule)
        {
            var users = GetAllUsers(rule);
            var countUsers = GetCountActiveUsers(users);
            if (countUsers == 1) return true;

            return false;
        }

        private IList<int> GetAllUsers(Rule rule)
        {
            var groups = _ruleProvider.GetAllGroupsByRule(rule.ID.Value);
            var allUsers = new HashSet<int>();
            foreach (var group in groups)
            {
                allUsers.UnionWith(_groupProvider.GetAllUserIdsByGroup(group.ID.Value));
            }
            return allUsers.ToList();
        }

        private int GetCountActiveUsers(IList<int> users)
        {
            int countActiveUser = 0;
            foreach (var userId in users)
            {
                if (_eventTypeRecognizer.GetType(userId) == WorkEventType.TimeOff)
                    countActiveUser++;
            }
            return countActiveUser;
        }
    }
}
