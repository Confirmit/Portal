using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
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

        public NotifyByTimeRule() : base("") { }

        public NotifyByTimeRule(string description,  string subject, string information, TimeEntity timeInformation) : base(description)
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

        public override void Visit(RuleVisitor ruleVisitor, RuleInstance ruleInstance)
        {
            ruleVisitor.ExecuteRule(this, ruleInstance);
        }
    }
}
