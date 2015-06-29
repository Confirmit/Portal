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

                DaysOfWeekCheckBoxList.DataSource = Enum.GetNames(typeof(DayOfWeek));
                DaysOfWeekCheckBoxList.DataBind();
            }
            CreateRuleButton.Click += CreateGroupButtonOnClick;
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
                    rule = new NotifyByTimeRule(RuleDiscriptionTextBox.Text, "Subject", "Information", timeInformation);
                    break;
                case RuleKind.NotifyLastUser:
                    rule = new NotifyLastUserRule(RuleDiscriptionTextBox.Text, "Subject", timeInformation);
                    break;
                case RuleKind.AddWorkTime:
                    rule = new InsertTimeOffRule(RuleDiscriptionTextBox.Text, TimeSpan.Zero, timeInformation);
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