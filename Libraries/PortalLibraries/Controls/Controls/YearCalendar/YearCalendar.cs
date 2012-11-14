using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.YearCalendar
{
    [ToolboxData("<{0}:YearCalendar runat=\"server\"></{0}:YearCalendar>")]
    public class YearCalendar : Control, IPostBackEventHandler
    {
        public Int32 WatchYear
        {
            get
            {
                if (ViewState["YearCalendar_WatchYear"] == null)
                    ViewState["YearCalendar_WatchYear"] = DateTime.Today.Year;
                return (Int32) ViewState["YearCalendar_WatchYear"];
            }
            set { ViewState["YearCalendar_WatchYear"] = value; }
        }

        public DateTime SelectedDate
        {
            get
            {
                if (ViewState["YearCalendar_SelectedDate"] == null)
                    ViewState["YearCalendar_SelectedDate"] = DateTime.Today;
                return (DateTime)ViewState["YearCalendar_SelectedDate"];
            }
            set { ViewState["YearCalendar_SelectedDate"] = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            HtmlTable table = new HtmlTable();
            table.Border = 0;
            table.CellPadding = 0;
            table.CellSpacing = 1;
            table.Width = "100%";
            table.ID = ClientID;

            for (int nQuarter = 0; nQuarter < 4; nQuarter++)
            {
                HtmlTableRow tr = new HtmlTableRow();
                table.Rows.Add(tr);
                for (int nQMonth = 1; nQMonth <= 3; nQMonth++)
                {
                    HtmlTableCell td = new HtmlTableCell();
                    tr.Cells.Add(td);
                    td.VAlign = "top";
                    td.Align = "center";
                    td.Attributes.Add("class", "control-calendar-body-month");

                    DateTime dtCurrentMonth = new DateTime(WatchYear, 3 * nQuarter + nQMonth, 1);

                    Label lblMonth = new Label();
                    td.Controls.Add(lblMonth);
                    lblMonth.CssClass = "control-calendar-label";
                    lblMonth.Text = dtCurrentMonth.ToString("MMMM");

                    Calendar cal = new Calendar();
                    cal.ID = ClientID;
                    td.Controls.Add(cal);
                    cal.ToolTip = lblMonth.Text;
                    cal.FirstDayOfWeek = FirstDayOfWeek.Monday;
                    cal.VisibleDate = new DateTime(WatchYear, 3 * nQuarter + nQMonth, 1);
                    cal.Width = new Unit(100, UnitType.Percentage);
                    cal.CssClass = "control-calendar-month-box";
                    cal.ShowGridLines = true;
                    cal.ShowTitle = false;
                    cal.ShowNextPrevMonth = false;
                    cal.SelectionMode = CalendarSelectionMode.None;
                    cal.DayHeaderStyle.CssClass = "control-calendar-month-body-day-header";
                    cal.DayStyle.CssClass = "control-calendar-month-body-weekday";
                    cal.WeekendDayStyle.CssClass = "control-calendar-month-body-weekend";
                    cal.OtherMonthDayStyle.CssClass = "control-calendar-month-body-weekday";
                    cal.OtherMonthDayStyle.ForeColor = System.Drawing.Color.FromArgb(0xfa, 0xfa, 0xfa);//"#fafafa";
                    cal.DayRender += new DayRenderEventHandler(YearCalendar_RenderDay);
                }
            }

            table.RenderControl(writer);
        }

        public event DayRenderEventHandler RenderDay;

        protected virtual void YearCalendar_RenderDay(object sender, DayRenderEventArgs e)
        {
            if (RenderDay != null)
            {
                RenderDay(sender, e);
            }
        }
        
        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            SelectedDate = DateTime.ParseExact(eventArgument, 
                                                "dd.MM.yyyy", 
                                                culture, 
                                                System.Globalization.DateTimeStyles.AllowWhiteSpaces);

            OnClick(new EventArgs());
        }

        #endregion

       
        // Defines the Click event.
        public event EventHandler DaySelected;

        //Invoke delegates registered with the Click event.
        protected virtual void OnClick(EventArgs e)
        {
            if (DaySelected != null)
            {
                DaySelected(this, e);
            }
        }
    }
}
