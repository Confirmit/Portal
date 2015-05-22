using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Properties;
using UlterSystems.PortalLib.Notification;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotifyByTimeRuleExecutor
    {
        private readonly RuleRepository<NotifyByTimeRule> _ruleRepository;
        public NotifyByTimeRuleExecutor(RuleRepository<NotifyByTimeRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public IList<NotifyByTimeRule> GetActiveRulesByTime(DateTime beginTime, DateTime endTime)
        {
            return _ruleRepository.GetAllRules().Where(rule => rule.Time > beginTime && rule.Time < endTime).ToList();
        }

        public IList<MailItem> GetMailsToReport(DateTime beginTime, DateTime endTime)
        {
            var result = new List<MailItem>();
            var rules = GetActiveRulesByTime(beginTime, endTime);
            foreach (var rule in rules)
            {
                result.Add(GetMail(rule));
            }

            return result;
        }

        public MailItem GetMail(NotifyByTimeRule rule)
        {
            return new MailItem
            {
                //TODO добавить остальные параментры

                ToAddress = Settings.Default.ErrorToAddress,
                MessageType = (int)MailTypes.CENotification,
                IsHTML = false,
                Body = rule.Information
            };
        }
    }
}