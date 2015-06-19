using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

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

        public void ExecuteRule(InsertTimeOffRule rule)
        {
            InsertTimeOffExecutor.ExecuteRule(rule);
        }

        public void ExecuteRule(NotifyByTimeRule rule)
        {
            NotifyByTimeExecutor.ExecuteRule(rule);
        }

        public void ExecuteRule(NotifyLastUserRule rule)
        {
            NotifyLastUserExecutor.ExecuteRule(rule);
        }

        public void ExecuteRule(NotReportToMoscowRule rule)
        {
            ReportComposerToMoscow.ExecuteRule(rule);
        }
    }
}
