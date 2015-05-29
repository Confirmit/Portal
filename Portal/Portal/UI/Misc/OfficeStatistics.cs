using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using Core;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

namespace PortalWeb.UI
{
    /// <summary>
    /// Элемент управления для отображения статистики по офису.
    /// </summary>
    public class OfficeStatistics : WebControl
    {
        #region Свойства

        /// <summary>
        /// Дата начала интервала расчета статистики.
        /// </summary>
        public DateTime BeginDate
        {
            get
            {
                if (ViewState["BeginDate"] == null)
                    return DateTime.MinValue;
                return (DateTime)ViewState["BeginDate"];
            }
            set { ViewState["BeginDate"] = value; }
        }

        /// <summary>
        /// Дата окончания интервала расчета статистики.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                if (ViewState["EndDate"] == null)
                    return DateTime.MinValue;
                return (DateTime)ViewState["EndDate"];
            }
            set { ViewState["EndDate"] = value; }
        }

        #endregion

        /// <summary>
        /// Отрисовывает элемент управления.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            Visible = false;
            if (BeginDate == DateTime.MinValue || EndDate == DateTime.MinValue)
                return;

            var officeStatistics = PeriodOfficeStatistics.GetOfficeStatistics(BeginDate, EndDate);
            if (officeStatistics == null
                || officeStatistics.UserStatistics.Length == 0
                || officeStatistics.UserStatistics[0].DayWorkTimes.Length == 0)
                return;

            Visible = true;

            writer.WriteLine(@"<div id='container' style='height: 100%;'>");
            writer.WriteLine(@"<div id='updatedTable'>");
            writer.WriteLine(@"<div class='firstColumn' style='border: 1px solid #9d9d9d'>");
            writer.WriteLine(@"<table class='innerTable'>");
            writer.WriteLine(@"<tbody>");

            FillFirstColumnWithFullUserName(writer, officeStatistics);
            writer.WriteLine(@"</tbody>");
            writer.WriteLine(@"</table>");
            writer.WriteLine(@"</div>");//firstcolumn

            writer.WriteLine(@"<div id='secondColumnWithHeader'>");

            writer.WriteLine(@"<div class='customHeader'>");
            writer.WriteLine(@"<table class='innerTable'>");
            writer.WriteLine(@"<thead>");
            writer.WriteLine(@"<tr>");
            FillHeaderItems(writer, officeStatistics);
            writer.WriteLine(@"</tr>");
            writer.WriteLine(@"</thead>");
            writer.WriteLine(@"</table>");
            writer.WriteLine(@"</div>");//customHeader

            writer.WriteLine(@"<div class='customTable'>");
            writer.WriteLine(@"<table class='innerTable'>");
            writer.WriteLine(@"<thead>");
            writer.WriteLine(@"<tr>");
            FillInternalTable(writer, officeStatistics);
            writer.WriteLine(@"</tr>");
            writer.WriteLine(@"</thead>");
            writer.WriteLine(@"</table>");
            writer.WriteLine(@"</div>"); //customTable

            writer.WriteLine(@"</div>");//secondColumnWithHeader

            writer.WriteLine(@"</div>");//updatedTable
            writer.WriteLine(@"</div>");//updatedTable

            writer.WriteLine(@"<script src='/Scripts/external/jquery-1.6.4.min.js'></script>");
            writer.WriteLine(@"<script src='/Scripts/statistics_table.js'></script>");
            writer.WriteLine(@"<script src='/Scripts/statistics_table.autoresize.js'></script>");
            writer.WriteLine(@"<link href='/App_Themes/ConfirmitPortal/css/StatisticsStyle.css' rel='stylesheet' type='text/css'/>");
        }

        private void FillHeaderItems(HtmlTextWriter writer, PeriodOfficeStatistics officeStatistics)
        {
            foreach (var dayWorkTime in officeStatistics.UserStatistics[0].DayWorkTimes)
            {
                var calendarItem = new CalendarItem(dayWorkTime);
                string contentValue;
                if(CultureManager.CurrentLanguage == CultureManager.Languages.English)
                    contentValue = dayWorkTime.Date.ToString("MM/dd");
                else
                    contentValue = dayWorkTime.Date.ToString("dd.MM");

                if (calendarItem.IsWeekend)
                    writer.WriteLine("<th class='weekend statistic-table-internal-th'>{0}</th>", contentValue);
                else
                    writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", contentValue);
            }

            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.TotalTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.RateTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.DiffTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.DomainName);
        }

        private void FillFirstColumnWithFullUserName(HtmlTextWriter writer, PeriodOfficeStatistics officeStatistics)
        {
            for (var i = 0; i < officeStatistics.UserStatistics.Length; i++)
            {
                var userOfficeStatistics = officeStatistics.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");

                var beginTimeString = BeginDate.ToString(CultureInfo.InvariantCulture);
                var endTimeString = EndDate.ToString(CultureInfo.InvariantCulture);
                writer.WriteLine("<td class='statistic-table-first-td'><a href='/Statistics/UserStatistics.aspx?UserID={0}&BeginDate={1}&EndDate={2}' style=''>{3}</a></td></tr>", userOfficeStatistics.User.ID, beginTimeString, endTimeString, userOfficeStatistics.User.FullName);
            }
        }

        private void FillInternalTable(HtmlTextWriter writer, PeriodOfficeStatistics officeStatistics)
        {
            for (var i = 0; i < officeStatistics.UserStatistics.Length; i++)
            {
                var userStatistic = officeStatistics.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");


                foreach (var dayWorkTime in userStatistic.DayWorkTimes)
                {
                    var calendarItem = new CalendarItem(dayWorkTime);
                    var contentValue = DateTimePresenter.GetTime(dayWorkTime.WorkTime);
                    if (dayWorkTime.WorkTime == TimeSpan.Zero)
                        contentValue = string.Empty;

                    var cellColor = new Color();
                    if (dayWorkTime.WorkTime == TimeSpan.Zero && !calendarItem.IsWeekend)
                    {
                        WorkEvent workEvent = WorkEvent.GetCurrentEventOfDate((int)userStatistic.User.ID, dayWorkTime.Date);

                        if (workEvent != null)
                        {
                            switch (workEvent.EventType)
                            {
                                case WorkEventType.BusinessTrip:
                                    contentValue = "Trip";
                                    cellColor = Color.LightSlateGray;
                                    break;

                                case WorkEventType.Ill:
                                    contentValue = "Ill";
                                    cellColor = Color.LightPink;
                                    break;

                                case WorkEventType.TrustIll:
                                    contentValue = "Trust Ill";
                                    cellColor = Color.LightPink;
                                    break;

                                case WorkEventType.Vacation:
                                    contentValue = "Vacation";
                                    cellColor = Color.LightYellow;
                                    break;
                            }
                        }
                    }

                    if (calendarItem.IsWeekend)
                        writer.WriteLine("<td class='weekend statistic-table-internal-td'>{0}</td>", contentValue);
                    else
                        writer.WriteLine(
                            "<td class='statistic-table-internal-td' style='background-color: {0};' align='center'>{1}</td>",
                            cellColor.ToKnownColor(),
                            contentValue);
                }

                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>",
                    DateTimePresenter.GetTime(userStatistic.TotalWorkTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>",
                    DateTimePresenter.GetTime(userStatistic.RateTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>",
                    DateTimePresenter.GetTime(userStatistic.RateTime - userStatistic.TotalWorkTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>", GetDomainNameByUserStatistic(userStatistic));
                writer.WriteLine("</tr>");
            }
        }

        private string GetDomainNameByUserStatistic(UserOfficeStatistics userStatistic)
        {
            var strDomainValue = String.Empty;
            foreach (var domain in userStatistic.User.DomainNames)
            {
                var names = domain.Split('\\');
                var name = (names.Length == 1) ? names[0] : names[names.Length - 1];

                strDomainValue += String.IsNullOrEmpty(strDomainValue) ? name : ", " + name;
            }
            return strDomainValue;
        }

        /// <summary>
        /// Заставляет элемент управления показать статистику.
        /// </summary>
        /// <param name="begin">Начало интервала статистики.</param>
        /// <param name="end">Конец интервала статистики.</param>
        public void ShowStatistics(DateTime begin, DateTime end)
        {
            BeginDate = begin;
            EndDate = end;
        }
    }
}
