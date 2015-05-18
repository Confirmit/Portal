using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace TestOfImplementersOfRules
{
    public class TestActivityRuleChecking : IActivityRuleChecking
    {
        public bool IsActive(Rule rule)
        {
            return true;
        }
    }
}