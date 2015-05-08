using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotReportedToMoscowExecuter
    {
        private IEnumerable<Rule> _rules;
        private HashSet<int> _userIdentificates;

        public NotReportedToMoscowExecuter(IRuleProvider<NotReportingRuleToMoscow> providerRules)
        {
            _rules = providerRules.GetAllRules();
            _userIdentificates = new HashSet<int>();
        }

        public HashSet<int> GetUsersId()
        {
            if (_userIdentificates.Count != 0) return _userIdentificates;

            foreach (var rule in _rules)
            {
                var groups = rule.GetUserGroups();
                foreach (var group in groups)
                {
                    _userIdentificates.UnionWith(group.GetUsersId());
                }
            }
            return _userIdentificates;
        }
    }
}
