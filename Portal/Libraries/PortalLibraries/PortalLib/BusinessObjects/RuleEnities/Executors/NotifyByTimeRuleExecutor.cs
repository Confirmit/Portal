using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor
    {
        private readonly RuleRepository<NotifyByTimeRule> _ruleRepository;
        private IMailStorage _mailStorage;
        private MailProvider _mailProvider;

        public NotifyByTimeRuleExecutor(RuleRepository<NotifyByTimeRule> ruleRepository, MailProvider mailProvider, IMailStorage mailStorage)
        {
            _ruleRepository = ruleRepository;
            _mailStorage = mailStorage;
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
            var rules = _ruleRepository.GetAllRules().Where(rule => rule.Time > beginTime && rule.Time < endTime).ToList();
            var mails = new List<MailItem>();

            foreach (var rule in rules)
            {
                var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
                mails.AddRange(users.Select(user => _mailProvider.GetMailForUser(user.PrimaryEMail, rule.Subject, rule.Information)));
            }
            return mails;
        }
    }
}