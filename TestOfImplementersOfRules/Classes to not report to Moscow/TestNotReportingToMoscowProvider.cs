using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace TestOfImplementersOfRules.NotReportingToMoscow
{
    public class TestNotReportingToMoscowProvider : INotReportingToMoscowProvider
    {
        private IRule _rule;
        private List<IRule> _rules;

        public TestNotReportingToMoscowProvider(List<IRule> rules)
        {
            _rules = rules;
        }
        public List<IRule> GetRules()
        {
            return _rules;
        }
    }
}
