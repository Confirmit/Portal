using System;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public abstract class TimeCheckExecuting<T> : ICheckExecuting<T> where T : Rule, ITimeRule
    {
        private readonly IExecutedRuleProvider _executedRuleProvider;

        protected TimeCheckExecuting(IExecutedRuleProvider executedRuleProvider)
        {
            _executedRuleProvider = executedRuleProvider;
        }

        public virtual bool IsExecute(T rule, DateTime beginTime, DateTime endTime)
        {
            return rule.Time > beginTime && rule.Time < endTime && IsExecutedRule(rule, beginTime, endTime);
        }

        private bool IsExecutedRule(Rule rule, DateTime begiTime, DateTime endTime)
        {
            var rules = _executedRuleProvider.GetExecutedRules(begiTime, endTime);
            return rules.Any(item => rule.ID.Value == rule.ID.Value);
        }
    }
}