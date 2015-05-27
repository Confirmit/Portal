using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestOfImplementersOfRules.CommonTestClasses
{
    public class TestActivityRuleChecking : IActivityRuleChecking
    {
        public bool Result { get; set; }

        public TestActivityRuleChecking(bool result)
        {
            Result = result;
        }

        public bool IsActive(Rule rule)
        {
            return Result;
        }
    }
}