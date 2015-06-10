using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters
{
    public interface IRuleFilter
    {
        bool IsNeccessaryToExecute(Rule rule);
    }
}