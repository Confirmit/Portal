using System;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [Serializable]
    public class RuleArguments
    {
        public int RuleId { get; set; }
        public RuleKind CurrentRuleKind { get; set; }
    }
}
