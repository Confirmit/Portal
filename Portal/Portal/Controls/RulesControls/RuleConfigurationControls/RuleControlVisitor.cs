using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public abstract class RuleControlVisitor
    {
        public abstract void Visit(InsertTimeOffRuleControl insertTimeOffRuleControl);

        public abstract void Visit(NotifyByTimeRuleControl notifyByTimeRuleControl);

        public abstract void Visit(NotifyLastUserRuleControl notifyLastUserRuleControl);

        public abstract void Visit(NotReportToMoscowRuleControl notReportToMoscowRuleControl);
    }
}