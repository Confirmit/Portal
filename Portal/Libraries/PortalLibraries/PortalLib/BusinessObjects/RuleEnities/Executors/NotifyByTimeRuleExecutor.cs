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
        //private readonly IExecutedRulesInspector<NotifyByTimeRule> _ruleInspector;
        private readonly MailProvider _mailProvider;
        

        public NotifyByTimeRuleExecutor(RuleRepository ruleRepository,
            MailProvider mailProvider,
            IExecutedRuleRepository executedRuleRepository)
            : base(executedRuleRepository)
        {
            _ruleRepository = ruleRepository;
            //_ruleInspector = checkExecuting;
            _mailProvider = mailProvider;
        }

        private void GenerateAndSaveMails(DateTime beginTime, DateTime endTime)
        {
            //var rules = _ruleRepository.GetAllRulesByType<NotifyByTimeRule>().Where(rule =>
              //  _ruleInspector.IsExecute(rule, beginTime, endTime)).ToList();

           // foreach (var rule in rules)
            //{
            //    ExecuteRule(rule);
            //}
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