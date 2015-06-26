using System;
using System.Globalization;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class NotReportToMoscowRuleConfigurationControl : UserControl
    {
        public Action RefreshRulesListAction;

        public int RuleId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        public void SetDateTime(DateTime beginDateFromQueryStringInInvariantCulture,
             DateTime endDateFromQueryStringInInvariantCulture)
        {
            //Получение  DateTime в текущей культуре из строковых инвариантных строк
            DateTime beginDateTimeInCurrentCulture;
            DateTime endDateTimeInCurrentCulture;
            var isParsedDateTimeFromQueryInvariantCulture = InitializeDateTimeByString(beginDateFromQueryStringInInvariantCulture.ToString(),
                endDateFromQueryStringInInvariantCulture.ToString(),
                out beginDateTimeInCurrentCulture, out endDateTimeInCurrentCulture, CultureInfo.CurrentCulture);
            if (!isParsedDateTimeFromQueryInvariantCulture)
                return;

            BeginTimeDatePicker.Text = beginDateTimeInCurrentCulture.ToShortDateString();
            EndTimeDatePicker.Text = endDateTimeInCurrentCulture.ToShortDateString();
        }

        private bool InitializeDateTimeByString(string beginDateString, string endDateString,
           out DateTime beginDateTime, out  DateTime endDateTime, CultureInfo cutureInfo)
        {
            if (!DateTime.TryParse(beginDateString, cutureInfo, DateTimeStyles.None, out beginDateTime))
            {
                endDateTime = new DateTime();
                return false;
            }
            if (!DateTime.TryParse(endDateString, cutureInfo, DateTimeStyles.None, out endDateTime))
                return false;
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SaveChangesButton.Click += SaveChangesButtonOnClick;
        }

        public void SaveChangesButtonOnClick(object sender, EventArgs eventArgs)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            var editingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(RuleId);
            editingRule.TimeInformation.BeginTime = BeginTimeDatePicker.Date;
            editingRule.TimeInformation.EndTime = EndTimeDatePicker.Date;
            ruleRepository.SaveRule(editingRule);
            if (RefreshRulesListAction != null)
                RefreshRulesListAction();
            Response.Redirect("~/Admin/AdminRulesListPage.aspx", false);
        }
    }
}