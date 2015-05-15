using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotReportToMoscowRule : Rule
    {
        protected override string GetXmlRepresentation()
        {
            var helperSerialize = new SerializeHelper<NotReportToMoscowRule>();
            return helperSerialize.GetXml(this);
        }

        protected override void LoadFromXlm() { }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotReportToMoscow; }
        }
    }
}
