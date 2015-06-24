using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters
{
    public interface IRuleFilter
    {
        bool IsNeccessaryToExecute(ITimeInformation timeInfo, DateTime currentDateTime);
    }
}