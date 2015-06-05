using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyByTimeRule : Rule, IDateRule
    {
        public string Subject { get; set; }
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyByTime; }
        }

        public NotifyByTimeRule(){}

        public NotifyByTimeRule(string subject, string information, DateTime time, params DayOfWeek[] daysOfWeek)
        {
            Subject = subject;
            Information = information;
            Time = time;
            DaysOfWeek = new HashSet<DayOfWeek>(daysOfWeek);
            RuleDetails = new NotifyByTimeRuleDetails(subject,information, time, daysOfWeek);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<NotifyByTimeRuleDetails>().GetInstance(XmlInformation);
            DaysOfWeek = ruleDetails.DaysOfWeek;
            Time = ruleDetails.Time;
            Information = ruleDetails.Information;
            Subject = ruleDetails.Subject;
        }

        public override void Visit(Visitor visitor)
        {
            visitor.ExecuteRule(this);
        }
    }
}
