using System;
using System.Drawing;
using System.Text;
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
            var periodOfficeStatistics = PeriodOfficeStatistics.GetOfficeStatistics(BeginDate, EndDate);
            if (periodOfficeStatistics == null
                || periodOfficeStatistics.UserStatistics.Length == 0
                || periodOfficeStatistics.UserStatistics[0].DayWorkTimes.Length == 0)
                return;

            Visible = true;
            writer.WriteLine("<div style='width: 68%; overflow-x: scroll;  margin-left: 30%;  padding-bottom: 1px;'>");
            writer.WriteLine("<table border=\"1px\">");
            writer.Indent++;

            // Создать строку заголовков.
            writer.WriteLine("<tr class='gridview-headerrow'>");
            writer.WriteLine("<th></th>");
            foreach (var dwt in periodOfficeStatistics.UserStatistics[0].DayWorkTimes)
            {
                WriteDataTime(writer, dwt, "th", false, null);
            }

            writer.WriteLine("<th>{0}</th>", Resources.Strings.TotalTime);
            writer.WriteLine("<th>{0}</th>", Resources.Strings.RateTime);
            writer.WriteLine("<th>{0}</th>", Resources.Strings.DiffTime);
            writer.WriteLine("</tr>");

            // Создать строки данных.
            for (var i = 0; i < periodOfficeStatistics.UserStatistics.Length; i++)
            {
                var statisticCurrentUser = periodOfficeStatistics.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");

                var domainValue = String.Empty;
                foreach (var domain in statisticCurrentUser.User.DomainNames)
                {
                    var names = domain.Split('\\');
                    var name = (names.Length == 1)
                                      ? names[0]
                                      : names[names.Length - 1];

                    domainValue += String.IsNullOrEmpty(domainValue)
                                          ? name
                                          : ", " + name;
                }
                writer.WriteLine("<td style=\"position: absolute;  border: 1px solid grey; white-space: nowrap; background: #FF7300; width: 30%; left: 0;\">{0} ({1}) </td>", statisticCurrentUser.User.FullName, domainValue);

                foreach (var dayWorkTime in statisticCurrentUser.DayWorkTimes)
                {
                    WriteDataTime(writer, dayWorkTime, "td", true, statisticCurrentUser.User.ID);
                }

               writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(statisticCurrentUser.TotalWorkTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(statisticCurrentUser.RateTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(statisticCurrentUser.RateTime - statisticCurrentUser.TotalWorkTime));

                writer.WriteLine("</tr>");
            }

            writer.Indent--;
            writer.WriteLine("</table>");
            writer.WriteLine("</div>");
        }

        #region Методы

        /// <summary>
        /// Write date to container with style.
        /// </summary>
        /// <param name="writer">HtmlTextWriter.</param>
        /// <param name="dayWorkTime">Date.</param>
        /// <param name="container">HtmlContainer.</param>
        /// <param name="fWriteTime">Write date(false) or time(true).</param>
        /// <param name="userId">ID of user.</param>
        private void WriteDataTime(HtmlTextWriter writer,
            DayWorkTime dayWorkTime,
            String container,
            bool fWriteTime,
            int? userId)
        {
            var calItem = new CalendarItem(dayWorkTime);
            var strValue = fWriteTime
                                  ? DateTimePresenter.GetTime(dayWorkTime.WorkTime)
                                  : dayWorkTime.Date.ToString("dd/MM");

            var cellColor = new Color();
            if (userId != null
                 && dayWorkTime.WorkTime == TimeSpan.Zero
                 && !calItem.IsWeekend)
            {
                WorkEvent workEvent = WorkEvent.GetCurrentEventOfDate((int)userId, dayWorkTime.Date);

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

            if (calItem.IsWeekend)
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
