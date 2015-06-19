using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotReportToMoscowRule : Rule
    {
        public NotReportToMoscowRule()
        {
            
        }

        public NotReportToMoscowRule(TimeEntity timeInformation)
        {
            RuleDetails = new RuleDetails(timeInformation);
        }

        public override void DeserializeInstance()
        {
            RuleDetails = new SerializeHelper<RuleDetails>().GetInstance(XmlInformation);
        }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotReportToMoscow; }
        }

        public override void Visit(RuleVisitor ruleVisitor)
        {
            ruleVisitor.ExecuteRule(this);
        }
    }
}
