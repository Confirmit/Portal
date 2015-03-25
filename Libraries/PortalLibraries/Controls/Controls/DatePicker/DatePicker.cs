using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.DatePicker
{
    public class DatePicker : TextBox
    {

        private string Selector
        {
            get { return "$(\"#" + ClientID + "\")"; }
        }

        private string InitializationScript
        {
            get
            {
                var settings = new JavaScriptSerializer().Serialize(GetSettings());

                return Selector + ".DatePickerControl(" + settings + ");";
            }
        }

        private object GetSettings()
        {
            return new
            {
                language = GetCurrentLanguage().ToLower()
            };
        }

        [Bindable(true, BindingDirection.TwoWay)]
        public DateTime SelectedDate
        {
            get
            {
                DateTime result;
                if (DateTime.TryParse(Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return DateTime.Today;
            }
            set
            {
                Text = (value == DateTime.MinValue) ? string.Empty : value.ToString(CultureInfo.CurrentCulture); 
            }
        }

        private string GetCurrentLanguage()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        protected override void OnPreRender(EventArgs e)
        {
            const string cssResourceName = "Controls.DatePicker.css.DatePicker.css";
            const string datePickerResourceName = "Controls.DatePicker.js.DatePicker.js";
            const string pickMeUpResourceName = "Controls.DatePicker.js.PickMeUp.js";

            IncludeJavaScript(new[]{datePickerResourceName, pickMeUpResourceName});
            IncludeStyle(cssResourceName);

            Page.ClientScript.RegisterStartupScript(GetType(), "DatePicker" + ClientID, InitializationScript, true);

            base.OnPreRender(e);
        }

        private void IncludeStyle(string resourceName)
        {
            var cssUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            var cssLink = new HtmlLink {Href = cssUrl};
           
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
           
            Page.Header.Controls.Add(cssLink);
        }

        private void IncludeJavaScript(IEnumerable<string> resourceNames)
        {
            foreach (var resourceName in resourceNames)
            {
                IncludeJavaScript(resourceName);
            }
        }

        private void IncludeJavaScript(string resourceName)
        {
            var urlScript = Page.ClientScript.GetWebResourceUrl(GetType(), resourceName);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), resourceName))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), resourceName, urlScript);
        }
    }
}