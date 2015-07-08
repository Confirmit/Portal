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

        public TimeSpan ExpirationTime
        {
            get
            {
                return new TimeSpan(ExpirationTimeSelectorControl.Hours,
                    ExpirationTimeSelectorControl.Minutes, ExpirationTimeSelectorControl.Seconds);
            }
            set
            {
                ExpirationTimeSelectorControl.Hours = value.Hours;
                ExpirationTimeSelectorControl.Minutes = value.Minutes;
                ExpirationTimeSelectorControl.Seconds = value.Seconds;
            }
        }

        public TimeSpan LaunchTime
        {
            get
            {
                return new TimeSpan(LaunchTimeSelectorControl.Hours,
                    LaunchTimeSelectorControl.Minutes, LaunchTimeSelectorControl.Seconds);
            }
            set
            {
                LaunchTimeSelectorControl.Hours = value.Hours;
                LaunchTimeSelectorControl.Minutes = value.Minutes;
                LaunchTimeSelectorControl.Seconds = value.Seconds;
            }
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
                var jsCode = string.Format("javascript: selectAllDaysOfWeekCheckBoxes ('{0}', '{1}');",
                    DaysOfWeekCheckBoxList.ClientID, SelectAllDayCheckBox.ClientID);
                SelectAllDayCheckBox.Attributes.Add("onclick", jsCode);
            }
        }
    }
}