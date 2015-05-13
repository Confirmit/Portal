using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotReportedToMoscowExecuter
    {
        private IEnumerable<Rule> _rules;
        private HashSet<int> _userIds;
        private IRuleProvider<NotReportingRuleToMoscow> _ruleProvider;
        private IGroupProvider _groupProvider;
        public NotReportedToMoscowExecuter(IRuleProvider<NotReportingRuleToMoscow> ruleProvider, IGroupProvider groupProvider)
        {
            _rules = ruleProvider.GetAllRules();
            _userIds = new HashSet<int>();
            _groupProvider = groupProvider;
            _ruleProvider = ruleProvider;
        }

        public HashSet<int> GetUsersId()
        {
            if (_userIds.Count != 0) return _userIds;

            foreach (var rule in _rules)
            {
                var groups = _ruleProvider.GetAllGroupsByRule(rule.ID.Value);
                foreach (var group in groups)
                {
                    _userIds.UnionWith(_groupProvider.GetAllUserIdsByGroup(group.ID.Value));
                }
            }
            return _userIds;
        }
    }
}
