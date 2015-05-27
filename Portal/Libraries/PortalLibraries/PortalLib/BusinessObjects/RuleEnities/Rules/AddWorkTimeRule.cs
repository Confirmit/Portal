using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class AddWorkTimeRule : Rule, IDateRule
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime Time { get; set; }
        
        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }
        
        public AddWorkTimeRule() { }

        public AddWorkTimeRule(TimeSpan interval, DayOfWeek dayOfWeek, DateTime time)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            Time = time;
            RuleDetails = new AddWorkTimeRuleDetails(interval, dayOfWeek, time);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<AddWorkTimeRuleDetails>().GetInstance(XmlInformation);
            DayOfWeek = ruleDetails.DayOfWeek;
            Interval = ruleDetails.Interval;
            Time = ruleDetails.Time;
        }
    }
}
