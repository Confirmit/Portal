using System;
using System.ComponentModel;
using System.Web.UI;

using System.Globalization;

namespace Controls.DatePicker
{
	/// <summary>
	///	Represents the control to get the date and time.
	/// </summary>
    [ToolboxData("<{0}:DatePicker runat=\"server\"></{0}:DatePicker>")]
    public class DatePicker : Control, INamingContainer
	{
		#region Properties

		/// <summary>
		///	Gets or sets the date for datepicker.
		/// </summary>
		public DateTime Value
		{
            get
            {
                if (DesignMode)
                    return DateTime.Today;

                if (_value != null)
                    return (DateTime)_value;
                
                String strValue = Page.Request.Form[ValueId];
                if (strValue == null)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    try
                    {
                        IFormatProvider culture = new CultureInfo("en-US", true);
                        DateTime dt = (String.IsNullOrEmpty(strValue) ? 
                            DateTime.MinValue :
                            DateTime.ParseExact(strValue, _formatDDMMYYYY, culture, DateTimeStyles.AllowWhiteSpaces));

                        _value = dt;

                        return dt;
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
            }
			set
			{
                _value = value;
			}
		}

        //set all for read only
        public Boolean DataReadOnly
        {
            set
            {
                _readOnly = value;
            }
            get
            {
                return _readOnly;
            }
        }

        public String OnChangeJS
        {
            set
            {
                _onChange = value;
            }
            get
            {
                return _onChange;
            }
        }

        public String AttributeStyleInput
        {
            set { _attrStyleInput = value; }
            get { return _attrStyleInput; }
        }

        // PROPERTY:: MinYear
        [Category("Behavior")]
        [Description("Set min year in drop down years. Default: Today year - 50")]
        public Int32 MinYear
        {
            get { return _minYear; }
            set { _minYear = value; }
        }

        // PROPERTY:: MaxYear
        [Category("Behavior")]
        [Description("Set max year in drop down years. Default: Today year + 50")]
        public Int32 MaxYear
        {
            get { return _maxYear; }
            set { _maxYear = value; }
        }

        // PROPERTY:: ValueIfNull
        [Category("Behavior")]
        [Description("Set value to set if text in textbox is null")]
        public DateTime ValueIfNull
        {
            get { return valueIfNull; }
            set { valueIfNull = value; }
        }

        // PROPERTY:: AutoPostBack
        [Category("Behavior")]
        [Description("Use post back on select or enter")]
        public Boolean AutoPostBack
        {
            get { return autoPostBack; }
            set { autoPostBack = value; }
        }

        // PROPERTY:: HiddenElementID
        [Category("Behavior")]
        [Description("HiddenElementID")]
        public String HiddenElementID
        {
            get { return hiddenElementID; }
            set { hiddenElementID = value; }
        }

		#endregion

		#region Web Form Designer generated code
		/// <summary>
		/// Executes on page initialization.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		#region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.IsPostBack)
                _value = null;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!(Page.ClientScript.IsStartupScriptRegistered("DatePicker")))
                Page.ClientScript.RegisterStartupScript(GetType(), "DatePicker", "<script  language=\"javascript\" src=\""
                    + Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.DatePicker.DatePicker.js") +
                    "\"></script>");

            if (!Page.ClientScript.IsStartupScriptRegistered("DatePickerStylescss"))
                Page.ClientScript.RegisterStartupScript(GetType(), "DatePickerStylescss", "<link  href=\""
                    + Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.DatePicker.Styles.css")
                    + "\" rel=\"stylesheet\" type=\"text/css\" />");

        } 

		/// <summary>
		/// Renders the control and writes HTML string into it.
		/// </summary>
		/// <param name="writer">The <see cref="System.Web.UI.HtmlTextWriter"/> to write control into.</param>
		protected override void Render(HtmlTextWriter writer)
		{
            if (!(Visible))
                return;

            writer.Write(writer.NewLine);

            writer.Write("<input type=\"text\" name=\"{0}\" id=\"{0}\" maxlength=\"10\" value=\""
                + GetStringDate() + "\" title=\"Date in format dd.mm.yyyy\""
                + " style=\"width: 70px;font-family:Arial;font-size:10px;font-weight:normal;border-style:groove;" + AttributeStyleInput + "\" "
                + ((DataReadOnly) ? " readonly=\"readonly\" " : "onchange=\"OnChangeDateByType({0});" + OnChangeJS + "\"")
                + " onfocus=\"OnDataPickerFocus({0});\" onkeypress = \"DatePickerValue_onkeypress({0}, '{1}', '{2}')\"  " + "/>&nbsp;",
                ValueId, autoPostBack.ToString(), hiddenElementID);

            CreateButtonCalendar(writer);

            writer.Write(writer.NewLine);
            writer.Write("<span id=\"theDatePicker" + ClientID + "\" class=\"DatePicker\">");
            writer.Write(writer.NewLine);
            writer.Write("</span>");

		    _value = null;
        }
		#endregion

        #region Helpers

        private String GetStringDate()
        {
            return (Value == DateTime.MinValue) ? "" : Value.ToString(_formatDDMMYYYY);
        }

        protected void CreateButtonCalendar(HtmlTextWriter htmlWriter)
        {
            if (DataReadOnly)
                return;

            htmlWriter.Write("<img src=\"" + Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.DatePicker.images.buttons.dp.gif")
                    + "\"  class=\"DatePickerBtnShow\" onclick=\"oDataPicker{1} = DatePickerShow({0}, {0}Ch, 'theDatePicker{1}', oDataPicker{1}, {2}, {3}, '{4}', '{5}', '{6}');\" "
                    + " title=\"Select\" name=\"{0}Ch\" id=\"{0}Ch\" />", ValueId, ClientID, MinYear, MaxYear, ValueIfNull.ToString(_formatDDMMYYYY), autoPostBack.ToString(), hiddenElementID);

            htmlWriter.Write(htmlWriter.NewLine);
            htmlWriter.Write("<script type=\"text/javascript\" language=\"javascript\">");
            htmlWriter.Write(htmlWriter.NewLine);
            htmlWriter.Write("var oDataPicker" + ClientID + ";");
            htmlWriter.Write(htmlWriter.NewLine);
            htmlWriter.Write("</script>");

            // Проверяем что уже регистрировался скрипт и потому не нужно еще раз регистрировать картинки
            if (!(Page.ClientScript.IsStartupScriptRegistered("DatePicker")))
            {
                RegisterImgOnClient(htmlWriter, "btn_prev", "Controls.DatePicker.images.datepicker.prev.gif");
                RegisterImgOnClient(htmlWriter, "btn_close", "Controls.DatePicker.images.datepicker.close.gif");
                RegisterImgOnClient(htmlWriter, "btn_monthafter", "Controls.DatePicker.images.datepicker.monthafter.gif");
                RegisterImgOnClient(htmlWriter, "btn_monthago", "Controls.DatePicker.images.datepicker.monthago.gif");
                RegisterImgOnClient(htmlWriter, "btn_next", "Controls.DatePicker.images.datepicker.next.gif");
                RegisterImgOnClient(htmlWriter, "btn_today", "Controls.DatePicker.images.datepicker.today.gif");
                RegisterImgOnClient(htmlWriter, "btn_tomorrow", "Controls.DatePicker.images.datepicker.tomorrow.gif");
                RegisterImgOnClient(htmlWriter, "btn_weekafter", "Controls.DatePicker.images.datepicker.weekafter.gif");
                RegisterImgOnClient(htmlWriter, "btn_weekago", "Controls.DatePicker.images.datepicker.weekago.gif");
                RegisterImgOnClient(htmlWriter, "btn_yesterday", "Controls.DatePicker.images.datepicker.yesterday.gif");
            }
        }

        protected void RegisterImgOnClient(HtmlTextWriter writer, String imgName, String imagePath)
        {
            writer.Write(writer.NewLine);
            writer.Write("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\"/>",
                imgName, Page.ClientScript.GetWebResourceUrl(GetType(), imagePath));
        }

        protected String ValueId
        {
            get
            {
                return String.Format("{0}Vb", ClientID);
            }
        }

        #endregion

        #region Private Fields

	    private DateTime? _value = null;
        private const String _formatDDMMYYYY = "dd.MM.yyyy";
        private Boolean _readOnly = false;
        private String _onChange = "";
        private String _attrStyleInput = "; color:#000000;";
	    private Int32 _minYear = DateTime.Today.AddYears(-50).Year;
        private Int32 _maxYear = DateTime.Today.AddYears(50).Year;
	    private DateTime valueIfNull = DateTime.Today;
	    private Boolean autoPostBack = false;
	    private String hiddenElementID = "";
	    #endregion
    }
}
