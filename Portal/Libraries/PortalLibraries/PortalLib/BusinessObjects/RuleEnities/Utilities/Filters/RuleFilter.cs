using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public class RuleFilter
    {
        protected IList<RuleFilter> Filters { get; set; }

        public virtual bool IsNeccessaryToExecute(Rule rule)
        {
            foreach (var ruleFilter in Filters)
            {
                if (!ruleFilter.IsNeccessaryToExecute(rule))
                    return false;
            }
            return true;
        }

        public void Add(RuleFilter ruleFilter)
        {
            Filters.Add(ruleFilter);
        }

        public void Remove(RuleFilter ruleFilter)
        {
            Filters.Remove(ruleFilter);
        }
    }
}