using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Portal.Controls.RulesControls.RuleConfigurationControls;

namespace Portal.Controls.RulesControls
{
    public partial class RuleConfigurationControl : System.Web.UI.UserControl
    {
        public int RuleId
        {
            get { return ViewState["RuleID"] is int ? (int) ViewState["RuleID"] : -1; }
            set { ViewState["RuleID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SaveRuleCongigurationButton.Click += SaveRuleCongigurationButtonOnClick;
        }

        private void SaveRuleCongigurationButtonOnClick(object sender, EventArgs eventArgs)
        {
            var selectedCheckboxItems = CommonRuleSettingsControl.DaysOfWeekCheckBoxes.Items.Cast<ListItem>().Where(x => x.Selected).Select(item => item.Value).ToArray();
            var selectedDaysOfWeek = new HashSet<DayOfWeek>(selectedCheckboxItems.Select(selectedItem => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), selectedItem)));
            var expirationHoursTime = int.Parse(CommonRuleSettingsControl.ExpirationTime.Text);
            var launchTimeText = CommonRuleSettingsControl.LaunchTime.Text + ":00";
            var launchTime = TimeSpan.Parse(launchTimeText);
            
            DateTime beginDateTime;
            if (!DateTime.TryParse(CommonRuleSettingsControl.BeginTime.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out beginDateTime))
                return;
            DateTime endDateTime;
            if (!DateTime.TryParse(CommonRuleSettingsControl.EndTime.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out endDateTime))
                return;
            var timeInformation = new TimeEntity(new TimeSpan(expirationHoursTime, 0, 0), launchTime, selectedDaysOfWeek, beginDateTime, endDateTime);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);

            var rule = ruleRepository.GetRuleById(RuleId);
            rule.Description = CommonRuleSettingsControl.RuleDiscription.Text;
            rule.TimeInformation = timeInformation;
            switch (rule.RuleType)
            {
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as NotifyByTimeRuleConfigurationControl;
                    var notifyByTimeSubject = notifyByTimeRuleConfigurationControl.Subject;
                    var notifyByTimeInformation = notifyByTimeRuleConfigurationControl.Information;
                    var notifyByTimeRule = (NotifyByTimeRule) rule;
                    notifyByTimeRule.Subject = notifyByTimeSubject;
                    notifyByTimeRule.Information = notifyByTimeInformation;
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as NotifyLastUserRuleConfigurationControl;
                    var notifyLastUserRuleSubject = notifyLastUserRuleConfigurationControl.Subject;
                    var notifyLastUserRule = (NotifyLastUserRule) rule;
                    notifyLastUserRule.Subject = notifyLastUserRuleSubject;
                    break;
                case RuleKind.AddWorkTime:
                    var insertTimeOffRuleConfigurationControl = CommonRuleSettingsControl.RuleConfiguration.Controls[0] as InsertTimeOffRuleConfigurationControl;
                    var timeInterval = insertTimeOffRuleConfigurationControl.TimeInterval;
                    var insertTimeOffRule = (InsertTimeOffRule) rule;
                    insertTimeOffRule.Interval = timeInterval;
                    break;
                case RuleKind.NotReportToMoscow:
                    break;
                default:
                    throw new ArgumentException();
            }

            ruleRepository.SaveRule(rule);
            Response.Redirect(Request.Url.ToString(), false);
        }

        public void SetRuleProperty(Rule rule, RuleKind ruleKind)
        {
            CommonRuleSettingsControl.DaysOfWeekCheckBoxes.DataSource = Enum.GetNames(typeof(DayOfWeek));
            CommonRuleSettingsControl.DaysOfWeekCheckBoxes.DataBind();
            CommonRuleSettingsControl.RuleDiscription.Text = rule.Description;
            CommonRuleSettingsControl.BeginTime.Text = rule.TimeInformation.BeginTime.ToString();
            CommonRuleSettingsControl.EndTime.Text = rule.TimeInformation.EndTime.ToString();
            CommonRuleSettingsControl.ExpirationTime.Text = rule.TimeInformation.ExpirationTime.Hours.ToString();
            CommonRuleSettingsControl.LaunchTime.Text = string.Format("{0}:{1}", rule.TimeInformation.LaunchTime.Hours,
                rule.TimeInformation.LaunchTime.Minutes);
            var daysOfWeek = rule.TimeInformation.DaysOfWeek;
            var daysOfWeekStrings = daysOfWeek.Select(dayOfWeek => dayOfWeek.ToString()).ToList();
            foreach (var item in CommonRuleSettingsControl.DaysOfWeekCheckBoxes.Items)
            {
                if (daysOfWeekStrings.Contains(item.ToString()))
                {
                    (item as ListItem).Selected = true;
                }
            }

            switch (ruleKind)
            {
                case RuleKind.NotifyByTime:
                    var notifyByTimeRuleConfigurationControl =
                       (NotifyByTimeRuleConfigurationControl)LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyByTimeRuleConfigurationControl.ascx");
                    var notifyByTimeRule = (NotifyByTimeRule)rule;

                    notifyByTimeRuleConfigurationControl.Subject = notifyByTimeRule.Subject;
                    notifyByTimeRuleConfigurationControl.Information = notifyByTimeRule.Information;
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(notifyByTimeRuleConfigurationControl);
                    break;
                case RuleKind.NotifyLastUser:
                    var notifyLastUserRuleConfigurationControl =
                        (NotifyLastUserRuleConfigurationControl)LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/NotifyLastUserRuleConfigurationControl.ascx");
                    var notifyLastUserRule = (NotifyLastUserRule)rule;
                    notifyLastUserRuleConfigurationControl.Subject = notifyLastUserRule.Subject;
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(notifyLastUserRuleConfigurationControl);
                    break;
                case RuleKind.AddWorkTime:
                    var insertTimeOffRuleConfigurationControl =
                        (InsertTimeOffRuleConfigurationControl)LoadControl(
                            "~/Controls/RulesControls/RuleConfigurationControls/InsertTimeOffRuleConfigurationControl.ascx");
                    var insertTimeOffRule = (InsertTimeOffRule)rule;
                    insertTimeOffRuleConfigurationControl.TimeInterval = insertTimeOffRule.Interval;
                    CommonRuleSettingsControl.RuleConfiguration.Controls.Add(insertTimeOffRuleConfigurationControl);
                    break;
                case RuleKind.NotReportToMoscow:
                    break;
                default:
                    throw new ArgumentException();
            }

        }
    }
}