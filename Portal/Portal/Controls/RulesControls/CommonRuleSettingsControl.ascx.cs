using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Controls.DatePicker;

namespace Portal.Controls.RulesControls
{
    public partial class CommonRuleSettingsControl : UserControl
    {
        public TextBox RuleDiscription
        {
            get { return RuleDiscriptionTextBox; }
        }

        public DatePicker BeginTime
        {
            get { return BeginTimeDatePicker; }
        }

        public DatePicker EndTime
        {
            get { return EndTimeDatePicker; }
        }

        public TextBox ExpirationTime
        {
            get { return ExpirationTimeTextBox; }
        }

        public TextBox LaunchTime
        {
            get { return LaunchTimeTextBox; }
        }

        public PlaceHolder RuleConfiguration
        {
            get { return RuleConfigurationControlPlaceHolder; }
        }

        public CheckBoxList DaysOfWeekCheckBoxes
        {
            get { return DaysOfWeekCheckBoxList; }
        }

        public HtmlInputCheckBox SelectAllDay
        {
            get { return SelectAllDayCheckBox; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var jsCode = string.Format("javascript: CheckBoxListSelect ('{0}', '{1}');",
                    DaysOfWeekCheckBoxList.ClientID, SelectAllDayCheckBox.ClientID);
                SelectAllDayCheckBox.Attributes.Add("onclick", jsCode);
            }
        }
    }
}