using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class AddWorkTimeRule : Rule
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }
        
        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }
        
        public AddWorkTimeRule() { }

        public AddWorkTimeRule(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            RuleDetails = new AddWorkTimeRuleDetails(interval, dayOfWeek);
        }

        public override void BuildInstance()
        {
            var ruleDetails = new SerializeHelper<AddWorkTimeRuleDetails>().GetInstance(XmlInformation);
            DayOfWeek = ruleDetails.DayOfWeek;
            Interval = ruleDetails.Interval;
        }
    }
}
