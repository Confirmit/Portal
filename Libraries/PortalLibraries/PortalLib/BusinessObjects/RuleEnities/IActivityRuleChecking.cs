using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IActivityRuleChecking
    {
        bool IsActive(Rule rule);
    }
}
