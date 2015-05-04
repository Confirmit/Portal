using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class TimeActivityRuleChecking : IActivityRuleChecking
    {
        public bool IsActive(IRule rule)
        {
            if (DateTime.Now > rule.BeginTime && DateTime.Now < rule.EndTime)
                return true;

            return false;
        }
    }
}
