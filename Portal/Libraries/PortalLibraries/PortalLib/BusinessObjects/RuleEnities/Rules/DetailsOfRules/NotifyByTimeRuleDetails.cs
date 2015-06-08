using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class NotifyByTimeRuleDetails : RuleDetails
    {
        public string Information { get; set; }
        public string Subject { get; set; }

        public NotifyByTimeRuleDetails()
        {
        }

        public NotifyByTimeRuleDetails(string subject, string information, TimeEntity timeInformation)
            : base(timeInformation)
        {
            Subject = subject;
            Information = information;
        }
    }
}