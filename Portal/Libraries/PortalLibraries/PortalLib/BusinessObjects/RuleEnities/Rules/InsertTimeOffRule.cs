using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class InsertTimeOffRule : Rule, IDateRule
    {
        public TimeSpan Interval { get; set; }
        public DateTime Time { get; set; }
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }
        
        public InsertTimeOffRule() { }

        public InsertTimeOffRule(TimeSpan interval, DateTime time, params DayOfWeek[] daysOfWeek)
        {
            Interval = interval;
            DaysOfWeek = new HashSet<DayOfWeek>(daysOfWeek);
            Time = time;
            RuleDetails = new InsertTimeOffRuleDetails(interval, time, daysOfWeek);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<InsertTimeOffRuleDetails>().GetInstance(XmlInformation);
            DaysOfWeek = ruleDetails.DaysOfWeek;
            Interval = ruleDetails.Interval;
            Time = ruleDetails.Time;
        }

        public override void Visit(Visitor visitor)
        {
            visitor.ExecuteRule(this);
        }
    }
}
