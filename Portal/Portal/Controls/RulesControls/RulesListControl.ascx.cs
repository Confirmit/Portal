using System;
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
        public PlaceHolder RuleEditingControlPlaceHolder { get; set; }
        public event EventHandler<RuleArguments> RulesSelectionChangingEventHandler;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RulesListGridView.RowDataBound += RulesListGridView_OnRowDataBound;
            RulesListGridView.SelectedIndexChanging += RulesListGridViewOnSelectedIndexChanging;
            RulesListGridView.RowDeleting += RulesListGridViewOnRowDeleting;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                BindRules();
            }
        }

        private void RulesListGridViewOnSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var label = RulesListGridView.Rows[e.NewSelectedIndex].FindControl("RuleTypeLabel") as Label;
            var ruleKind = label.Text;
            RuleKind parsedRuleKind;
            Enum.TryParse(ruleKind, out parsedRuleKind);
            var ruleId = int.Parse(RulesListGridView.Rows[e.NewSelectedIndex].Cells[0].Text);

            if (RulesSelectionChangingEventHandler != null)
                RulesSelectionChangingEventHandler(this, new RuleArguments {RuleId = ruleId, CurrentRuleKind = parsedRuleKind});
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
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);

            var label = RulesListGridView.Rows[e.RowIndex].FindControl("RuleTypeLabel") as Label;
            if (label != null)
            {
                var ruleKind = label.Text;
                RuleKind parsedRuleKind;
                Enum.TryParse(ruleKind, out parsedRuleKind);
                var ruleId = int.Parse(RulesListGridView.Rows[e.RowIndex].Cells[0].Text);
                var deletingRule = new RuleProvider().GetRuleByIdAndRuleKind(ruleId, parsedRuleKind);
                ruleRepository.DeleteRule(deletingRule);
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
                    imageButton.OnClientClick = "if (confirm(\'Are you sure?\') == false) return false; ";
                }
            }
        }
    }
}