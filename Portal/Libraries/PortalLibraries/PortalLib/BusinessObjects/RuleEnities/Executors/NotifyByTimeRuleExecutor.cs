using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;
using ConfirmIt.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor
    {
        private readonly IRuleRepository<NotifyByTimeRule> _ruleRepository;
        private readonly IMailStorage _mailStorage;
        private readonly ICheckExecuting<NotifyByTimeRule> _checkExecuting;
        private readonly MailProvider _mailProvider;

        public NotifyByTimeRuleExecutor(RuleRepository<NotifyByTimeRule> ruleRepository, MailProvider mailProvider, IMailStorage mailStorage, ICheckExecuting<NotifyByTimeRule> checkExecuting)
        {
            _ruleRepository = ruleRepository;
            _mailStorage = mailStorage;
            _checkExecuting = checkExecuting;
            _mailProvider = mailProvider;
        }

        public void GenerateAndSaveMails(DateTime beginTime, DateTime endTime)
        {
            var mails = GetMailsToReport(beginTime, endTime);
            foreach (var mail in mails)
            {
                _mailStorage.SaveMail(mail);
            }
        }
        
        private IList<MailItem> GetMailsToReport(DateTime beginTime, DateTime endTime)
        {
            var rules = _ruleRepository.GetAllRules().Where(rule =>_checkExecuting.IsExecute(rule, beginTime, endTime)).ToList();
            var mails = new List<MailItem>();

            foreach (var rule in rules)
            {
                var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
                _ruleRepository.SaveRule(rule);
                mails.AddRange(users.Select(user => _mailProvider.GetMailForUser(user.PrimaryEMail, rule.Subject, rule.Information)));
            }
            return mails;
        }
    }
}