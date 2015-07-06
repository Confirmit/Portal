using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Admin
{
    public partial class AdminRulesListPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlForEditingRules.RulesSelectionChangingEventHandler += RulesSelectionChangingEventHandler;
            AddNewRuleButton.Click += AddNewRuleButtonOnClick;
        }

        private void RulesSelectionChangingEventHandler(RuleArguments e)
        {
            Response.Redirect(string.Format("~/Admin/AdminRulesEditingPage.aspx?RuleID={0}", e.RuleId), false);
        }

        private void AddNewRuleButtonOnClick(object sender, EventArgs eventArgs)
        {
            Response.Redirect("~/Admin/AdminRulesEditingPage.aspx", false);
        }
    }
}