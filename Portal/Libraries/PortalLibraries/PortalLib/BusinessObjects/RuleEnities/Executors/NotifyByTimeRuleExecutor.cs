using System;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor : RuleExecutor<NotifyByTimeRule>
    {
        private readonly IRuleRepository _ruleRepository;
        private readonly MailProvider _mailProvider;
        

        public NotifyByTimeRuleExecutor(RuleRepository ruleRepository,
            MailProvider mailProvider,
            IRuleInstanceRepository ruleInstanceRepository)
            : base(ruleInstanceRepository)
        {
            _ruleRepository = ruleRepository;
            _mailProvider = mailProvider;
        }
        protected override void TryToExecuteRule(NotifyByTimeRule rule)
        {
            var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
            foreach (var user in users)
            {
                _mailProvider.SaveMail(user.PrimaryEMail, rule.Subject, rule.Information);
            }
        }
    }
}