using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class InsertTimeOffRule : Rule
    {
        public TimeSpan Interval { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }
        
        public InsertTimeOffRule() { }

        public InsertTimeOffRule(TimeSpan interval, TimeEntity timeInformation)
        {
            Interval = interval;
            RuleDetails = new InsertTimeOffRuleDetails(interval, timeInformation);
        }

        public override void DeserializeInstance()
        {
            var ruleDetails = new SerializeHelper<InsertTimeOffRuleDetails>().GetInstance(XmlInformation);
            Interval = ruleDetails.Interval;
            RuleDetails = ruleDetails;
        }

        public override void Visit(RuleVisitor ruleVisitor, RuleInstance ruleInstance)
        {
            ruleVisitor.ExecuteRule(this, ruleInstance);
        }
    }
}
