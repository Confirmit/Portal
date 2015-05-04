using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotReportingRuleToMoscow : Rule, IRule
    {
        protected override void LoadToXml()
        {
            _xmlInformation = string.Empty;
        }

        protected override void LoadFromXlm()
        {
            _xmlInformation = string.Empty;
        }

        public override RuleKind GetRuleType()
        {
            return RuleKind.NotReportingToMoscow;
        }

        public NotReportingRuleToMoscow()
        {
            GroupsId = new List<int>();
        }

        public NotReportingRuleToMoscow(List<int> groupsId)
        {
            GroupsId = new List<int>(groupsId);
        }

    }
}
