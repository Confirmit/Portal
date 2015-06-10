using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class CompositeRuleFilter : IRuleFilter
    {
        protected IList<IRuleFilter> Filters { get; set; }

        public CompositeRuleFilter(params IRuleFilter[] filters)
        {
            Filters = filters;
        }

        public bool IsNeccessaryToExecute(Rule rule)
        {
            foreach (var ruleFilter in Filters)
            {
                if (!ruleFilter.IsNeccessaryToExecute(rule))
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