using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestOfImplementersOfRules.Factories.TimeEntityFactories
{
    public interface ITimeEntityFactory
    {
        List<TimeEntity> GetTimeEntities(int number);
    }
}
