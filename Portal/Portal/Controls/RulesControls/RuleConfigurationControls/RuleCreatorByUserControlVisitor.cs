using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class RuleCreatorByUserControlVisitor : RuleControlVisitor
    {
        public Rule GeneratingRule { get; set; }
        public string Description { get; set; }
        public TimeEntity TimeInformation { get; set; }

        public RuleCreatorByUserControlVisitor(string description, TimeEntity timeInformation)
        {
            Description = description;
            TimeInformation = timeInformation;
        }

        public override void Visit(InsertTimeOffRuleControl insertTimeOffRuleControl)
        {
            var initializableRule = new InsertTimeOffRule
            {
                Interval = insertTimeOffRuleControl.TimeInterval,
                Description = Description,
                TimeInformation = TimeInformation
            };
            GeneratingRule = initializableRule;
        }

        public override void Visit(NotifyByTimeRuleControl notifyByTimeRuleControl)
        {
            var notifyByTimeRule = new NotifyByTimeRule
            {
                Description = Description,
                TimeInformation = TimeInformation,
                Subject = notifyByTimeRuleControl.Subject,
                Information = notifyByTimeRuleControl.Information
            };
            GeneratingRule = notifyByTimeRule;
        }

        public override void Visit(NotifyLastUserRuleControl notifyLastUserRuleControl)
        {
            var notifyLastUserRule = new NotifyByTimeRule
            {
                Description = Description,
                TimeInformation = TimeInformation,
                Subject = notifyLastUserRuleControl.Subject
            };
            GeneratingRule = notifyLastUserRule;
        }

        public override void Visit(NotReportToMoscowRuleControl notReportToMoscowRuleControl)
        {
            var notReportToMoscowRule = new NotReportToMoscowRule
            {
                Description = Description,
                TimeInformation = TimeInformation
            };
            GeneratingRule = notReportToMoscowRule;
        }
    }
}