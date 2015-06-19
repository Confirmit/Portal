using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace TestConsoleExecutorRules.Factory
{
    public class FilterFactory
    {
        public CompositeRuleFilter GetCompositeFilter()
        {
            var activeFilter = new ActiveTimeFilter(DateTime.Now.AddHours(-1), DateTime.Now .AddHours(1));
            var dayOfWeekFilter = new DayOfWeekFilter();
            var experationTimeRuleFilter = new ExperationTimeFilter(new InstanceRuleRepository());
            var compositeFilter = new CompositeRuleFilter(dayOfWeekFilter, activeFilter, experationTimeRuleFilter);
            return compositeFilter;
        }
    }
}