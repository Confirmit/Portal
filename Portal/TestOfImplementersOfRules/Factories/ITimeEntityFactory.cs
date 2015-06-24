using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestOfImplementersOfRules.Factories
{
    public interface ITimeEntityFactory
    {
        List<TimeEntity> GetTimeEntities(int number);
    }
}
