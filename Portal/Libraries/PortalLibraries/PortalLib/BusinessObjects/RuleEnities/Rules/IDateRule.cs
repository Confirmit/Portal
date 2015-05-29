using System;
using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    public interface IDateRule
    {
        DateTime Time { get; set; }
        HashSet<DayOfWeek> DaysOfWeek { get; set; }
    }
}