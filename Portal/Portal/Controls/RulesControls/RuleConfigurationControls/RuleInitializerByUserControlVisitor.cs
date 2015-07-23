using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class RuleInitializerByUserControlVisitor : RuleControlVisitor
    {
        public Rule InitializableRule { get; set; }
        public string Description { get; set; }
        public TimeEntity TimeInformation { get; set; }

        public RuleInitializerByUserControlVisitor(Rule initializableRule, string description, TimeEntity timeInformation)
        {
            InitializableRule = initializableRule;
            Description = description;
            TimeInformation = timeInformation;
        }

        public override void Visit(InsertTimeOffRuleControl insertTimeOffRuleControl)
        {
            var insertTimeOffRule = (InsertTimeOffRule) InitializableRule;
            insertTimeOffRule.Interval = insertTimeOffRule.Interval;
            insertTimeOffRule.Description = Description;
            insertTimeOffRule.TimeInformation = TimeInformation;
            InitializableRule = insertTimeOffRule;
        }

        public override void Visit(NotifyByTimeRuleControl notifyByTimeRuleControl)
        {
            var notifyByTimeRule = (NotifyByTimeRule)InitializableRule;
            notifyByTimeRule.Description = Description;
            notifyByTimeRule.TimeInformation = TimeInformation;
            notifyByTimeRule.Subject = notifyByTimeRule.Subject;
            notifyByTimeRule.Information = notifyByTimeRule.Information;
            InitializableRule = notifyByTimeRule;
        }

        public override void Visit(NotifyLastUserRuleControl notifyLastUserRuleControl)
        {
            var notifyLastUserRule = (NotifyLastUserRule)InitializableRule;
            notifyLastUserRule.Description = Description;
            notifyLastUserRule.TimeInformation = TimeInformation;
            notifyLastUserRule.Subject = notifyLastUserRule.Subject;
            InitializableRule = notifyLastUserRule;
        }

        public override void Visit(NotReportToMoscowRuleControl notReportToMoscowRuleControl)
        {
            var notReportToMoscowRule = (NotReportToMoscowRule)InitializableRule;
            notReportToMoscowRule.Description = Description;
            notReportToMoscowRule.TimeInformation = TimeInformation;
            InitializableRule = notReportToMoscowRule;
        }
    }
}