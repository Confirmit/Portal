using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotReportedToMoscowImplementer
    {
        private List<IRule> _rules;
        private IActivityRuleChecking _ruleChecking;
        private HashSet<int> _usersId;

        public NotReportedToMoscowImplementer(INotReportingToMoscowProvider providerRules, IActivityRuleChecking checkingRule)
        {
            _rules = providerRules.GetRules();
            _ruleChecking = checkingRule;
            _usersId = new HashSet<int>();
        }

        public HashSet<int> GetUsersId()
        {
            if (_usersId.Count != 0) return _usersId;

            foreach (var rule in _rules)
            {
                if (!_ruleChecking.IsActive(rule)) continue;
                AddUsersFromRule(rule);
            }
            return _usersId;
        }

        private void AddUsersFromRule(IRule rule)
        {
            var groups = rule.GetUserGroups();
            foreach (var group in groups)
            {
                _usersId.UnionWith(group.GetUsersId());
            }
        }
    }
}
