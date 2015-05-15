using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.RuleInfo
{
    public class AddWorkTimeRuleInfo : IRuleInfo
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
