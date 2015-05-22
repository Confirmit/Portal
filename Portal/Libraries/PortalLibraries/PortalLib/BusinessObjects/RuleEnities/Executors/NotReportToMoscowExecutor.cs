using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotReportToMoscowExecutor
    {
        private IRuleRepository<NotReportToMoscowRule> _ruleRepository;
        public NotReportToMoscowExecutor(IRuleRepository<NotReportToMoscowRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public HashSet<int> GetUsersId()
        {
            var userIds = new HashSet<int>();

            foreach (var rule in _ruleRepository.GetAllRules())
            {
                userIds.UnionWith(_ruleRepository.GetAllUserIdsByRule(rule.ID.Value));
            }
            return userIds;
        }
    }
}
