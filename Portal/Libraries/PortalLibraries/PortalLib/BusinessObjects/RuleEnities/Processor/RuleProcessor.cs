using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor
{
    public class RuleProcessor
    {
        public RuleVisitor RuleController { get; set; }
        
        public RuleProcessor(RuleVisitor ruleController)
        {
            RuleController = ruleController;
        }

        public void ExecuteRule(params RuleEntity[] ruleEntities)
        {
            Array.ForEach(ruleEntities, ruleEntity => ruleEntity.Rule.Visit(RuleController, ruleEntity.RuleInstance));
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