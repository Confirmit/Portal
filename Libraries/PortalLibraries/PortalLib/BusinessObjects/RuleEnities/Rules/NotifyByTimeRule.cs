using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyByTimeRule : Rule
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyByTime; }
        }

        public NotifyByTimeRule(){}

        public NotifyByTimeRule(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
            RuleInfo = new NotifyByTimeRuleInfo(information, time, dayOfWeek);
        }

        public override void BuildInstance(RuleInfo ruleInfo)
        {
            var info = ruleInfo as NotifyByTimeRuleInfo;
            DayOfWeek = info.DayOfWeek;
            Time = info.Time;
            Information = info.Information;
        }
    }
}
