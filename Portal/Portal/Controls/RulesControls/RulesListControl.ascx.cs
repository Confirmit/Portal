using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class RulesListControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRules();
            }
            RulesListGridView.RowDeleting += RulesListGridViewOnRowDeleting;
        }

        private void BindRules()
        {
            var groupRepository = new GroupRepository();
            var allRules = new RuleRepository(groupRepository).GetAllRules();
            RulesListGridView.DataSource = allRules;
            RulesListGridView.DataBind();
        }

        private void RulesListGridViewOnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var ruleId = int.Parse(RulesListGridView.Rows[e.RowIndex].Cells[0].Text);
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);

            var label = RulesListGridView.Rows[e.RowIndex].FindControl("RuleTypeLabel") as Label;
            var ruleKind = label.Text;
            RuleKind parsedRuleKind;
            Enum.TryParse(ruleKind, out parsedRuleKind);
            Rule deletingRule;
            switch (parsedRuleKind)
            {
                //TODO AddWorkTime
                case RuleKind.AddWorkTime:
                    deletingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                    ruleRepository.DeleteRule(deletingRule);
                    break;
                case RuleKind.NotReportToMoscow:
                    deletingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                    ruleRepository.DeleteRule(deletingRule);
                    break;
                case RuleKind.NotifyByTime:
                    deletingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                    ruleRepository.DeleteRule(deletingRule);
                    break;
                case RuleKind.NotifyLastUser:
                    deletingRule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                    ruleRepository.DeleteRule(deletingRule);
                    break;
            }
            BindRules();
        }

        protected void RulesListGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imageButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
                if (imageButton != null)
                {
                    imageButton.Visible = ((BaseWebPage)Page).CurrentUser.IsInRole(RolesEnum.Administrator);
                    imageButton.OnClientClick = string.Format("if (confirm('Are you sure?') == false) return false; ");
                }
            }
        }
    }
}