namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class NotifyLastUserRuleDetails : RuleDetails
    {
        public string Subject { get; set; }

        public NotifyLastUserRuleDetails(string subject)
        {
            Subject = subject;
        }
    }
}