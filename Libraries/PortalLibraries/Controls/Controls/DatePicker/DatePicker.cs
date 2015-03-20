using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.DatePicker
{
    /// <summary>
    ///	Represents the control to get the date and time.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
    public class DatePicker : WebControl, INamingContainer
    {
        private readonly TextBox _txtDate = new TextBox();

        private string Selector
        {
            get { return "$(\"#" + _txtDate.ClientID + "\")"; }
        }

        private string InitializationScript
        {
            get
            {
                var result = Selector + ".pickmeup({";
                result += "language : '" + GetCurrentLanguage().ToLower() + "'";

                return result + "});";
            }
        }

        [DefaultValue(typeof(DateTime)),
        Category("Date Selection"),
        Bindable(true, BindingDirection.TwoWay)]
        public DateTime SelectedDate
        {
            get
            {
                DateTime result;
                if (DateTime.TryParse(_txtDate.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return DateTime.Today;
            }
            set
            {
                _txtDate.Text = (value == DateTime.MinValue) ? string.Empty : value.ToString(CultureInfo.CurrentCulture); 
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
          
            IncludeJavaScript(datePickerResourceName);
            IncludeStyle(cssResourceName);

            Page.ClientScript.RegisterStartupScript(GetType(), "DatePicker" + ClientID, InitializationScript, true);

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            Controls.Add(_txtDate);
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

        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }
    }
}