using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class TimeActivityRuleChecking : IActivityRuleChecking
    {
        public bool IsActive(Rule rule)
        {
            if (DateTime.Now > rule.BeginTime && DateTime.Now < rule.EndTime)
                return true;

            return false;
        }
    }
}
