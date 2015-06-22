namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    public class RuleEntity
    {
        public RuleEntity(Rule rule, RuleInstance ruleInstance)
        {
            Rule = rule;
            RuleInstance = ruleInstance;
        }

        public Rule Rule { get; set; }
        public RuleInstance RuleInstance { get; set; }
    }
}