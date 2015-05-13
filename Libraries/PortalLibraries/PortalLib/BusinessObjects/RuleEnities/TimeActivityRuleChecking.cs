using System;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
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
