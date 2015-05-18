using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotReportToMoscowRule : Rule
    {
        public override void BuildInstance(RuleInfo ruleInfo) { }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotReportToMoscow; }
        }
    }
}
