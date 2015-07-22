using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class NotReportToMoscowRuleControl : UserControl, IRuleInitializer, IRuleCreator
    {
        public Rule InitializeRule(Rule rule, string description, TimeEntity timeInformation)
        {
            var notReportToMoscowRule = (NotReportToMoscowRule)rule;
            notReportToMoscowRule.Description = description;
            notReportToMoscowRule.TimeInformation = timeInformation;
            return notReportToMoscowRule;
        }

        public Rule CreateRule(string description, TimeEntity timeInformation)
        {
            var notReportToMoscowRule = new NotReportToMoscowRule(description, timeInformation);
            return notReportToMoscowRule;
        }
    }
}