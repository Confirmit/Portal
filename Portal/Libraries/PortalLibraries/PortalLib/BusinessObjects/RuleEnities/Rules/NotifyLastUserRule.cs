using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
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

        public NotifyLastUserRule() { }

        public NotifyLastUserRule(string subject)
        {
            Subject = subject;
            RuleDetails = new NotifyLastUserRuleDetails(subject);
        }

        public override void BuildInstance()
        {
            var ruleDetails = new SerializeHelper<NotifyLastUserRuleDetails>().GetInstance(XmlInformation);
            Subject = ruleDetails.Subject;
        }
    }
}
