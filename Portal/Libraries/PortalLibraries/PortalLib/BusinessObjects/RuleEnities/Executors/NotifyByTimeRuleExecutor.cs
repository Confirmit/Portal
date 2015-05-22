using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Properties;
using UlterSystems.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor
    {
        private readonly RuleRepository<NotifyByTimeRule> _ruleRepository;
        private IMailStorage _mailStorage;

        public NotifyByTimeRuleExecutor(RuleRepository<NotifyByTimeRule> ruleRepository, IMailStorage mailStorage)
        {
            _ruleRepository = ruleRepository;
            _mailStorage = mailStorage;
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
            return rules.Select(rule => GetMail(rule)).ToList();
        }

        private MailItem GetMail(NotifyByTimeRule rule)
        {
            return new MailItem
            {
                //TODO добавить остальные параментры
                ToAddress = Settings.Default.ErrorToAddress,
                MessageType = (int)MailTypes.CENotification,
                IsHTML = false,
                Body = rule.Information,
                
            };
        }

    }
}