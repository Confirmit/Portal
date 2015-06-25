using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Portal.Controls.RulesControls;

namespace Portal.Admin
{
    public partial class AdminRulesEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isShowRuleCreatorControl = string.IsNullOrEmpty(Request.QueryString["RuleID"]);
            if (isShowRuleCreatorControl)
            {
                RuleCreatorControl.Visible = RuleEditingControlPlaceHolder.Visible = true; 
            }
            else
            {
                var ruleId = int.Parse(Request.QueryString["RuleID"]);
                RuleCreatorControl.Visible = false;
                GroupsMinipulationControl.CurrentRuleId = ruleId;

                var groupRepository = new GroupRepository();
                var ruleRepository = new RuleRepository(groupRepository);

                var ruleKind = Request.QueryString["RuleKind"];
                RuleKind parsedRuleKind;
                Enum.TryParse(ruleKind, out parsedRuleKind);

                //RuleEditingControlPlaceHolder.Controls.Clear();
                Rule editingRule;
                switch (parsedRuleKind)
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
                        ruleConfigurationControl.SetDateTime(editingRule.BeginTime, editingRule.EndTime);
                        ViewState["CurrentRuleArguments"] = new RuleArguments
                        {
                            RuleId = ruleId,
                            CurrentRuleKind = RuleKind.NotReportToMoscow
                        };
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
    }
}