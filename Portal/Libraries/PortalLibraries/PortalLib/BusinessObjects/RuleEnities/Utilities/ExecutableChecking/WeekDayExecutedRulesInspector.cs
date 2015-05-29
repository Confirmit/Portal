using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public class WeekDayExecutedRulesInspector : TimeExecutedRulesInspector<InsertTimeOffRule>
    {
        public WeekDayExecutedRulesInspector(IExecutedRuleRepository executedRuleProvider)
            : base(executedRuleProvider) { }

        public override bool IsExecute(InsertTimeOffRule rule, DateTime beginTime, DateTime endTime)
        {
            //return (beginTime.DayOfWeek == rule.DayOfWeek || endTime.DayOfWeek == rule.DayOfWeek) &&
            //       base.IsExecute(rule, beginTime, endTime);
            return true;
        }
    }
}