using System;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor
    {
        private readonly IRuleRepository<NotifyByTimeRule> _ruleRepository;
        private readonly IExecutedRulesInspector<NotifyByTimeRule> _ruleInspector;
        private readonly MailProvider _mailProvider;
        private readonly IExecutedRuleRepository _executedRuleRepository;

        public NotifyByTimeRuleExecutor(RuleRepository<NotifyByTimeRule> ruleRepository,
            MailProvider mailProvider,IExecutedRulesInspector<NotifyByTimeRule> checkExecuting, 
            IExecutedRuleRepository executedRuleRepository)
        {
            _executedRuleRepository = executedRuleRepository;
            _ruleRepository = ruleRepository;
            _ruleInspector = checkExecuting;
            _mailProvider = mailProvider;
        }

        public void GenerateAndSaveMails(DateTime beginTime, DateTime endTime)
        {
            var rules = _ruleRepository.GetAllRules().Where(rule =>
                _ruleInspector.IsExecute(rule, beginTime, endTime)).ToList();

            foreach (var rule in rules)
            {
                var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
                foreach (var user in users)
                {
                    _mailProvider.SaveMail(user.PrimaryEMail, rule.Subject, rule.Information);
                }
                _executedRuleRepository.SaveAsExecuted(rule);
            }
        }
    }
}