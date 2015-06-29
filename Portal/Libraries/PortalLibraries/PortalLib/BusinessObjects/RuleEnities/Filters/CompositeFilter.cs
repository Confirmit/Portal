using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public class CompositeRuleFilter : IRuleFilter
    {
        protected IList<IRuleFilter> Filters { get; set; }

        public CompositeRuleFilter(params IRuleFilter[] filters)
        {
            Filters = filters;
        }

        public bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime)
        {
            foreach (var ruleFilter in Filters)
            {
                if (!ruleFilter.IsNeccessaryToExecute(rule, currentDateTime))
                    return false;
            }
            return true;
        }

        public void Add(IRuleFilter ruleFilter)
        {
            Filters.Add(ruleFilter);
        }

        public void Remove(IRuleFilter ruleFilter)
        {
            Filters.Remove(ruleFilter);
        }
    }
}