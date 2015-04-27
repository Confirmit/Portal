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
			PeriodOfficeStatistics stat = PeriodOfficeStatistics.GetOfficeStatistics(BeginDate, EndDate);
            if (stat == null
                || stat.UserStatistics.Length == 0
                || stat.UserStatistics[0].DayWorkTimes.Length == 0)
                return;

			Visible = true;

            writer.WriteLine("<div style='width: 100%; height: 400px; overflow: auto;'>");
			writer.WriteLine("<table border=\"1px\">");
			writer.Indent++;

			// Создать строку заголовков.
            writer.WriteLine("<tr class='gridview-headerrow'>");
			writer.Indent++;

			writer.WriteLine("<th>{0}</th>", Resources.Strings.Employee);

			foreach (DayWorkTime dwt in stat.UserStatistics[0].DayWorkTimes)
			{
                WriteDataTime(writer, dwt, "th", false, null);
			}

			writer.WriteLine("<th>{0}</th>", Resources.Strings.TotalTime);
			writer.WriteLine("<th>{0}</th>", Resources.Strings.RateTime);
			writer.WriteLine("<th>{0}</th>", Resources.Strings.DiffTime);
            writer.WriteLine("<th>{0}</th>", Resources.Strings.DomainName);

			writer.Indent--;
			writer.WriteLine("</tr>");

			// Создать строки данных.
            for (int i = 0; i < stat.UserStatistics.Length; i++ )
            {
                UserOfficeStatistics uos = stat.UserStatistics[i];
                if (i % 2 == 0)
                    writer.WriteLine("<tr class='gridview-row'>");
                else
                    writer.WriteLine("<tr class='gridview-alternatingrow'>");

                writer.Indent++;
                writer.WriteLine("<td class='control-label-bold' style=\"white-space:nowrap\">{0}</td>", uos.User.FullName);

                foreach (DayWorkTime dwt in uos.DayWorkTimes)
                {
                    WriteDataTime(writer, dwt, "td", true, uos.User.ID);
                }

                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(uos.TotalWorkTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(uos.RateTime));
                writer.WriteLine("<td>{0}</td>", DateTimePresenter.GetTime(uos.RateTime - uos.TotalWorkTime));

                String strDomainValue = String.Empty;
                foreach (String domain in uos.User.DomainNames)
                {
                    String[] names = domain.Split('\\');
                    String name = (names.Length == 1)
                                      ? names[0]
                                      : names[names.Length - 1];

                    strDomainValue += String.IsNullOrEmpty(strDomainValue)
                                          ? name
                                          : ", " + name;
                }
                writer.WriteLine("<td>{0}</td>", strDomainValue);

                writer.Indent--;
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
        /// <param name="dwt">Date.</param>
        /// <param name="Container">HtmlContainer.</param>
        /// <param name="fWriteTime">Write date(false) or time(true).</param>
        /// <param name="UserID">ID of user.</param>
        private void WriteDataTime(HtmlTextWriter writer,
            DayWorkTime dwt,
            String Container,
            bool fWriteTime,
            int? UserID)
        {
            CalendarItem calItem = new CalendarItem(dwt);
            String strValue = fWriteTime
                                  ? DateTimePresenter.GetTime(dwt.WorkTime)
                                  : dwt.Date.ToString("dd/MM");

            Color cellColor = new Color();
           if (UserID != null 
                && dwt.WorkTime == TimeSpan.Zero
                && !calItem.IsWeekend)
            {
                WorkEvent workEvent = WorkEvent.GetCurrentEventOfDate((int) UserID, dwt.Date);
                
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
                                 Container,
                                 strValue);
            else
                writer.WriteLine("<{0} style='background-color: {1};' align='center'>{2}</{0}>",
                                 Container,
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
