using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;
using Core;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public interface IRuleProvider<T> where T : Rule
    {
        IList<T> GetAllRules();
        void SaveRule(T rule);
        void DeleteRule(int id);
        T GetRuleById(int id);
    }
}
