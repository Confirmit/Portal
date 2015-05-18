namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule
{
    public class NotifyLastUserRuleInfo : RuleInfo
    {
        public string Subject { get; set; }

        public NotifyLastUserRuleInfo(string subject)
        {
            Subject = subject;
        }
    }
}