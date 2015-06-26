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
        public Action<RuleArguments> RulesSelectionChangingEventHandler;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRules();
            }
            else
            {
                if (ViewState["CurrentRuleArguments"] != null)
                {
                    var groupRepository = new GroupRepository();
                    var ruleRepository = new RuleRepository(groupRepository);
                    var ruleArguments = ViewState["CurrentRuleArguments"] as RuleArguments;
                    Rule editingRule;
                    var ruleId = ruleArguments.RuleId;
                    switch (ruleArguments.CurrentRuleKind)
                    {
                        //TODO AddWorkTime
                        case RuleKind.AddWorkTime:
                            editingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                            break;
                        case RuleKind.NotReportToMoscow:
                            editingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                            var ruleConfigurationControl = (NotReportToMoscowRuleConfigurationControl)
                                 LoadControl("~/Controls/RulesControls/NotReportToMoscowRuleConfigurationControl.ascx");
                            ruleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                            ruleConfigurationControl.RuleId = ruleId;
                            ruleConfigurationControl.SetDateTime(editingRule.TimeInformation.BeginTime, editingRule.TimeInformation.EndTime);
                            ruleConfigurationControl.RefreshRulesListAction += BindRules;
                            RuleEditingControlPlaceHolder.Controls.Add(ruleConfigurationControl);
                            break;
                        case RuleKind.NotifyByTime:
                            editingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                            break;
                        case RuleKind.NotifyLastUser:
                            editingRule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }

            RulesListGridView.RowDataBound += RulesListGridView_OnRowDataBound;
            RulesListGridView.SelectedIndexChanging += RulesListGridViewOnSelectedIndexChanging;
            RulesListGridView.RowDeleting += RulesListGridViewOnRowDeleting;
            //RulesListGridView.SelectedIndexChanged += RulesListGridViewOnSelectedIndexChanged;
        }

        private void RulesListGridViewOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            //if (SelectedRuleId == -1)
            //    throw new Exception("Selected rule id equals -1.");

            //if (RulesSelectionChangingEventHandler != null && SelectedRuleId != -1)
            //    RulesSelectionChangingEventHandler(new SelectedObjectEventArgs { ObjectID = SelectedRuleId });
        }

        private void RulesListGridViewOnSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var label = RulesListGridView.Rows[e.NewSelectedIndex].FindControl("RuleTypeLabel") as Label;
            var ruleKind = label.Text;
            RuleKind parsedRuleKind;
            Enum.TryParse(ruleKind, out parsedRuleKind);
            var ruleId = int.Parse(RulesListGridView.Rows[e.NewSelectedIndex].Cells[0].Text);

            if (RulesSelectionChangingEventHandler != null)
                RulesSelectionChangingEventHandler(new RuleArguments {RuleId = ruleId, CurrentRuleKind = parsedRuleKind});
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
                Rule deletingRule;
                switch (parsedRuleKind)
                {
                    //TODO AddWorkTime
                    case RuleKind.AddWorkTime:
                        deletingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                        break;
                    case RuleKind.NotReportToMoscow:
                        deletingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                        break;
                    case RuleKind.NotifyByTime:
                        deletingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                        break;
                    case RuleKind.NotifyLastUser:
                        deletingRule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                        break;
                    default:
                        throw new ArgumentException();
                }
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
                    imageButton.OnClientClick = string.Format("if (confirm('Are you sure?') == false) return false; ");
                }
            }
        }
    }
}