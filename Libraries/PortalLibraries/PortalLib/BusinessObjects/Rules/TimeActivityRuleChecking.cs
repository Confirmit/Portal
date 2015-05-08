using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
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
