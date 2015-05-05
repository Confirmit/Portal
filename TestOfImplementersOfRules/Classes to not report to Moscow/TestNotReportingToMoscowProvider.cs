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
        public List<IRule> GetRules()
        {
            throw new NotImplementedException();
        }
    }
}
