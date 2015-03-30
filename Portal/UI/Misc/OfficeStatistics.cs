using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.Statistics;

namespace PortalWeb.UI
{
    /// <summary>
    /// ������� ���������� ��� ����������� ���������� �� �����.
    /// </summary>
    public class OfficeStatistics : WebControl
    {
        #region ��������

        /// <summary>
        /// ���� ������ ��������� ������� ����������.
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
        /// ���� ��������� ��������� ������� ����������.
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
        /// ������������ ������� ����������.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            Visible = false;
            if (BeginDate == DateTime.MinValue || EndDate == DateTime.MinValue)
                return;

            // �������� ���������� �� ������ ������.
            var officeStatistics = PeriodOfficeStatistics.GetOfficeStatistics(BeginDate, EndDate);
            if (officeStatistics == null
                || officeStatistics.UserStatistics.Length == 0
                || officeStatistics.UserStatistics[0].DayWorkTimes.Length == 0)
                return;

            Visible = true;
            writer.WriteLine(@"<table>");
            writer.WriteLine(@"<th class='statistic-table-first-th'>");
            writer.WriteLine(@"<div id='updatedTable'>");
            writer.WriteLine(@"<header class='customHeader'>");
            writer.WriteLine(@"<table class='innerTable'>");
            writer.WriteLine(@"<thead>");
            writer.WriteLine(@"<tr>");

            foreach (var dayWorkTime in officeStatistics.UserStatistics[0].DayWorkTimes)
            {
                var calendarItem = new CalendarItem(dayWorkTime);
                var strValue = dayWorkTime.Date.ToString("dd/MM");
                var cellColor = new Color();

                if (calendarItem.IsWeekend)
                    writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>",
                                      strValue);
                else
                    writer.WriteLine("<th class='statistic-table-internal-th' style='background-color: {0};' align='center'>{1}</th>",
                                 cellColor.ToKnownColor(),
                                 strValue);
            }

            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.TotalTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.RateTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.DiffTime);
            writer.WriteLine("<th class='statistic-table-internal-th'>{0}</th>", Resources.Strings.DomainName);

            writer.WriteLine(@"</tr></thead></table></header><div class='firstColumn'><table class='innerTable'><tbody>");

            //��������� ������, �.�. ������� ��������������� ��������� �������, ���������� ������ ��������
            foreach (var userStatistic in officeStatistics.UserStatistics)
            {
                writer.WriteLine("<tr><td class='statistic-table-first-td'>{0}</td></tr>", userStatistic.User.FullName);
            }

            writer.WriteLine(@"</tbody>");
            writer.WriteLine(@"</table>");
            writer.WriteLine(@"</div>");

            writer.WriteLine(@"<div class='customTable'>");
            writer.WriteLine(@"<table class='innerTable'>");
            writer.WriteLine(@"<tbody>");
            // ������� ������ ������.
            for (var i = 0; i < officeStatistics.UserStatistics.Length; i++)
            {
                var userStatistic = officeStatistics.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");


                foreach (var dwt in userStatistic.DayWorkTimes)
                {
                    var calendarItem = new CalendarItem(dwt);
                    var strValue = DateTimePresenter.GetTime(dwt.WorkTime);

                    var cellColor = new Color();
                    if (dwt.WorkTime == TimeSpan.Zero && !calendarItem.IsWeekend)
                    {
                        WorkEvent workEvent = WorkEvent.GetCurrentEventOfDate((int)userStatistic.User.ID, dwt.Date);

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
                        writer.WriteLine("<td class='weekend statistic-table-internal-td'>{0}</td>", strValue);
                    else
                        writer.WriteLine("<td class='statistic-table-internal-td' style='background-color: {0};' align='center'>{1}</td>",
                                         cellColor.ToKnownColor(),
                                         strValue);
                }

                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>", DateTimePresenter.GetTime(userStatistic.TotalWorkTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>", DateTimePresenter.GetTime(userStatistic.RateTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>", DateTimePresenter.GetTime(userStatistic.RateTime - userStatistic.TotalWorkTime));
                writer.WriteLine("<td class='statistic-table-internal-td'>{0}</td>", GetDomainNameByUserStatistic(userStatistic));
                writer.WriteLine("</tr>");
            }

            writer.WriteLine(@"</tbody>");
            writer.WriteLine(@"</table>");
            writer.WriteLine(@"</div>");
            writer.WriteLine(@"</div>");
            writer.WriteLine(@"</table></th>");
            writer.WriteLine(@"<script src='/Scripts/external/jquery-1.6.4.min.js'></script>");
            writer.WriteLine(@"<script src='/Scripts/statistics_table.js'></script>");
            writer.WriteLine(@"<link href='/App_Themes/ConfirmitPortal/css/StatisticsStyle.css' rel='stylesheet' type='text/css'/>");
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

        #region ������

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
        /// ���������� ������� ���������� �������� ����������.
        /// </summary>
        /// <param name="begin">������ ��������� ����������.</param>
        /// <param name="end">����� ��������� ����������.</param>
        public void ShowStatistics(DateTime begin, DateTime end)
        {
            BeginDate = begin;
            EndDate = end;
        }
        #endregion
    }
}
