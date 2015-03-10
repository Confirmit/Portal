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
    /// Класс статистики пользователя за определенный период.
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
        /// Пользователь, к которому относится статистика.
        /// </summary>
        public Person User
        {
            [DebuggerStepThrough]
            get { return m_User; }
            [DebuggerStepThrough]
            set { m_User = value; }
        }

        /// <summary>
        /// Общее время, проведенное пользователем в офисе.
        /// </summary>
        public TimeSpan TotalTime
        {
            [DebuggerStepThrough]
            get { return m_TotalTime; }
            [DebuggerStepThrough]
            set { m_TotalTime = value; }
        }

        /// <summary>
        /// Рабочее время, проведенное пользователем в офисе.
        /// </summary>
        public TimeSpan WorkTime
        {
            [DebuggerStepThrough]
            get { return m_WorkTime; }
            [DebuggerStepThrough]
            set { m_WorkTime = value; }
        }

        /// <summary>
        /// Ставка времени.
        /// </summary>
        public TimeSpan TimeRate
        {
            [DebuggerStepThrough]
            get { return m_Rate; }
            [DebuggerStepThrough]
            set { m_Rate = value; }
        }

        /// <summary>
        /// Время, которое еще нужно отработать.
        /// </summary>
        public TimeSpan RestTime
        {
            [DebuggerStepThrough]
            get { return m_RestTime; }
            [DebuggerStepThrough]
            set { m_RestTime = value; }
        }

        /// <summary>
        /// Статистики по дням.
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
        /// Добавляет статистику за день к списку статистик.
        /// </summary>
        /// <param name="stat">Статистика за день.</param>
        public void AddDayStatistics(DayUserStatistics stat)
        {
            if (stat == null)
                return;

            Debug.Assert(m_DayStatistics != null);
            m_DayStatistics.Add(stat);
        }

        /// <summary>
        /// Возвращает статистику по пользователю за данный период.
        /// </summary>
        /// <param name="user">Пользователь, статистика которого должна быть получена.</param>
        /// <param name="begin">Начало периода статистики.</param>
        /// <param name="end">Окончание периода статистики.</param>
        /// <returns>Статистика по пользователю за данный период.</returns>
        public static PeriodUserStatistics GetUserStatistics(Person user, DateTime begin, DateTime end)
        {
            if (user == null)
                throw new ArgumentNullException("user", Resources.UserIsNotDefined);
            if (user.ID == null)
                throw new Exception(Resources.UserIsNotDefined);

            PeriodUserStatistics stat = new PeriodUserStatistics();
            stat.User = user;

            // Изменить границы диапазона дат.
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
        /// Создает статистику за день.
        /// </summary>
        /// <param name="stat">Статистика пользователя.</param>
        /// <param name="date">Дата, за которую получается статистика.</param>
        private static void CreateDayStatistics(PeriodUserStatistics stat, DateTime date)
        {
            if (stat == null)
                return;

            UserTimeCalculator timeCalc = new UserTimeCalculator(stat.User.ID.Value);

            // Создать объект статистики за день.
            DayUserStatistics dStat = new DayUserStatistics();

            // Установить дату.
            dStat.Date = date;

            // Добавить объект в статистику пользователя.
            stat.AddDayStatistics(dStat);

            dStat.TotalTime = timeCalc.GetMainWorkTime(date);
            dStat.WorkTime = timeCalc.GetWorkedTimeWithoutLunch(date);

            // Получить текущее время.
            DateTime now = DateTime.Now;

            // Получить все события пользователя за дату.
            WorkEvent[] events = WorkEvent.GetEventsOfDate(stat.User.ID.Value, date);
            if (events == null || events.Length == 0)
                return;

            // Обработать все события за день.
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
        /// Возвращает минимальную дату в статистике.
        /// </summary>
        /// <returns>Минимальная дата в статистике.</returns>
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
        /// Возвращает максимальную дату в статистике.
        /// </summary>
        /// <returns>Максимальная дата в статистике.</returns>
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

        #region Методы для XML-представления

        /// <summary>
        /// Возвращает XML-представление статистики.
        /// </summary>
        /// <returns>XML-представление статистики.</returns>
        public string GetXMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(strWriter))
                {
                    writer.WriteStartDocument();

                    // Создать root-элемент.
                    writer.WriteStartElement("UserStatistics");

                    // Создать элемент, описывающий пользователя.
                    writer.WriteStartElement("User");
                    writer.WriteString(User.FullName);
                    writer.WriteEndElement(); // User

                    // Создать элемент, описывающий минимальную дату.
                    DateTime? minDate = GetMinDate();
                    if (minDate != null)
                    {
                        writer.WriteStartElement("MinDate");
                        writer.WriteString(minDate.Value.ToString("dd/MM/yyyy"));
                        writer.WriteEndElement(); // MinDate
                    }

                    // Создать элемент, описывающий максимальную дату.
                    DateTime? maxDate = GetMinDate();
                    if (maxDate != null)
                    {
                        writer.WriteStartElement("MaxDate");
                        writer.WriteString(maxDate.Value.ToString("dd/MM/yyyy"));
                        writer.WriteEndElement(); // MaxDate
                    }

                    // Создать элемент, описывающий общее время.
                    writer.WriteStartElement("TotalTime");
                    writer.WriteString(DateTimePresenter.GetTime(TotalTime));
                    writer.WriteEndElement(); // TotalTime

                    // Создать элемент, описывающий рабочее время.
                    writer.WriteStartElement("WorkTime");
                    writer.WriteString(DateTimePresenter.GetTime(WorkTime));
                    writer.WriteEndElement(); // WorkTime

                    // Добавить информацию об отдельных днях.
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
        /// Записывает в XML информацию о статистике за день.
        /// </summary>
        /// <param name="writer">XML-писатель.</param>
        /// <param name="dStat">Объект статистики за день.</param>
        private void WriteDayStatistics(XmlWriter writer, DayUserStatistics dStat)
        {
            if (writer == null || dStat == null)
                return;

            writer.WriteStartElement("DayStatistics");

            // Создать элемент даты.
            writer.WriteStartElement("Date");
            writer.WriteString(dStat.Date.ToString("dd/MM/yyyy"));
            writer.WriteEndElement(); // Date

            // Создать элемент причины отсутствия.
            if (dStat.AbsenceReason != null)
            {
                writer.WriteStartElement("AbsenceReason");
                writer.WriteString(dStat.AbsenceReason.Value.ToString());
                writer.WriteEndElement(); // AbsenceReason
            }

            // Если есть рабочие события...
            if (dStat.IsWorked)
            {
                // Создать элемент рабочего события.
                writer.WriteStartElement("Work");

                // Создать элемент начала работы.
                writer.WriteStartElement("BeginTime");
                writer.WriteString(dStat.BeginTime.ToString("HH:mm:ss"));
                writer.WriteEndElement(); // BeginTime

                // Создать элемент окончания работы.
                writer.WriteStartElement("EndTime");
                writer.WriteString(dStat.EndTime.ToString("HH:mm:ss"));
                writer.WriteEndElement(); // EndTime

                // Создать элемент длительности пребывания в оффисе.
                writer.WriteStartElement("TotalTime");
                writer.WriteString(DateTimePresenter.GetTime(dStat.TotalTime));
                writer.WriteEndElement(); // TotalTime

                // Создать элемент длительности работы в оффисе.
                writer.WriteStartElement("WorkTime");
                writer.WriteString(DateTimePresenter.GetTime(dStat.WorkTime));
                writer.WriteEndElement(); // WorkTime

                writer.WriteEndElement(); // Work

                // Создать элемент нерабочего времени.
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

        #region Методы для HTML-представления

        /// <summary>
        /// Возвращает HTML-представление статистики.
        /// </summary>
        /// <returns>HTML-представление статистики.</returns>
        public string GetHTMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(strWriter))
                {
                    writer.WriteLine("<html>");
                    writer.Indent++;

                    // Заголовок.
                    writer.Write("<title>");
                    writer.Write(Resources.UserStatisticsTitle, User.FullName, GetMinDate().Value.ToString("dd/MM/yyyy"),
                                 GetMaxDate().Value.ToString("dd/MM/yyyy"));
                    writer.WriteLine("</title>");

                    // Тело.
                    writer.WriteLine("<body>");
                    writer.Indent++;

                    // Основная таблица.
                    writer.WriteLine("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"2\">");
                    writer.Indent++;

                    // Ряд заголовков.
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

                    // Вставить строки данных по дням.
                    foreach (DayUserStatistics dStat in DaysStatistics)
                    {
                        writer.WriteLine("<tr>");
                        writer.Indent++;

                        // Ячейка с датой.
                        writer.WriteLine("<td style=\"width:15%\">");
                        writer.Indent++;
                        writer.WriteLine(dStat.Date.ToString("dd/MM/yyyy"));
                        writer.Indent--;
                        writer.WriteLine("</td>");

                        // Ячейка с данными.
                        writer.WriteLine("<td colspan=\"5\">");
                        writer.Indent++;
                        InsertDayStatisticsHTMLPresentation(writer, dStat);
                        writer.Indent--;
                        writer.WriteLine("</td>");

                        writer.Indent--;
                        writer.WriteLine("</tr>");

                    }

                    // Вставить строку общего времени.
                    writer.WriteLine("<tr>");
                    writer.Indent++;
                    writer.WriteLine("<td colspan=\"6\" align=\"right\" style=\"font-weight:bold\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.TotalTime, DateTimePresenter.GetTime(TotalTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");
                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // Вставить строку рабочего времени.
                    writer.WriteLine("<tr>");
                    writer.Indent++;
                    writer.WriteLine("<td colspan=\"6\" align=\"right\" style=\"font-weight:bold\">");
                    writer.Indent++;
                    writer.WriteLine(Resources.WorkTime, DateTimePresenter.GetTime(WorkTime));
                    writer.Indent--;
                    writer.WriteLine("</td>");
                    writer.Indent--;
                    writer.WriteLine("</tr>");

                    // Вставить строку оставшегося времени.
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
        /// Создает HTML-представление статистики пользователя за день.
        /// </summary>
        /// <param name="writer">Писатель.</param>
        /// <param name="dStat">Статистика пользователя за день.</param>
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
    /// Класс статистики по офису за определенный период.
    /// </summary>
    public class PeriodOfficeStatistics
    {
        #region Поля

        private readonly List<UserOfficeStatistics> m_Statistics = new List<UserOfficeStatistics>();
        private IUsersFilter m_Filter = null;

        #endregion

        #region Свойства

        /// <summary>
        /// Статистики пользователей.
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
        /// Фильтр пользователей.
        /// </summary>
        public IUsersFilter UsersFilter
        {
            get { return m_Filter; }
            set { m_Filter = value; }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавляет статистику пользователя.
        /// </summary>
        /// <param name="stat">Статистика пользователя.</param>
        public void AddUserStatistics(UserOfficeStatistics stat)
        {
            if (stat == null)
                return;

            Debug.Assert(m_Statistics != null);
            m_Statistics.Add(stat);
        }

        /// <summary>
        /// Возвращает минимальную дату в статистике.
        /// </summary>
        /// <returns>Минимальная дата в статистике.</returns>
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
        /// Возвращает максимальную дату в статистике.
        /// </summary>
        /// <returns>Максимальная дата в статистике.</returns>
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
        /// Возвращает статистику по офису за данный период.
        /// </summary>
        /// <param name="begin">Начало периода статистики.</param>
        /// <param name="end">Окончание периода статистики.</param>
        /// <returns>Статистика по офису за данный период.</returns>
        public static PeriodOfficeStatistics GetOfficeStatistics(DateTime begin, DateTime end)
        {
            PeriodOfficeStatistics stat = new PeriodOfficeStatistics();

            // Создать фильтр.
            IUsersFilter filter = new YaroslavlOfficeUsersFilter(null);
            stat.UsersFilter = filter;

            // Обработать всех пользователей.
            foreach (Person curUser in UserList.GetEmployeeList())
            {
                // Отфильтровать пользователей.
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
        /// Создает статистику пользователя по офису за указанный период.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="begin">Начало периода статистики.</param>
        /// <param name="end">Окончание периода статистики.</param>
        /// <returns>Статистика пользователя по офису за указанный период.</returns>
        private static UserOfficeStatistics GetUserOfficeStatistics(Person user, DateTime begin, DateTime end)
        {
            if (user == null || user.ID == null)
                return null;

            UserOfficeStatistics stat = new UserOfficeStatistics();
            stat.User = user;

            // Создать вычислитель времен.
            UserTimeCalculator timeCalc = new UserTimeCalculator(user.ID.Value);
            // Создать объект событий пользователя.

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

        #region Методы для XML-представления

        /// <summary>
        /// Возвращает XML-представление статистики.
        /// </summary>
        /// <returns>XML-представление статистики.</returns>
        public string GetXMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(strWriter))
                {
                    writer.WriteStartDocument();

                    // Создать root-элемент.
                    writer.WriteStartElement("OfficeStatistics");

                    foreach (UserOfficeStatistics userStat in this.UserStatistics)
                    {
                        writer.WriteStartElement("User");

                        // Создать элемент с именем пользователя.
                        writer.WriteStartElement("UserName");
                        writer.WriteString(userStat.User.FullName);
                        writer.WriteEndElement(); // UserName

                        // Создать элемент общего рабочего времени.
                        writer.WriteStartElement("TotalWorkTime");
                        writer.WriteString(DateTimePresenter.GetTime(userStat.TotalWorkTime));
                        writer.WriteEndElement(); // TotalWorkTime

                        // Создать элемент необходимого к отработке времени.
                        writer.WriteStartElement("RateTime");
                        writer.WriteString(DateTimePresenter.GetTime(userStat.RateTime));
                        writer.WriteEndElement(); // RateTime

                        // Обработать дни недели.
                        foreach (DayWorkTime dwt in userStat.DayWorkTimes)
                        {
                            writer.WriteStartElement("DayWorkTime");

                            // Создать элемент даты.
                            writer.WriteStartElement("Date");
                            writer.WriteString(dwt.Date.ToString("dd/MM/yyyy"));
                            writer.WriteEndElement(); // Date

                            // Создать элемент отработанного времени.
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

        #region Методы для HTML-представления

        /// <summary>
        /// Возвращает HTML-представление статистики.
        /// </summary>
        /// <returns>HTML-представление статистики.</returns>
        public string GetHTMLPresentation()
        {
            using (StringWriter strWriter = new StringWriter())
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(strWriter))
                {
                    writer.WriteLine("<html>");
                    writer.Indent++;

                    // Заголовок.
                    writer.Write("<title>");
                    writer.Write(Resources.OfficeStatisticsTitle, GetMinDate().Value.ToString("dd/MM/yyyy"),
                                 GetMaxDate().Value.ToString("dd/MM/yyyy"));
                    writer.WriteLine("</title>");

                    // Тело.
                    writer.WriteLine("<body>");
                    writer.Indent++;

                    // Основная таблица.
                    writer.WriteLine("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"2\">");
                    writer.Indent++;

                    // Ряд заголовков.
                    writer.WriteLine("<tr>");
                    writer.Indent++;

                    writer.WriteLine("<th>");
                    writer.Indent++;
                    writer.WriteLine(Resources.UserNameTitle);
                    writer.Indent--;
                    writer.WriteLine("</th>");

                    // Создать заголовки с датами.
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

                    // Вставить строки данных по пользователям.
                    if (UserStatistics != null)
                    {
                        foreach (UserOfficeStatistics uStat in UserStatistics)
                        {
                            writer.WriteLine("<tr>");
                            writer.Indent++;

                            // Ячейка с именем пользователя.
                            writer.WriteLine("<td style=\"white-space:nowrap\">");
                            writer.Indent++;
                            writer.WriteLine(uStat.User.FullName);
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // Добавить ячейки с данными по дням.
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

                            // Ячейка с рабочим временем.
                            writer.WriteLine("<td>");
                            writer.Indent++;
                            writer.WriteLine(DateTimePresenter.GetTime(uStat.TotalWorkTime));
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // Ячейка с нормой времени.
                            writer.WriteLine("<td>");
                            writer.Indent++;
                            writer.WriteLine(DateTimePresenter.GetTime(uStat.RateTime));
                            writer.Indent--;
                            writer.WriteLine("</td>");

                            // Ячейка с разностью времен.
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
    /// Класс статистики для пользователя за определенный период.
    /// Используется в рассчетах статистики по офису.
    /// </summary>
    public class UserOfficeStatistics : IComparable<UserOfficeStatistics>
    {
        #region Поля

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Person m_User;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_Total = TimeSpan.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_Rate = TimeSpan.Zero;

        private readonly List<DayWorkTime> m_Statistics = new List<DayWorkTime>();

        #endregion

        #region Свойства

        /// <summary>
        /// Пользователь.
        /// </summary>
        public Person User
        {
            [DebuggerStepThrough]
            get { return m_User; }
            [DebuggerStepThrough]
            set { m_User = value; }
        }

        /// <summary>
        /// Отработанное время по дням.
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
        /// Общее рабочее время.
        /// </summary>
        public TimeSpan TotalWorkTime
        {
            [DebuggerStepThrough]
            get { return m_Total; }
            [DebuggerStepThrough]
            set { m_Total = value; }
        }

        /// <summary>
        /// Время, которое необходимо отработать.
        /// </summary>
        public TimeSpan RateTime
        {
            [DebuggerStepThrough]
            get { return m_Rate; }
            [DebuggerStepThrough]
            set { m_Rate = value; }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавляет рабочее время за день в статистику.
        /// </summary>
        /// <param name="dwt">Рабочее время за день.</param>
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
