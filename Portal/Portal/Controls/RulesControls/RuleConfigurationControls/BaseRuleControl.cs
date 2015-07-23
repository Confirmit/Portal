using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public abstract class BaseRuleControl: UserControl
    {
        public abstract void Accept(RuleControlVisitor ruleControlVisitor);
        public abstract void DataBind(Rule rule);
    }
}