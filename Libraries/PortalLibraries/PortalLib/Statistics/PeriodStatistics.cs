using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Properties;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.UserFilters;
using ConfirmIt.PortalLib;

namespace UlterSystems.PortalLib.Statistics
{
    /// <summary>
    /// ����� ���������� ������������ �� ������������ ������.
    /// </summary>
    public class PeriodUserStatistics
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_TotalTime = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_WorkTime = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_RestTime = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_Rate = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Person m_User = null;

        private readonly List<DayUserStatistics> m_DayStatistics = new List<DayUserStatistics>();

        #endregion

        #region Properties

        /// <summary>
        /// ������������, � �������� ��������� ����������.
        /// </summary>
        public Person User
        {
            [DebuggerStepThrough]
            get { return m_User; }
            [DebuggerStepThrough]
            set { m_User = value; }
        }

        /// <summary>
        /// ����� �����, ����������� ������������� � �����.
        /// </summary>
        public TimeSpan TotalTime
        {
            [DebuggerStepThrough]
            get { return m_TotalTime; }
            [DebuggerStepThrough]
            set { m_TotalTime = value; }
        }

        /// <summary>
        /// ������� �����, ����������� ������������� � �����.
        /// </summary>
        public TimeSpan WorkTime
        {
            [DebuggerStepThrough]
            get { return m_WorkTime; }
            [DebuggerStepThrough]
            set { m_WorkTime = value; }
        }

        /// <summary>
        /// ������ �������.
        /// </summary>
        public TimeSpan TimeRate
        {
            [DebuggerStepThrough]
            get { return m_Rate; }
            [DebuggerStepThrough]
            set { m_Rate = value; }
        }

        /// <summary>
        /// �����, ������� ��� ����� ����������.
        /// </summary>
        public TimeSpan RestTime
        {
            [DebuggerStepThrough]
            get { return m_RestTime; }
            [DebuggerStepThrough]
            set { m_RestTime = value; }
        }

        /// <summary>
        /// ���������� �� ����.
        /// </summary>
        public DayUserStatistics[] DaysStatistics
        {
            [DebuggerNonUserCode]
            get
            {
                Debug.Assert(m_DayStatistics != null);
                return m_DayStatistics.ToArray();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// ��������� ���������� �� ���� � ������ ���������.
        /// </summary>
        /// <param name="stat">���������� �� ����.</param>
        public void AddDayStatistics(DayUserStatistics stat)
        {
            if (stat == null)
                return;

            Debug.Assert(m_DayStatistics != null);
            m_DayStatistics.Add(stat);
        }

        /// <summary>
        /// ���������� ���������� �� ������������ �� ������ ������.
        /// </summary>
        /// <param name="user">������������, ���������� �������� ������ ���� ��������.</param>
        /// <param name="begin">������ ������� ����������.</param>
        /// <param name="end">��������� ������� ����������.</param>
        /// <returns>���������� �� ������������ �� ������ ������.</returns>
        public static PeriodUserStatistics GetUserStatistics(Person user, DateTime begin, DateTime end)
        {
            if (user == null)
                throw new ArgumentNullException("user", Resources.UserIsNotDefined);
            if (user.ID == null)
                throw new Exception(Resources.UserIsNotDefined);

            PeriodUserStatistics stat = new PeriodUserStatistics();
            stat.User = user;

            // �������� ������� ��������� ���.
            begin = begin.Date;
            end = end.Date.AddSeconds(1);

            UserTimeCalculator timeCalc = new UserTimeCalculator(user.ID.Value);

            stat.TotalTime = timeCalc.GetMainWorkTime(begin, end);
            stat.WorkTime = timeCalc.GetWorkedTimeWithoutLunch(begin, end);

            while (begin < end)
            {
                CreateDayStatistics(stat, begin);
                stat.TimeRate += timeCalc.GetRateWithoutLunch(begin);
                begin = begin.AddDays(1);
            }

            stat.RestTime = stat.TimeRate - stat.WorkTime;

            return stat;
        }

        /// <summary>
        /// ������� ���������� �� ����.
        /// </summary>
        /// <param name="stat">���������� ������������.</param>
        /// <param name="date">����, �� ������� ���������� ����������.</param>
        private static void CreateDayStatistics(PeriodUserStatistics stat, DateTime date)
        {
            if (stat == null)
                return;

            UserTimeCalculator timeCalc = new UserTimeCalculator(stat.User.ID.Value);

            // ������� ������ ���������� �� ����.
            DayUserStatistics dStat = new DayUserStatistics();

            // ���������� ����.
            dStat.Date = date;

            // �������� ������ � ���������� ������������.
            stat.AddDayStatistics(dStat);

            dStat.TotalTime = timeCalc.GetMainWorkTime(date);
            dStat.WorkTime = timeCalc.GetWorkedTimeWithoutLunch(date);

            // �������� ������� �����.
            DateTime now = DateTime.Now;

            // �������� ��� ������� ������������ �� ����.
            WorkEvent[] events = WorkEvent.GetEventsOfDate(stat.User.ID.Value, date);
            if (events == null || events.Length == 0)
                return;

            // ���������� ��� ������� �� ����.
            foreach (WorkEvent curEvent in events)
            {
                switch (curEvent.EventType)
                {
                    case WorkEventType.BusinessTrip:
                    case WorkEventType.Vacation:
                    case WorkEventType.Ill:
                    case WorkEventType.TrustIll:
                        dStat.AbsenceReason = curEvent.EventType;
                        break;

                    case WorkEventType.MainWork:
                        dStat.IsWorked = true;
                        dStat.BeginTime = curEvent.BeginTime;

                        if (date.Date == DateTime.Today)
                        {
                            dStat.EndTime = curEvent.IsOpen
                                                ? now
                                                : curEvent.EndTime;
                        }
                        else
                            dStat.EndTime = curEvent.EndTime;
                        break;

                    case WorkEventType.TimeOff:
                        if (date.Date == DateTime.Today)
                        {
                            dStat.TimeOffTime += curEvent.IsOpen
                                                     ? now - curEvent.BeginTime
                                                     : curEvent.Duration;
                        }
                        else
                            dStat.TimeOffTime += curEvent.Duration;
                        break;
                }
            }
        }

        /// <summary>
        /// ���������� ����������� ���� � ����������.
        /// </summary>
        /// <returns>����������� ���� � ����������.</returns>
        public DateTime? GetMinDate()
        {
            Debug.Assert(m_DayStatistics != null);
            if (m_DayStatistics.Count == 0)
                return null;

            DateTime minDate = m_DayStatistics[0].Date;

            for (int i = 1; i < m_DayStatistics.Count; i++)
            {
                if (minDate > m_DayStatistics[i].Date)
                    minDate = m_DayStatistics[i].Date;
            }

            return minDate;
        }

        /// <summary>
        /// ���������� ������������ ���� � ����������.
        /// </summary>
        /// <returns>������������ ���� � ����������.</returns>
        public DateTime? GetMaxDate()
        {
            Debug.Assert(m_DayStatistics != null);
            if (m_DayStatistics.Count == 0)
                return null;

            DateTime maxDate = m_DayStatistics[0].Date;

            for (int i = 1; i < m_DayStatistics.Count; i++)
            {
                if (maxDate < m_DayStatistics[i].Date)
                    maxDate = m_DayStatistics[i].Date;
            }

            return maxDate;
        }

        #endregion

        #region ������ ��� XML-�������������

        /// <summary>
        /// ���������� XML-������������� ����������.
        /// </summary>
        /// <returns>XML-������������� ����������.</returns>
        public string GetXMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(strWriter))
                {
                    writer.WriteStartDocument();

                    // ������� root-�������.
                    writer.WriteStartElement("UserStatistics");

                    // ������� �������, ����������� ������������.
                    writer.WriteStartElement("User");
                    writer.WriteString(User.FullName);
                    writer.WriteEndElement(); // User

                    // ������� �������, ����������� ����������� ����.
                    DateTime? minDate = GetMinDate();
                    if (minDate != null)
                    {
                        writer.WriteStartElement("MinDate");
                        writer.WriteString(minDate.Value.ToString("dd/MM/yyyy"));
                        writer.WriteEndElement(); // MinDate
                    }

                    // ������� �������, ����������� ������������ ����.
                    DateTime? maxDate = GetMinDate();
                    if (maxDate != null)
                    {
                        writer.WriteStartElement("MaxDate");
                        writer.WriteString(maxDate.Value.ToString("dd/MM/yyyy"));
                        writer.WriteEndElement(); // MaxDate
                    }

                    // ������� �������, ����������� ����� �����.
                    writer.WriteStartElement("TotalTime");
                    writer.WriteString(DateTimePresenter.GetTime(TotalTime));
                    writer.WriteEndElement(); // TotalTime

                    // ������� �������, ����������� ������� �����.
                    writer.WriteStartElement("WorkTime");
                    writer.WriteString(DateTimePresenter.GetTime(WorkTime));
                    writer.WriteEndElement(); // WorkTime

                    // �������� ���������� �� ��������� ����.
                    foreach (DayUserStatistics dStat in DaysStatistics)
                    {
                        WriteDayStatistics(writer, dStat);
                    }

                    writer.WriteEndElement(); // UserStatistics
                    writer.WriteEndDocument();
                }

                return strWriter.ToString();
            }
        }

        /// <summary>
        /// ���������� � XML ���������� � ���������� �� ����.
        /// </summary>
        /// <param name="writer">XML-��������.</param>
        /// <param name="dStat">������ ���������� �� ����.</param>
        private void WriteDayStatistics(XmlWriter writer, DayUserStatistics dStat)
        {
            if (writer == null || dStat == null)
                return;

            writer.WriteStartElement("DayStatistics");

            // ������� ������� ����.
            writer.WriteStartElement("Date");
            writer.WriteString(dStat.Date.ToString("dd/MM/yyyy"));
            writer.WriteEndElement(); // Date

            // ������� ������� ������� ����������.
            if (dStat.AbsenceReason != null)
            {
                writer.WriteStartElement("AbsenceReason");
                writer.WriteString(dStat.AbsenceReason.Value.ToString());
                writer.WriteEndElement(); // AbsenceReason
            }

            // ���� ���� ������� �������...
            if (dStat.IsWorked)
            {
                // ������� ������� �������� �������.
                writer.WriteStartElement("Work");

                // ������� ������� ������ ������.
                writer.WriteStartElement("BeginTime");
                writer.WriteString(dStat.BeginTime.ToString("HH:mm:ss"));
                writer.WriteEndElement(); // BeginTime

                // ������� ������� ��������� ������.
                writer.WriteStartElement("EndTime");
                writer.WriteString(dStat.EndTime.ToString("HH:mm:ss"));
                writer.WriteEndElement(); // EndTime

                // ������� ������� ������������ ���������� � ������.
                writer.WriteStartElement("TotalTime");
                writer.WriteString(DateTimePresenter.GetTime(dStat.TotalTime));
                writer.WriteEndElement(); // TotalTime

                // ������� ������� ������������ ������ � ������.
                writer.WriteStartElement("WorkTime");
                writer.WriteString(DateTimePresenter.GetTime(dStat.WorkTime));
                writer.WriteEndElement(); // WorkTime

                writer.WriteEndElement(); // Work

                // ������� ������� ���������� �������.
                if (dStat.TimeOffTime > TimeSpan.Zero)
                {
                    writer.WriteStartElement("TimeOffTime");
                    writer.WriteString(DateTimePresenter.GetTime(dStat.TimeOffTime));
                    writer.WriteEndElement(); // TimeOffTime
                }
            }

            writer.WriteEndElement(); // DayStatistics
        }

        #endregion

        #region ������ ��� HTML-�������������

        /// <summary>
        /// ���������� HTML-������������� ����������.
        /// </summary>
        /// <returns>HTML-������������� ����������.</returns>
        public string GetHTMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(strWriter))
                {
                    writer.WriteLine("<html>");
                    writer.Indent++;

                    // ���������.
                    writer.Write("<title>");
                    writer.Write(Resources.UserStatisticsTitle, User.FullName, GetMinDate().Value.ToString("dd/MM/yyyy"),
                                 GetMaxDate().Value.ToString("dd/MM/yyyy"));
                    writer.WriteLine("</title>");

                    // ����.
                    writer.WriteLine("<body>");
                    writer.Indent++;

                    // �������� �������.
                    writer.WriteLine("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"2\">");
                    writer.Indent++;

                    // ��� ����������.
                    writer.WriteLine("<tr>");
                    writer.Indent++;

                    writer.WriteLine("<th style=\"width:15%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.DateTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th style=\"width:17%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.EventTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th style=\"width:17%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.BeginTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th style=\"width:17%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.EndTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th style=\"width:17%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.TotalTimeTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th style=\"width:17%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.WorkTimeTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // �������� ������ ������ �� ����.
                    foreach (DayUserStatistics dStat in DaysStatistics)
                    {
                        writer.WriteLine("<tr>");
                        writer.Indent++;

                        // ������ � �����.
                        writer.WriteLine("<td style=\"width:15%\">");
                        writer.Indent++;
                        writer.WriteLine(dStat.Date.ToString("dd/MM/yyyy"));
                        writer.Indent--;
                        writer.WriteLine("</td>");

                        // ������ � �������.
                        writer.WriteLine("<td colspan=\"5\">");
                        writer.Indent++;
                        InsertDayStatisticsHTMLPresentation(writer, dStat);
                        writer.Indent--;
                        writer.WriteLine("</td>");

                        writer.Indent--;
                        writer.WriteLine("</tr>");

                    }

                    // �������� ������ ������ �������.
                    writer.WriteLine("<tr>");
                    writer.Indent++;
                    writer.WriteLine("<td colspan=\"6\" align=\"right\" style=\"font-weight:bold\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.TotalTime, DateTimePresenter.GetTime(TotalTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");
                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // �������� ������ �������� �������.
                    writer.WriteLine("<tr>");
                    writer.Indent++;
                    writer.WriteLine("<td colspan=\"6\" align=\"right\" style=\"font-weight:bold\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.WorkTime, DateTimePresenter.GetTime(WorkTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");
                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // �������� ������ ����������� �������.
                    writer.WriteLine("<tr>");
                    writer.Indent++;
                    writer.WriteLine("<td colspan=\"6\" align=\"right\" style=\"font-weight:bold\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.RestTime, DateTimePresenter.GetTime(RestTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");
                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    writer.Indent--;
                    writer.WriteLine("</table>");

                    writer.Indent--;
                    writer.WriteLine("</body>");

                    writer.Indent--;
                    writer.WriteLine("</html>");
                }

                return strWriter.ToString();
            }
        }

        /// <summary>
        /// ������� HTML-������������� ���������� ������������ �� ����.
        /// </summary>
        /// <param name="writer">��������.</param>
        /// <param name="dStat">���������� ������������ �� ����.</param>
        private void InsertDayStatisticsHTMLPresentation(IndentedTextWriter writer, DayUserStatistics dStat)
        {
            if (writer == null || dStat == null)
                return;

            writer.WriteLine("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            writer.Indent++;

            if (dStat.AbsenceReason != null)
            {
                writer.WriteLine("<tr>");
                writer.Indent++;

                writer.WriteLine("<td colspan=\"5\" align=\"center\">");
                writer.Indent++;
                UptimeEventType uet = UptimeEventType.GetEventType((int)dStat.AbsenceReason.Value);
                writer.WriteLine(uet.Name);
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.Indent--;
                writer.WriteLine("</tr>");
            }

            if (dStat.IsWorked)
            {
                writer.WriteLine("<tr>");
                writer.Indent++;

                writer.WriteLine("<td style=\"width:20%\">");
                writer.Indent++;
                writer.WriteLine(Resources.Work);
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.WriteLine("<td style=\"width:20%\">");
                writer.Indent++;
                writer.WriteLine(dStat.BeginTime.ToString("HH:mm"));
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.WriteLine("<td style=\"width:20%\">");
                writer.Indent++;
                writer.WriteLine(dStat.EndTime.ToString("HH:mm"));
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.WriteLine("<td style=\"width:20%\">");
                writer.Indent++;
                writer.WriteLine(DateTimePresenter.GetTime(dStat.TotalTime));
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.WriteLine("<td style=\"width:20%\">");
                writer.Indent++;
                writer.WriteLine(DateTimePresenter.GetTime(dStat.WorkTime));
                writer.Indent--;
                writer.WriteLine("</td>");

                writer.Indent--;
                writer.WriteLine("</tr>");
                
                if (dStat.TimeOffTime > TimeSpan.Zero)
                {
                    writer.WriteLine("<tr>");
                    writer.Indent++;

                    writer.WriteLine("<td style=\"width:20%\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.TimeOff);
                    writer.Indent--;
                    writer.WriteLine("</td>");

                    writer.WriteLine("<td style=\"width:20%\">");
                    writer.Indent++;
                    writer.Indent--;
                    writer.WriteLine("</td>");

                    writer.WriteLine("<td style=\"width:20%\">");
                    writer.Indent++;
                    writer.Indent--;
                    writer.WriteLine("</td>");

                    writer.WriteLine("<td style=\"width:20%\">");
                    writer.Indent++;
                    writer.WriteLine(DateTimePresenter.GetTime(dStat.TimeOffTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");

                    writer.WriteLine("<td style=\"width:20%\">");
                    writer.Indent++;
                    writer.Indent--;
                    writer.WriteLine("</td>");

                    writer.Indent--;
                    writer.WriteLine("</tr>");
                }
            }

            writer.Indent--;
            writer.WriteLine("</table>");
        }

        #endregion
    }

    /// <summary>
    /// ����� ���������� �� ����� �� ������������ ������.
    /// </summary>
    public class PeriodOfficeStatistics
    {
        #region ����

        private readonly List<UserOfficeStatistics> m_Statistics = new List<UserOfficeStatistics>();
        private IUsersFilter m_Filter = null;

        #endregion

        #region ��������

        /// <summary>
        /// ���������� �������������.
        /// </summary>
        public UserOfficeStatistics[] UserStatistics
        {
            get
            {
                Debug.Assert(m_Statistics != null);
                return m_Statistics.ToArray();
            }
        }

        /// <summary>
        /// ������ �������������.
        /// </summary>
        public IUsersFilter UsersFilter
        {
            get { return m_Filter; }
            set { m_Filter = value; }
        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ���������� ������������.
        /// </summary>
        /// <param name="stat">���������� ������������.</param>
        public void AddUserStatistics(UserOfficeStatistics stat)
        {
            if (stat == null)
                return;

            Debug.Assert(m_Statistics != null);
            m_Statistics.Add(stat);
        }

        /// <summary>
        /// ���������� ����������� ���� � ����������.
        /// </summary>
        /// <returns>����������� ���� � ����������.</returns>
        public DateTime? GetMinDate()
        {
            Debug.Assert(m_Statistics != null);
            if (m_Statistics.Count == 0)
                return null;

            UserOfficeStatistics uStat = m_Statistics[0];
            if (uStat.DayWorkTimes == null || uStat.DayWorkTimes.Length == 0)
                return null;

            DateTime minDate = uStat.DayWorkTimes[0].Date;
            for (int i = 1; i < uStat.DayWorkTimes.Length; i++)
            {
                if (minDate > uStat.DayWorkTimes[i].Date)
                    minDate = uStat.DayWorkTimes[i].Date;
            }

            return minDate;
        }

        /// <summary>
        /// ���������� ������������ ���� � ����������.
        /// </summary>
        /// <returns>������������ ���� � ����������.</returns>
        public DateTime? GetMaxDate()
        {
            Debug.Assert(m_Statistics != null);
            if (m_Statistics.Count == 0)
                return null;

            UserOfficeStatistics uStat = m_Statistics[0];
            if (uStat.DayWorkTimes == null || uStat.DayWorkTimes.Length == 0)
                return null;

            DateTime maxDate = uStat.DayWorkTimes[0].Date;
            for (int i = 1; i < uStat.DayWorkTimes.Length; i++)
            {
                if (maxDate < uStat.DayWorkTimes[i].Date)
                    maxDate = uStat.DayWorkTimes[i].Date;
            }

            return maxDate;
        }

        /// <summary>
        /// ���������� ���������� �� ����� �� ������ ������.
        /// </summary>
        /// <param name="begin">������ ������� ����������.</param>
        /// <param name="end">��������� ������� ����������.</param>
        /// <returns>���������� �� ����� �� ������ ������.</returns>
        public static PeriodOfficeStatistics GetOfficeStatistics(DateTime begin, DateTime end)
        {
            PeriodOfficeStatistics stat = new PeriodOfficeStatistics();

            // ������� ������.
            IUsersFilter filter = new YaroslavlOfficeUsersFilter(null);
            stat.UsersFilter = filter;

            // ���������� ���� �������������.
            foreach (Person curUser in UserList.GetEmployeeList())
            {
                // ������������� �������������.
                if (stat.UsersFilter != null)
                    if (!stat.UsersFilter.IsValid(curUser))
                        continue;

                UserOfficeStatistics uos = GetUserOfficeStatistics(curUser, begin, end);
                if (uos != null)
                    stat.AddUserStatistics(uos);
            }

            return stat;
        }

        /// <summary>
        /// ������� ���������� ������������ �� ����� �� ��������� ������.
        /// </summary>
        /// <param name="user">������������.</param>
        /// <param name="begin">������ ������� ����������.</param>
        /// <param name="end">��������� ������� ����������.</param>
        /// <returns>���������� ������������ �� ����� �� ��������� ������.</returns>
        private static UserOfficeStatistics GetUserOfficeStatistics(Person user, DateTime begin, DateTime end)
        {
            if (user == null || user.ID == null)
                return null;

            UserOfficeStatistics stat = new UserOfficeStatistics();
            stat.User = user;

            // ������� ����������� ������.
            UserTimeCalculator timeCalc = new UserTimeCalculator(user.ID.Value);
            // ������� ������ ������� ������������.

            DateTime date = begin.Date;
            end = end.Date.AddSeconds(1);

            while (date < end)
            {
                DayWorkTime dwt = new DayWorkTime();
                dwt.Date = date;
                dwt.WorkTime = timeCalc.GetWorkedTimeWithoutLunch(date);

                stat.AddDayWorkTime(dwt);

                stat.TotalWorkTime += dwt.WorkTime;
                stat.RateTime += timeCalc.GetRateWithoutLunch(date);

                date = date.AddDays(1);
            }

            return stat;
        }

        #endregion

        #region ������ ��� XML-�������������

        /// <summary>
        /// ���������� XML-������������� ����������.
        /// </summary>
        /// <returns>XML-������������� ����������.</returns>
        public string GetXMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(strWriter))
                {
                    writer.WriteStartDocument();

                    // ������� root-�������.
                    writer.WriteStartElement("OfficeStatistics");

                    foreach (UserOfficeStatistics userStat in this.UserStatistics)
                    {
                        writer.WriteStartElement("User");

                        // ������� ������� � ������ ������������.
                        writer.WriteStartElement("UserName");
                        writer.WriteString(userStat.User.FullName);
                        writer.WriteEndElement(); // UserName

                        // ������� ������� ������ �������� �������.
                        writer.WriteStartElement("TotalWorkTime");
                        writer.WriteString(DateTimePresenter.GetTime(userStat.TotalWorkTime));
                        writer.WriteEndElement(); // TotalWorkTime

                        // ������� ������� ������������ � ��������� �������.
                        writer.WriteStartElement("RateTime");
                        writer.WriteString(DateTimePresenter.GetTime(userStat.RateTime));
                        writer.WriteEndElement(); // RateTime

                        // ���������� ��� ������.
                        foreach (DayWorkTime dwt in userStat.DayWorkTimes)
                        {
                            writer.WriteStartElement("DayWorkTime");

                            // ������� ������� ����.
                            writer.WriteStartElement("Date");
                            writer.WriteString(dwt.Date.ToString("dd/MM/yyyy"));
                            writer.WriteEndElement(); // Date

                            // ������� ������� ������������� �������.
                            writer.WriteStartElement("WorkTime");
                            writer.WriteString(DateTimePresenter.GetTime(dwt.WorkTime));
                            writer.WriteEndElement(); // WorkTime

                            writer.WriteEndElement(); // DayWorkTime
                        }

                        writer.WriteEndElement(); // User
                    }

                    writer.WriteEndElement(); // OfficeStatistics
                    writer.WriteEndDocument();
                }

                return strWriter.ToString();
            }
        }

        #endregion

        #region ������ ��� HTML-�������������

        /// <summary>
        /// ���������� HTML-������������� ����������.
        /// </summary>
        /// <returns>HTML-������������� ����������.</returns>
        public string GetHTMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(strWriter))
                {
                    writer.WriteLine("<html>");
                    writer.Indent++;

                    // ���������.
                    writer.Write("<title>");
                    writer.Write(Resources.OfficeStatisticsTitle, GetMinDate().Value.ToString("dd/MM/yyyy"),
                                 GetMaxDate().Value.ToString("dd/MM/yyyy"));
                    writer.WriteLine("</title>");

                    // ����.
                    writer.WriteLine("<body>");
                    writer.Indent++;

                    // �������� �������.
                    writer.WriteLine("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"2\">");
                    writer.Indent++;

                    // ��� ����������.
                    writer.WriteLine("<tr>");
                    writer.Indent++;

                    writer.WriteLine("<th>");
                    writer.Indent++;
                    writer.WriteLine(Resources.UserNameTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    // ������� ��������� � ������.
                    if (UserStatistics != null && UserStatistics.Length != 0)
                    {
                        if (UserStatistics[0].DayWorkTimes != null)
                        {
                            foreach (DayWorkTime dwt in UserStatistics[0].DayWorkTimes)
                            {
                                writer.WriteLine("<th>");
                                writer.Indent++;
                                writer.WriteLine(dwt.Date.ToString("dd/MM"));
                                writer.Indent--;
                                writer.WriteLine("</th>");
                            }
                        }
                    }

                    writer.WriteLine("<th>");
                    writer.Indent++;
                    writer.WriteLine(Resources.TotalTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th>");
                    writer.Indent++;
                    writer.WriteLine(Resources.RateTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.WriteLine("<th>");
                    writer.Indent++;
                    writer.WriteLine(Resources.DiffTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // �������� ������ ������ �� �������������.
                    if (UserStatistics != null)
                    {
                        foreach (UserOfficeStatistics uStat in UserStatistics)
                        {
                            writer.WriteLine("<tr>");
                            writer.Indent++;

                            // ������ � ������ ������������.
                            writer.WriteLine("<td style=\"white-space:nowrap\">");
                            writer.Indent++;
                            writer.WriteLine(uStat.User.FullName);
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // �������� ������ � ������� �� ����.
                            if (uStat.DayWorkTimes != null)
                            {
                                foreach (DayWorkTime dwt in uStat.DayWorkTimes)
                                {
                                    writer.WriteLine("<td>");
                                    writer.Indent++;
                                    writer.WriteLine(DateTimePresenter.GetTime(dwt.WorkTime));
                                    writer.Indent--;
                                    writer.WriteLine("</td>");
                                }
                            }

                            // ������ � ������� ��������.
                            writer.WriteLine("<td>");
                            writer.Indent++;
                            writer.WriteLine(DateTimePresenter.GetTime(uStat.TotalWorkTime));
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // ������ � ������ �������.
                            writer.WriteLine("<td>");
                            writer.Indent++;
                            writer.WriteLine(DateTimePresenter.GetTime(uStat.RateTime));
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // ������ � ��������� ������.
                            writer.WriteLine("<td>");
                            writer.Indent++;
                            writer.WriteLine(DateTimePresenter.GetTime(uStat.RateTime - uStat.TotalWorkTime));
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            writer.Indent--;
                            writer.WriteLine("</tr>");

                        }
                    }

                    writer.Indent--;
                    writer.WriteLine("</table>");

                    writer.Indent--;
                    writer.WriteLine("</body>");

                    writer.Indent--;
                    writer.WriteLine("</html>");
                }

                return strWriter.ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// ����� ���������� ��� ������������ �� ������������ ������.
    /// ������������ � ��������� ���������� �� �����.
    /// </summary>
    public class UserOfficeStatistics : IComparable<UserOfficeStatistics>
    {
        #region ����

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Person m_User;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_Total = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_Rate = TimeSpan.Zero;

        private readonly List<DayWorkTime> m_Statistics = new List<DayWorkTime>();

        #endregion

        #region ��������

        /// <summary>
        /// ������������.
        /// </summary>
        public Person User
        {
            [DebuggerStepThrough]
            get { return m_User; }
            [DebuggerStepThrough]
            set { m_User = value; }
        }

        /// <summary>
        /// ������������ ����� �� ����.
        /// </summary>
        public DayWorkTime[] DayWorkTimes
        {
            [DebuggerNonUserCode]
            get
            {
                Debug.Assert(m_Statistics != null);
                return m_Statistics.ToArray();
            }
        }

        /// <summary>
        /// ����� ������� �����.
        /// </summary>
        public TimeSpan TotalWorkTime
        {
            [DebuggerStepThrough]
            get { return m_Total; }
            [DebuggerStepThrough]
            set { m_Total = value; }
        }

        /// <summary>
        /// �����, ������� ���������� ����������.
        /// </summary>
        public TimeSpan RateTime
        {
            [DebuggerStepThrough]
            get { return m_Rate; }
            [DebuggerStepThrough]
            set { m_Rate = value; }
        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ������� ����� �� ���� � ����������.
        /// </summary>
        /// <param name="dwt">������� ����� �� ����.</param>
        public void AddDayWorkTime(DayWorkTime dwt)
        {
            Debug.Assert(m_Statistics != null);
            m_Statistics.Add(dwt);
        }

        #endregion

        #region IComparable<UserOfficeStatistics> Members

        public int CompareTo(UserOfficeStatistics other)
        {
            if (User == null)
                return -1;
            if (other.User == null)
                return 1;

            return string.Compare(User.FullName, other.User.FullName);
        }

        #endregion
    }
}
