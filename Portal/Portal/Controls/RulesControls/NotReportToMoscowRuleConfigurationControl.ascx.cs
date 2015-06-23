using System;
using System.Globalization;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class NotReportToMoscowRuleConfigurationControl : System.Web.UI.UserControl
    {
        public int RuleId { get; set; }

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
            editingRule.BeginTime = BeginTimeDatePicker.Date;
            editingRule.EndTime = EndTimeDatePicker.Date;
            editingRule.Save();
        }
    }
}