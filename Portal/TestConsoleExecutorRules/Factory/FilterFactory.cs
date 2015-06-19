﻿using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.Filters;

namespace TestConsoleExecutorRules.Factory
{
    public class FilterFactory
    {
        public CompositeRuleFilter GetCompositeFilter()
        {
            var activeFilter = new ActiveTimeFilter(DateTime.Now.AddHours(-1), DateTime.Now .AddHours(1));
            var dayOfWeekFilter = new DayOfWeekFilter();
            var compositeFilter = new CompositeRuleFilter(dayOfWeekFilter, activeFilter);
            return compositeFilter;
        }
    }
}