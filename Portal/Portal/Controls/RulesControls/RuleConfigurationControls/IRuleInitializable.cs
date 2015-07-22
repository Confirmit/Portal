using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public interface IRuleInitializable
    {
        void InitializeRuleControl(Rule rule);
    }
}
