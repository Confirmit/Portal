using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotReportToMoscowRule : Rule
    {
        public override void DeserializeInstance() { }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotReportToMoscow; }
        }
    }
}
