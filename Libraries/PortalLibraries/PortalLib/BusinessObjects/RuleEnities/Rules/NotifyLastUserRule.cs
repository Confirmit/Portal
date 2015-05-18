using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyLastUserRule : Rule
    {
        public string Subject { get; set; }

        public override void BuildInstance(RuleInfo ruleInfo)
        {
            var info = RuleInfo as NotifyLastUserRuleInfo;
            Subject = info.Subject;
        }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyLastUser; }
        }

        public NotifyLastUserRule() { }

        public NotifyLastUserRule(string subject)
        {
            Subject = subject;
            RuleInfo = new NotifyLastUserRuleInfo(subject);
        }
    }
}
