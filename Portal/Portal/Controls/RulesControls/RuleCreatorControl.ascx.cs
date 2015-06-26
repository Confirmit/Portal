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
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class RuleCreatorControl : UserControl
    {
        public Action RefreshRulesListAction;

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
            //var launchTime = new DateTime(0, 0, 0, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            //TODO
            var launchTime = DateTime.Now;
            var ruleTypeString = RuleTypesDropDownList.SelectedValue;
            Rule rule = null;
            switch (ruleTypeString)
            {
                case "NotifyByTime":
                    rule = new NotifyByTimeRule();
                    break;
                case "NotifyLastUser":
                    rule = new NotifyLastUserRule();
                    break;
                case "AddWorkTime":
                    //TODO
                    break;
                case "NotReportToMoscow":
                    rule = new NotReportToMoscowRule();
                    break;
                default:
                    throw new ArgumentException();
            }
            DateTime dateTime;
            if (!DateTime.TryParse(BeginTimeDatePicker.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
                return;
            rule.BeginTime = dateTime;
            if (!DateTime.TryParse(EndTimeDatePicker.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
                return;
            rule.EndTime = dateTime;
            rule.RuleDetails = new RuleDetails
            {
                TimeInformation =
                    new TimeEntity(new TimeSpan(expirationHoursTime, 0, 0), launchTime, selectedDaysOfWeek)
            };
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            try
            {
                ruleRepository.SaveRule(rule);
            }
            catch
            {
                Console.WriteLine();
            }

            if (RefreshRulesListAction != null)
                RefreshRulesListAction();
            Response.Redirect("~/Admin/AdminRulesListPage.aspx", false);
        }
    }
}