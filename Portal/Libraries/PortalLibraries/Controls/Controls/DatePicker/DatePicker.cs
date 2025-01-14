using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.DatePicker
{
    public class DatePicker : TextBox
    {


        private string InitializationScript
        {
            get
            {
                var settings = new JavaScriptSerializer().Serialize(GetSettings());
                var script = string.Format("$(\"#{0}\").datePickerControl({1});", ClientID, settings);

                return script;
            }
        }

        private object GetLocale()
        {
            if (DateTimeFormatInfo.CurrentInfo != null)
                return new
                {
                    days = DateTimeFormatInfo.CurrentInfo.DayNames,
                    daysShort = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames,
                    daysMin = DateTimeFormatInfo.CurrentInfo.ShortestDayNames,
                    months = DateTimeFormatInfo.CurrentInfo.MonthNames,
                    monthsShort = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames
                };

            return null;
        }

        private object GetSettings()
        {
            return new
            {
                hideOnSelect = _hideOnSelect,
                format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                locale = GetLocale(),
            };
        }

        [Bindable(true, BindingDirection.TwoWay)]
        public DateTime Date
        {
            get
            {
                DateTime result;
                if (!string.IsNullOrEmpty(Text) && DateTime.TryParse(Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return DateTime.Today;
            }
            set
            {
                Text = (value == DateTime.MinValue) ? string.Empty : value.ToString(CultureInfo.CurrentCulture); 
            }
        }

        private bool _hideOnSelect = true;
        [Bindable(true, BindingDirection.TwoWay)]
        public bool HideOnSelect
        {
            get { return _hideOnSelect; }
            set { _hideOnSelect = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            const string cssResourceName = "Controls.DatePicker.css.DatePicker.css";
            const string datePickerResourceName = "Controls.DatePicker.scripts.DatePicker.js";
            const string pickMeUpResourceName = "Controls.DatePicker.scripts.PickMeUp.js";

            IncludeJavaScript(datePickerResourceName, pickMeUpResourceName);
            IncludeStyle(cssResourceName);

            Page.ClientScript.RegisterStartupScript(GetType(), "DatePicker" + ClientID, InitializationScript, true);

            base.OnPreRender(e);
        }

        private void IncludeStyle(string resourceName)
        {
            var cssUrl = Page.ClientScript.GetWebResourceUrl(GetType(), resourceName);
            var cssLink = new HtmlLink {Href = cssUrl};
           
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
           
            Page.Header.Controls.Add(cssLink);
        }

        private void IncludeJavaScript(params string[] resourceNames)
        {
            foreach (var resourceName in resourceNames)
            {
                var urlScript = Page.ClientScript.GetWebResourceUrl(GetType(), resourceName);

                if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), resourceName))
                    Page.ClientScript.RegisterClientScriptInclude(GetType(), resourceName, urlScript);
            }
        }
    }
}