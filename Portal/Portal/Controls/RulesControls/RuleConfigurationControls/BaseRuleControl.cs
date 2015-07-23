using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public abstract class BaseRuleControl: UserControl
    {
        public abstract void Accept(RuleControlVisitor ruleControlVisitor);
    }
}