using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor
{
    public class RuleVisitor
    {
        public InsertTimeOffRuleExecutor InsertTimeOffExecutor { get; set; }
        public NotifyByTimeRuleExecutor NotifyByTimeExecutor { get; set; }
        public NotifyLastUserExecutor NotifyLastUserExecutor { get; set; }
        public ReportComposerToMoscowExecutor ReportComposerToMoscow { get; set; }

       public RuleVisitor(InsertTimeOffRuleExecutor insertTimeOffExecutor, NotifyByTimeRuleExecutor notifyByTimeExecutor,
            NotifyLastUserExecutor notifyLastUserExecutor, ReportComposerToMoscowExecutor reportComposerToMoscow)
        {
            InsertTimeOffExecutor = insertTimeOffExecutor;
            NotifyByTimeExecutor = notifyByTimeExecutor;
            NotifyLastUserExecutor = notifyLastUserExecutor;
            ReportComposerToMoscow = reportComposerToMoscow;
        }

        public void ExecuteRule(InsertTimeOffRule rule, RuleInstance ruleInstance)
        {
            InsertTimeOffExecutor.ExecuteRule(rule, ruleInstance);
        }

        public void ExecuteRule(NotifyByTimeRule rule, RuleInstance ruleInstance)
        {
            NotifyByTimeExecutor.ExecuteRule(rule, ruleInstance);
        }

        public void ExecuteRule(NotifyLastUserRule rule, RuleInstance ruleInstance)
        {
            NotifyLastUserExecutor.ExecuteRule(rule, ruleInstance);
        }

        public void ExecuteRule(NotReportToMoscowRule rule, RuleInstance ruleInstance)
        {
            ReportComposerToMoscow.ExecuteRule(rule, ruleInstance);
        }
    }
}
