using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotReportedToMoscowExecuter
    {
        private IEnumerable<Rule> _rules;
        private HashSet<int> _userIdentificates;
        private IGroupProvider _groupProvider;
        private IUserProvider _userProvider;
        public NotReportedToMoscowExecuter(IRuleProvider<NotReportingRuleToMoscow> providerRules, IGroupProvider groupProvider, IUserProvider userProvider)
        {
            _rules = providerRules.GetAllRules();
            _userIdentificates = new HashSet<int>();
            _userProvider = userProvider;
            _groupProvider = groupProvider;
        }

        public HashSet<int> GetUsersId()
        {
            if (_userIdentificates.Count != 0) return _userIdentificates;

            foreach (var rule in _rules)
            {
                var groups = _groupProvider.GetGroupsByRule(rule.ID.Value);
                foreach (var group in groups)
                {
                    _userIdentificates.UnionWith(_userProvider.GetUsersByGroup(group.ID.Value));
                }
            }
            return _userIdentificates;
        }
    }
}
