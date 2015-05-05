using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules
{
    public class TestActivityRuleChecking : IActivityRuleChecking
    {
        public bool IsActive(IRule rule)
        {
            return true;
        }
    }
}