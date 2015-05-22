using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public interface IActivityRuleChecking
    {
        bool IsActive(Rule rule);
    }
}
