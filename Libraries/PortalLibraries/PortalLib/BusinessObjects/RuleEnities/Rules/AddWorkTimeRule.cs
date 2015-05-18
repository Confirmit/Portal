using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class AddWorkTimeRule : Rule
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public override void BuildInstance(RuleInfo ruleInfo)
        {
            var info = ruleInfo as AddWorkTimeRuleInfo;
            DayOfWeek = info.DayOfWeek;
            Interval = info.Interval;
        }

        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }
        
        public AddWorkTimeRule() { }

        public AddWorkTimeRule(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            RuleInfo = new AddWorkTimeRuleInfo(interval, dayOfWeek);
        }
    }
}
