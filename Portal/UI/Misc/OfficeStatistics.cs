using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
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

            // Получить статистику за данный период.
            var stat = PeriodOfficeStatistics.GetOfficeStatistics(BeginDate, EndDate);
            if (stat == null
                || stat.UserStatistics.Length == 0
                || stat.UserStatistics[0].DayWorkTimes.Length == 0)
                return;

            Visible = true;
            writer.WriteLine(@"<table><th>");
            writer.WriteLine(@"<div id='updatedTable'><header class='customHeader'><table class='innerTable'><thead><tr>");

            foreach (var dayWorkTime in stat.UserStatistics[0].DayWorkTimes)
            {
                WriteDataTime(writer, dayWorkTime, "th", false, null);
            }

            writer.WriteLine("<th>{0}</th>", Resources.Strings.TotalTime);
            writer.WriteLine("<th>{0}</th>", Resources.Strings.RateTime);
            writer.WriteLine("<th>{0}</th>", Resources.Strings.DiffTime);

            writer.WriteLine(@" </tr></thead></table></header><div class='firstColumn'><table class='innerTable'><tbody>");

            for (var i = 0; i < stat.UserStatistics.Length; i++)
            {
                var userStatistic = stat.UserStatistics[i];
                var strDomainValue = GetFullNameWithDomain(userStatistic);
                var fullNameWithDomainName = userStatistic.User.FullName + " (" + strDomainValue + ")";
                if (fullNameWithDomainName.Length > 40)
                {
                    fullNameWithDomainName = fullNameWithDomainName.Substring(0, 40) + "..";
                }
                writer.WriteLine("<tr><td>{0}</td></tr>", fullNameWithDomainName);

            }
            writer.WriteLine(@"</tbody></table></div>");
            writer.WriteLine(@"<div class='customTable'><table class='innerTable'><tbody>");

            // Создать строки данных.
            for (var i = 0; i < stat.UserStatistics.Length; i++)
            {
                var userStatistic = stat.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");


                foreach (var dwt in userStatistic.DayWorkTimes)
                {
                    WriteDataTime(writer, dwt, "td", true, userStatistic.User.ID);
                }

                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(userStatistic.TotalWorkTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(userStatistic.RateTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(userStatistic.RateTime - userStatistic.TotalWorkTime));
                writer.WriteLine("</tr>");
            }

            writer.WriteLine(@"</tbody></table></div></div>");
            writer.WriteLine(@"</table></th>");
            writer.WriteLine(@"<script src='/Scripts/external/jquery-1.6.4.min.js'></script>");
            writer.WriteLine(@"<script src='/Scripts/statistics_table.js'></script>");
            writer.WriteLine(@"<link href='/App_Themes/ConfirmitPortal/css/StatisticsStyle.css' rel='stylesheet' type='text/css'/>");
        }

        private string GetFullNameWithDomain(UserOfficeStatistics userStatistic)
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

        #region Методы

        /// <summary>
        /// Write date to container with style.
        /// </summary>
        /// <param name="writer">HtmlTextWriter.</param>
        /// <param name="dwt">Date.</param>
        /// <param name="container">HtmlContainer.</param>
        /// <param name="fWriteTime">Write date(false) or time(true).</param>
        /// <param name="userId">ID of user.</param>
        private void WriteDataTime(HtmlTextWriter writer,
            DayWorkTime dwt,
            String container,
            bool fWriteTime,
            int? userId)
        {
            var calendarItem = new CalendarItem(dwt);
            var strValue = fWriteTime
                                  ? DateTimePresenter.GetTime(dwt.WorkTime)
                                  : dwt.Date.ToString("dd/MM");

            var cellColor = new Color();
            if (userId != null
                 && dwt.WorkTime == TimeSpan.Zero
                 && !calendarItem.IsWeekend)
            {
                WorkEvent workEvent = WorkEvent.GetCurrentEventOfDate((int)userId, dwt.Date);

                if (workEvent != null)
                    switch (workEvent.EventType)
                    {
                        case WorkEventType.BusinessTrip:
                            strValue = "Trip";
                            cellColor = Color.LightSlateGray;
                            break;

                        case WorkEventType.Ill:
                            strValue = "Ill";
                            cellColor = Color.LightPink;
                            break;

                        case WorkEventType.TrustIll:
                            strValue = "Trust Ill";
                            cellColor = Color.LightPink;
                            break;

                        case WorkEventType.Vacation:
                            strValue = "Vacation";
                            cellColor = Color.LightYellow;
                            break;
                    }
            }

            if (calendarItem.IsWeekend)
                writer.WriteLine("<{0} class='weekend'>{1}</{0}>",
                                  container,
                                  strValue);
            else
                writer.WriteLine("<{0} style='background-color: {1};' align='center'>{2}</{0}>",
                                 container,
                                 cellColor.ToKnownColor(),
                                 strValue);
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
        #endregion
    }
}
