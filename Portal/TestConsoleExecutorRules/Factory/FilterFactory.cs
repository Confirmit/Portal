using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;

namespace TestConsoleExecutorRules.Factory
{
    public class FilterFactory
    {
        public CompositeRuleFilter GetCompositeFilter()
        {
            var activeFilter = new ActiveTimeFilter();
            var dayOfWeekFilter = new DayOfWeekFilter();
            var experationTimeRuleFilter = new ExperationTimeFilter();
            var compositeFilter = new CompositeRuleFilter(dayOfWeekFilter, activeFilter, experationTimeRuleFilter);
            return compositeFilter;
        }
    }
}