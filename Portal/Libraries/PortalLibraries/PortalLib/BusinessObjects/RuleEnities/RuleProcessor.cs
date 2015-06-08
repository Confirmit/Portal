using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
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

        public InsertTimeOffRuleExecutor InsertTimeOffExecutor
        {
            get { return RuleController.InsertTimeOffExecutor; }
            set { RuleController.InsertTimeOffExecutor = value; }
        }

        public NotifyByTimeRuleExecutor NotifyByTimeExecutor
        {
            get { return RuleController.NotifyByTimeExecutor; }
            set { RuleController.NotifyByTimeExecutor = value; }
        }

        public NotifyLastUserExecutor NotifyLastUserExecutor
        {
            get { return RuleController.NotifyLastUserExecutor; }
            set { RuleController.NotifyLastUserExecutor = value; }
        }

        public ReportComposerToMoscowExecutor ReportComposerToMoscow
        {
            get { return RuleController.ReportComposerToMoscow; }
            set { RuleController.ReportComposerToMoscow = value; }
        }
    }
}