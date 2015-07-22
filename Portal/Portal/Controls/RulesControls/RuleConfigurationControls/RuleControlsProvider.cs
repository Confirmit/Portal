using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class RuleControlsProvider
    {
        public UserControl GetRuleControl(RuleKind ruleKind)
        {
            UserControl ruleControl;
            switch (ruleKind)
            {
                case RuleKind.NotifyByTime:
                    ruleControl = new NotifyByTimeRuleControl();
                    break;
                case RuleKind.NotifyLastUser:
                    ruleControl = new NotifyLastUserRuleControl();
                    break;
                case RuleKind.AddWorkTime:
                    ruleControl = new InsertTimeOffRuleControl();
                    break;
                case RuleKind.NotReportToMoscow:
                    ruleControl = new NotReportToMoscowRuleControl();
                    break;
                default:
                    throw new ArgumentException();
            }
            return ruleControl;
        }
    }
}