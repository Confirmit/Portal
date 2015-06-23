using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public interface IRuleFilter
    {
        bool IsNeccessaryToExecute(Rule rule, DateTime currentDateTime);
    }
}