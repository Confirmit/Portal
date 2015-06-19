using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyByTimeRule : Rule
    {
        public string Subject { get; set; }
        public string Information { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyByTime; }
        }

        public NotifyByTimeRule(){}

        public NotifyByTimeRule(string subject, string information, TimeEntity timeInformation)
        {
            Subject = subject;
            Information = information;
            RuleDetails = new NotifyByTimeRuleDetails(subject, information, timeInformation);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<NotifyByTimeRuleDetails>().GetInstance(XmlInformation);
            Information = ruleDetails.Information;
            Subject = ruleDetails.Subject;
            RuleDetails = ruleDetails;
        }

        public override void Visit(RuleVisitor ruleVisitor)
        {
            ruleVisitor.ExecuteRule(this);
        }
    }
}
