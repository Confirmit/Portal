using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class RuleUserControlsInitalizerByRuleVisitor : RuleControlVisitor
    {
        public Rule RuleForInitalizing { get; set; }
        public UserControl RuleUserControl { get; set; }

        public RuleUserControlsInitalizerByRuleVisitor(Rule ruleForInitalizing)
        {
            RuleForInitalizing = ruleForInitalizing;
        }

        public override void Visit(InsertTimeOffRuleControl insertTimeOffRuleControl)
        {
            var insertTimeOffRuleConfigurationControl = new InsertTimeOffRuleControl();
            var insertTimeOffRule = (InsertTimeOffRule)RuleForInitalizing;
            insertTimeOffRuleConfigurationControl.TimeIntervalSelector.InitializeAllTimeListBoxes();
            insertTimeOffRuleConfigurationControl.TimeInterval = insertTimeOffRule.Interval;
            RuleUserControl = insertTimeOffRuleControl;
        }

        public override void Visit(NotifyByTimeRuleControl notifyByTimeRuleControl)
        {
            var notifyByTimeRuleConfigurationControl = new NotifyByTimeRuleControl();
            var notifyByTimeRule = (NotifyByTimeRule)RuleForInitalizing;
            notifyByTimeRuleConfigurationControl.Subject = notifyByTimeRule.Subject;
            notifyByTimeRuleConfigurationControl.Information = notifyByTimeRule.Information;
            RuleUserControl = notifyByTimeRuleControl;
        }

        public override void Visit(NotifyLastUserRuleControl notifyLastUserRuleControl)
        {
            var notifyLastUserRuleConfigurationControl = new NotifyLastUserRuleControl();
            var notifyLastUserRule = (NotifyLastUserRule)RuleForInitalizing;
            notifyLastUserRuleConfigurationControl.Subject = notifyLastUserRule.Subject;
            RuleUserControl = notifyLastUserRuleControl;
        }

        public override void Visit(NotReportToMoscowRuleControl notReportToMoscowRuleControl)
        {
            RuleUserControl = new NotReportToMoscowRuleControl();
        }
    }
}