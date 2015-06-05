using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class RuleProcessor
    {
        public Visitor RuleController { get; set; }

        public RuleProcessor(Visitor ruleController)
        {
            RuleController = ruleController;
        }

        public void ExecuteRule(params Rule[] rules)
        {
            Array.ForEach(rules, rule => rule.Visit(RuleController));
        }
    }
}