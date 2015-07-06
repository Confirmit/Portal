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
                var jsCode = string.Format("javascript: CheckBoxListSelect ('{0}', '{1}');",
                    DaysOfWeekCheckBoxList.ClientID, SelectAllDayCheckBox.ClientID);
                SelectAllDayCheckBox.Attributes.Add("onclick", jsCode);
                
                var ruleKinds = Enum.GetNames(typeof(RuleKind));
                RuleTypesDropDownList.DataSource = ruleKinds;
                RuleTypesDropDownList.DataBind();

                DaysOfWeekCheckBoxList.DataSource = Enum.GetNames(typeof(DayOfWeek));
                DaysOfWeekCheckBoxList.DataBind();
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
            CreateRuleButton.Click += CreateGroupButtonOnClick;
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
                    RuleConfigurationControlPlaceHolder.Controls.Add(insertTimeOffRuleConfigurationControl);
                    break;
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl = (NotifyByTimeRuleConfigurationControl)
                        LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyByTimeRuleConfigurationControl.ascx");
                    notifyByTimeRuleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                    RuleConfigurationControlPlaceHolder.Controls.Add(notifyByTimeRuleConfigurationControl);
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl = (NotifyLastUserRuleConfigurationControl)
                        LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyLastUserRuleConfigurationControl.ascx");
                    notifyLastUserRuleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                    RuleConfigurationControlPlaceHolder.Controls.Add(notifyLastUserRuleConfigurationControl);
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
            RuleConfigurationControlPlaceHolder.Controls.Clear();
            AddRuleConfigurationControl(parsedRuleKind);
        }

        private void CreateGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var selectedCheckboxItems = DaysOfWeekCheckBoxList.Items.Cast<ListItem>().Where(x => x.Selected).Select(item => item.Value).ToArray();
            var selectedDaysOfWeek = new HashSet<DayOfWeek>(selectedCheckboxItems.Select(selectedItem => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), selectedItem)));
            var expirationHoursTime = int.Parse(ExpirationTimeTextBox.Text);
            var launchTimeText = LaunchTimeTextBox.Text + ":00";
            var timeSpan = TimeSpan.Parse(launchTimeText);
            var currentDateTime = DateTime.Now;
            var launchTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            RuleKind ruleKind;
            Enum.TryParse(RuleTypesDropDownList.SelectedValue, out ruleKind);
            DateTime beginDateTime;
            if (!DateTime.TryParse(BeginTimeDatePicker.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out beginDateTime))
                return;
            DateTime endDateTime;
            if (!DateTime.TryParse(EndTimeDatePicker.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out endDateTime))
                return;
            var timeInformation = new TimeEntity(new TimeSpan(expirationHoursTime, 0, 0), launchTime, selectedDaysOfWeek, beginDateTime, endDateTime);
            Rule rule;
            switch (ruleKind)
            {
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl = RuleConfigurationControlPlaceHolder.Controls[0] as NotifyByTimeRuleConfigurationControl;
                    var notifyByTimeSubject = notifyByTimeRuleConfigurationControl.Subject;
                    var notifyByTimeInformation = notifyByTimeRuleConfigurationControl.Information;
                    rule = new NotifyByTimeRule(RuleDiscriptionTextBox.Text, notifyByTimeSubject, notifyByTimeInformation, timeInformation);
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl = RuleConfigurationControlPlaceHolder.Controls[0] as NotifyLastUserRuleConfigurationControl;
                    var notifyLastUserRuleSubject = notifyLastUserRuleConfigurationControl.Subject;
                    rule = new NotifyLastUserRule(RuleDiscriptionTextBox.Text, notifyLastUserRuleSubject, timeInformation);
                    break;
                case RuleKind.AddWorkTime:
                    var insertTimeOffRuleConfigurationControl = RuleConfigurationControlPlaceHolder.Controls[0] as InsertTimeOffRuleConfigurationControl;
                    var timeInterval = insertTimeOffRuleConfigurationControl.TimeInterval;
                    rule = new InsertTimeOffRule(RuleDiscriptionTextBox.Text, timeInterval, timeInformation);
                    break;
                case RuleKind.NotReportToMoscow:
                    rule = new NotReportToMoscowRule(RuleDiscriptionTextBox.Text, timeInformation);
                    break;
                default:
                    throw new ArgumentException();
            }

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.SaveRule(rule);

            var urlForRedirection = string.Format("{0}?RuleID={1}&RuleKind={2}", Request.Url, rule.ID, rule.RuleType);
            Response.Redirect(urlForRedirection, false);
        }
    }
}