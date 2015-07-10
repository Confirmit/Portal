namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class NotifyLastUserRuleDetails : RuleDetails
    {
        public string Subject { get; set; }

        public NotifyLastUserRuleDetails()
        {
            Subject = "";
        }

        public NotifyLastUserRuleDetails(string subject, TimeEntity timeInformation) : base(timeInformation)
        {
            Subject = subject;
        }
    }
}