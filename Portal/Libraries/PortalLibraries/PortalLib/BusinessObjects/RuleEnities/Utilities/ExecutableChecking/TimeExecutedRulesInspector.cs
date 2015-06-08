using System;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public class TimeExecutedRulesInspector<T> : IExecutedRulesInspector<T> where T : Rule
    {
        private readonly IExecutedRuleRepository _executedRuleProvider;

        public TimeExecutedRulesInspector(IExecutedRuleRepository executedRuleProvider)
        {
            _executedRuleProvider = executedRuleProvider;
        }

        public virtual bool NeedToExecute(T rule, DateTime beginTime, DateTime endTime)
        {
            return rule.RuleDetails.TimeInformation.LaunchTime > beginTime && rule.RuleDetails.TimeInformation.LaunchTime < endTime && !IsExecutedRule(rule, beginTime, endTime);
        }

        private bool IsExecutedRule(Rule rule, DateTime begiTime, DateTime endTime)
        {
            var rules = _executedRuleProvider.GetExecutedRuleIds(begiTime, endTime);
            return rules.Any(item => item == rule.ID.Value);
        }
    }
}