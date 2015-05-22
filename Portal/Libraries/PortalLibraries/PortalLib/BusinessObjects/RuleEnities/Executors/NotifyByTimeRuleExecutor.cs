using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

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
    }
}