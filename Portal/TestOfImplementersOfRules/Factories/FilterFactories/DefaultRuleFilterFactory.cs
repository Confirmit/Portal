using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;

namespace TestOfImplementersOfRules.Factories.FilterFactories
{
    public class DefaultRuleFilterFactory : IRuleFilterFactory
    {
        public IRuleFilter GetFilter()
        {
            var activeFilter = new ActiveTimeFilter();
            var dayOfWeekFilter = new DayOfWeekFilter();
            var experationTimeRuleFilter = new ExperationTimeFilter();
            var compositeFilter = new CompositeRuleFilter(dayOfWeekFilter, activeFilter, experationTimeRuleFilter);
            return compositeFilter; 
        }
    }
}