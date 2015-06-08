using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyLastUserExecutor : RuleExecutor<NotifyLastUserRule>
    {
        private readonly IActivityRuleChecking _checkingRule;
        private readonly IWorkEventTypeRecognizer _eventTypeRecognizer;
        private readonly IRuleRepository _ruleRepository;

        public string Subject { get; set; }
        public MessageHelper MessageHelper { get; set; }
        public int UserId { get; set; }

        public NotifyLastUserExecutor(IRuleRepository ruleRepository,
            IActivityRuleChecking ruleChecking,
            IWorkEventTypeRecognizer eventRecognizer, IExecutedRuleRepository executedRuleRepository)
            : base(executedRuleRepository)
        {
            _ruleRepository = ruleRepository;
            _checkingRule = ruleChecking;
            _eventTypeRecognizer = eventRecognizer;
        }


        public bool FillNotificationMessage(MessageHelper messageHelper, int userId, string subject)
        {
            var rulesForLastUser = GetRulesForLastUser(userId);
            if (!rulesForLastUser.Any()) return false;

            foreach (NotifyLastUserRule rule in rulesForLastUser)
            {
                messageHelper.AddNote(rule.Subject);
            }
            messageHelper.Subject = subject;
            return true;
        }

        private IList<NotifyLastUserRule> GetRulesForLastUser(int userId)
        {
            var activeRules = new List<NotifyLastUserRule>();

            foreach (var rule in _ruleRepository.GetAllRulesByType<NotifyLastUserRule>())
            {
                if (_checkingRule.IsActive(rule)
                    && _ruleRepository.IsUserExistedInRule(rule.ID.Value, userId)
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

        protected override void TryToExecuteRule(NotifyLastUserRule rule)
        {
            if (_ruleRepository.IsUserExistedInRule(rule.ID.Value, UserId) && IsLastActiveUser(rule))
                MessageHelper.AddNote(rule.Subject);
        }
    }
}
