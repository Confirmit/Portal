using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotReportedToMoscowImplementer
    {
        private List<IRule> _rules;
        public NotReportedToMoscowImplementer(INotReportingToMoscowProvider providerRules)
        {
            _rules = providerRules.GetRules();
        }
    }
}
