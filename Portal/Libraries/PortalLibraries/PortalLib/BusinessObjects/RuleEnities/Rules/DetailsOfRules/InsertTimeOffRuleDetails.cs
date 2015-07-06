using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class InsertTimeOffRuleDetails : RuleDetails
    {
        public TimeSpan Interval { get; set; }

        public InsertTimeOffRuleDetails() { }

        public InsertTimeOffRuleDetails(TimeSpan interval, TimeEntity timeInformation) : base(timeInformation)
        {
            Interval = interval;
        }
    }
}
