using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public interface IRuleInitializer
    {
        Rule InitializeRule(Rule rule, string description, TimeEntity timeInformation);
    }
}