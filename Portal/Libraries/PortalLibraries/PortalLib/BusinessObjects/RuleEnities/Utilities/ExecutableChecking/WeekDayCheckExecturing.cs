using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public class WeekDayCheckExecturing : TimeCheckExecuting<AddWorkTimeRule>
    {
        public WeekDayCheckExecturing(IExecutedRuleProvider executedRuleProvider)
            : base(executedRuleProvider) { }

        public override bool IsExecute(AddWorkTimeRule rule, DateTime beginTime, DateTime endTime)
        {
            return (beginTime.DayOfWeek == rule.DayOfWeek || endTime.DayOfWeek == rule.DayOfWeek) &&
                   base.IsExecute(rule, beginTime, endTime);
        }
    }
}