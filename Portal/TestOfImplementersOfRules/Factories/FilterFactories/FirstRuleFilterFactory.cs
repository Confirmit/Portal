using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;

namespace TestOfImplementersOfRules.Factories.FilterFactories
{
    public class FirstRuleFilterFactory : IRuleFilterFactory
    {
        public IRuleFilter GetFilter()
        {
            var activeFilter = new ActiveTimeFilter(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1));
            var dayOfWeekFilter = new DayOfWeekFilter();
            var experationTimeRuleFilter = new ExperationTimeFilter();
            var compositeFilter = new CompositeRuleFilter(dayOfWeekFilter, activeFilter, experationTimeRuleFilter);
            return compositeFilter; 
        }
    }
}