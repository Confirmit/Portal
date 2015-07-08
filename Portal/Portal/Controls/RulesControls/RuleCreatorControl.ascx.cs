using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Portal.Controls.RulesControls.RuleConfigurationControls;

namespace Portal.Controls.RulesControls
{
    public partial class RuleCreatorControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var ruleKinds = Enum.GetNames(typeof(RuleKind));
                RuleTypesDropDownList.DataSource = ruleKinds;
                RuleTypesDropDownList.DataBind();

                CommonRuleSettingsControl.DaysOfWeekCheckBoxes.DataSource = Enum.GetNames(typeof(DayOfWeek));
                CommonRuleSettingsControl.DaysOfWeekCheckBoxes.DataBind();
                AddSelectedRuleConfigurationPanel();
            }
            else
            {
                if (ViewState["CurrentRuleArguments"] != null)
                {
                    var currentRuleKind = (ViewState["CurrentRuleArguments"] as RuleArguments).CurrentRuleKind;
                    AddRuleConfigurationControl(currentRuleKind);
                }
            }
            CreateRuleButton.Click += CreateRuleButtonOnClick;
            RuleTypesDropDownList.SelectedIndexChanged += RuleTypesDropDownListOnSelectedIndexChanged;
        }

        private void AddRuleConfigurationControl(RuleKind currentRuleKind)
        {
            switch (currentRuleKind)
            {
                case RuleKind.AddWorkTime:
                    var insertTimeOffRuleConfigurationControl = (InsertTimeOffRuleConfigurationControl)
                        LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/InsertTimeOffRuleConfigurationControl.ascx");
                    insertTimeOffRuleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                    insertTimeOffRuleConfigurationControl.TimeIntervalSelector.InitializeAllTimeListBoxes();
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(insertTimeOffRuleConfigurationControl);
                    break;
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl = (NotifyByTimeRuleConfigurationControl)
                        LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyByTimeRuleConfigurationControl.ascx");
                    notifyByTimeRuleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(notifyByTimeRuleConfigurationControl);
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl = (NotifyLastUserRuleConfigurationControl)
                        LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyLastUserRuleConfigurationControl.ascx");
                    notifyLastUserRuleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(notifyLastUserRuleConfigurationControl);
                    break;
            }
            ViewState["CurrentRuleArguments"] = new RuleArguments
            {
                CurrentRuleKind = currentRuleKind
            };
        }

        private void RuleTypesDropDownListOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            AddSelectedRuleConfigurationPanel();
        }

        private void AddSelectedRuleConfigurationPanel()
        {
            RuleKind parsedRuleKind;
            Enum.TryParse(RuleTypesDropDownList.SelectedValue, out parsedRuleKind);
            CommonRuleSettingsControl.RuleConfiguration.Controls.Clear();
            AddRuleConfigurationControl(parsedRuleKind);
        }

        private void CreateRuleButtonOnClick(object sender, EventArgs eventArgs)
        {
            var selectedCheckboxItems = CommonRuleSettingsControl.DaysOfWeekCheckBoxes.Items.Cast<ListItem>().Where(x => x.Selected).Select(item => item.Value).ToArray();
            var selectedDaysOfWeek = new HashSet<DayOfWeek>(selectedCheckboxItems.Select(selectedItem => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), selectedItem)));
            var expirationTime = CommonRuleSettingsControl.ExpirationTime;
            var launchTime = CommonRuleSettingsControl.LaunchTime;
            
            RuleKind ruleKind;
            Enum.TryParse(RuleTypesDropDownList.SelectedValue, out ruleKind);
            DateTime beginDateTime;
            if (!DateTime.TryParse(CommonRuleSettingsControl.BeginTime.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out beginDateTime))
                return;
            DateTime endDateTime;
            if (!DateTime.TryParse(CommonRuleSettingsControl.EndTime.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out endDateTime))
                return;
            var timeInformation = new TimeEntity(expirationTime, launchTime, selectedDaysOfWeek, beginDateTime, endDateTime);
            Rule rule;
            switch (ruleKind)
            {
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as NotifyByTimeRuleConfigurationControl;
                    var notifyByTimeSubject = notifyByTimeRuleConfigurationControl.Subject;
                    var notifyByTimeInformation = notifyByTimeRuleConfigurationControl.Information;
                    rule = new NotifyByTimeRule(CommonRuleSettingsControl.RuleDiscription.Text, notifyByTimeSubject, notifyByTimeInformation, timeInformation);
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as NotifyLastUserRuleConfigurationControl;
                    var notifyLastUserRuleSubject = notifyLastUserRuleConfigurationControl.Subject;
                    rule = new NotifyLastUserRule(CommonRuleSettingsControl.RuleDiscription.Text, notifyLastUserRuleSubject, timeInformation);
                    break;
                case RuleKind.AddWorkTime:
                    var insertTimeOffRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as InsertTimeOffRuleConfigurationControl;
                    var timeInterval = insertTimeOffRuleConfigurationControl.TimeInterval;
                    rule = new InsertTimeOffRule(CommonRuleSettingsControl.RuleDiscription.Text, timeInterval, timeInformation);
                    break;
                case RuleKind.NotReportToMoscow:
                    rule = new NotReportToMoscowRule(CommonRuleSettingsControl.RuleDiscription.Text, timeInformation);
                    break;
                default:
                    throw new ArgumentException();
            }

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.SaveRule(rule);

            var urlForRedirection = string.Format("{0}?RuleID={1}", Request.Url, rule.ID);
            Response.Redirect(urlForRedirection, false);
        }
    }
}