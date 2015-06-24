using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyLastUserExecutor : RuleExecutor<NotifyLastUserRule>
    {
        private readonly IWorkEventTypeRecognizer _eventTypeRecognizer;
        private readonly IRuleRepository _ruleRepository;
       
        public MessageHelper MessageHelper { get; set; }
        public int UserId { get; set; }

        public NotifyLastUserExecutor(IWorkEventTypeRecognizer eventRecognizer, IRuleInstanceRepository ruleInstanceRepository, MessageHelper messageHelper, int userId)
            : base(ruleInstanceRepository)
        {
            MessageHelper = messageHelper;
            UserId = userId;
            _ruleRepository = ruleInstanceRepository.RuleRepository;
            _eventTypeRecognizer = eventRecognizer;
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
