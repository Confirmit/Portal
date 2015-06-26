using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyLastUserRule : Rule
    {
        public string Subject { get; set; }
        
        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyLastUser; }
        }

        public NotifyLastUserRule() : base("") { }

        public NotifyLastUserRule(string description, string subject, TimeEntity timeInformation) : base(description)
        {
            Subject = subject;
            RuleDetails = new NotifyLastUserRuleDetails(subject, timeInformation);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<NotifyLastUserRuleDetails>().GetInstance(XmlInformation);
            Subject = ruleDetails.Subject;
            RuleDetails = ruleDetails;
        }

        public override void Visit(RuleVisitor ruleVisitor, RuleInstance ruleInstance)
        {
            ruleVisitor.ExecuteRule(this, ruleInstance);
        }
    }
}
