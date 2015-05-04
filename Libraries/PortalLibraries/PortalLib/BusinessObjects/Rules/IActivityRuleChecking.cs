using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IActivityRuleChecking
    {
        bool IsActive(IRule rule);
    }
}
