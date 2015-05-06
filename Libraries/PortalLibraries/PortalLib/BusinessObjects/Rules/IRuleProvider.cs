using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using Core;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public interface IRuleProvider<T> where T : IRule
    {
        List<T> GetRules();
    }
}
