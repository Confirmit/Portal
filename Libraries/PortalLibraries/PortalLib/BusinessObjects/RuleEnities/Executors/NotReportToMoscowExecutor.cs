using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotReportToMoscowExecutor
    {
        private IRuleProvider<NotReportToMoscowRule> _ruleProvider;
        private IGroupProvider _groupProvider;
        public NotReportToMoscowExecutor(IRuleProvider<NotReportToMoscowRule> ruleProvider, IGroupProvider groupProvider)
        {
            _groupProvider = groupProvider;
            _ruleProvider = ruleProvider;
        }

        public HashSet<int> GetUsersId()
        {
            var userIds = new HashSet<int>();

            foreach (var rule in _ruleProvider.GetAllRules())
            {
                var groups = _ruleProvider.GetAllGroupsByRule(rule.ID.Value);
                foreach (var group in groups)
                {
                    userIds.UnionWith(_groupProvider.GetAllUserIdsByGroup(group.ID.Value));
                }
            }
            return userIds;
        }
    }
}
