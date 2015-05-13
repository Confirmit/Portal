using System.Collections.Generic;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [DBTable("Rules")]
    public class NotReportingRuleToMoscow : Rule
    {
        protected override string GetXmlRepresentation()
        {
            var helperSerialize = new SerializeHelper<NotReportingRuleToMoscow>();
            return helperSerialize.GetXml(this);
        }

        protected override void LoadFromXlm() { }

        public override RuleKind GetRuleType()
        {
            return RuleKind.NotReportingToMoscow;
        }

        public NotReportingRuleToMoscow()
        {
            GroupIdentifiers = new List<int>();
        }

        public NotReportingRuleToMoscow(List<int> groupsId)
        {
            GroupIdentifiers = new List<int>(groupsId);
        }

    }
}
