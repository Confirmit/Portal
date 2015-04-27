using System;
using System.Collections.Generic;
using System.Diagnostics;

using ConfirmIt.PortalLib.DAL;
using UlterSystems.PortalLib.Statistics;
using UlterSystems.PortalLib;

namespace ConfirmIt.PortalLib.BAL
{
    /// <summary>
    /// Class of calendar item.
    /// </summary>
    public class CalendarItem : ICalendarItem
    {
        #region Fields

        private static readonly Cache<string, CalendarItem> m_Cache = new Cache<string, CalendarItem>(delegate { return Globals.Settings.Calendar.CacheEnabled; }, StringComparer.InvariantCultureIgnoreCase);

        private int m_ID = -1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_Date;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan m_WorkTime = Globals.Settings.WorkTime.DefaultWorkTime;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_Comment;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Date">Day work time.</param>
        public CalendarItem(DayWorkTime date)
        {
            m_Date = date.Date;
            m_WorkTime = date.WorkTime;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CalendarItem()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date
        {
            [DebuggerStepThrough]
            get { return m_Date; }
            [DebuggerStepThrough]
            set { m_Date = value.Date; }
        }

        /// <summary>
        /// Work time for date.
        /// </summary>
        public TimeSpan WorkTime
        {
            [DebuggerStepThrough]
            get { return m_WorkTime; }
            [DebuggerStepThrough]
            set
            {
                if (value < TimeSpan.Zero)
                    m_WorkTime = TimeSpan.Zero;
                else
                    m_WorkTime = value;
            }
        }

        /// <summary>
        /// Comment for date.
        /// </summary>
        public string Comment
        {
            [DebuggerStepThrough]
            get { return m_Comment; }
            [DebuggerStepThrough]
            set { m_Comment = value; }
        }

        /// <summary>
        /// Is item saved in database.
        /// </summary>
        public bool IsSaved
        {
            [DebuggerStepThrough]
            get { return (m_ID != -1); }
        }

        /// <summary>
        /// Is this day holiday.
        /// </summary>
        public bool IsHoliday
        {
            [DebuggerStepThrough]
            get { return ( m_WorkTime == TimeSpan.Zero ); }
        }

        /// <summary>
        /// Is this day holiday.
        /// </summary>
        public bool IsWeekend
        {
            [DebuggerStepThrough]
            get
            {
                if (m_Date.DayOfWeek == DayOfWeek.Saturday
                    || m_Date.DayOfWeek == DayOfWeek.Sunday)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Cache of calendar items.
        /// </summary>
        internal static IDictionary<string, CalendarItem> Cache
        {
            [DebuggerStepThrough]
            get { return m_Cache; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves calendar item into database.
        /// </summary>
        public void Save()
        {
            if (m_ID == -1)
            {
                m_ID = InsertCalendarItem(m_Date, m_WorkTime, m_Comment);
            }
            else
            {
                UpdateCalendarItem(m_ID, m_Date, m_WorkTime, m_Comment);
            }

            string key = GetCacheDateKey(m_Date);
            m_Cache[key] = this;
        }

        /// <summary>
        /// Deletes calendar item from database.
        /// </summary>
        public void Delete()
        {
            if (m_ID != -1)
            {
                DeleteCalendarItem(m_ID);
                m_ID = -1;

                string key = GetCacheDateKey(m_Date);
                if (m_Cache.ContainsKey(key))
                    m_Cache.Remove(key);

            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns calendar item for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Calendar item for given date.</returns>
        public static CalendarItem GetCalendarItem(DateTime date)
        {
            string key = GetCacheDateKey(date);

            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            else
            {
                CalendarDetails details = SiteProvider.Calendar.GetCalendarDetails(date);
                CalendarItem item = new CalendarItem();
                item.Date = date.Date;
                if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                { item.WorkTime = TimeSpan.Zero; }
                else
                { item.WorkTime = Globals.Settings.WorkTime.DefaultWorkTime; }
                if (details != null)
                {
                    item.m_ID = details.ID;
                    item.m_Date = details.Date;
                    item.m_WorkTime = details.WorkTime.TimeOfDay;
                    item.m_Comment = details.Comment;
                }
                Cache[key] = item;
                return item;
            }
        }

        /// <summary>
        /// Deletes item from database.
        /// </summary>
        /// <param name="id">ID of calendar item.</param>
        /// <returns>If record was deleted.</returns>
        public static bool DeleteCalendarItem(int id)
        {
            Cache.Clear();
            return SiteProvider.Calendar.DeleteDetails(id);
        }

        /// <summary>
        /// Inserts item to database.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="workTime">Work time.</param>
        /// <param name="comment">Comment</param>
        /// <returns>ID of inserted record.</returns>
        public static int InsertCalendarItem(DateTime date, TimeSpan workTime, string comment)
        {
            Cache.Remove(GetCacheDateKey(date.Date));

            CalendarDetails details = new CalendarDetails();
            details.Date = date.Date;
            details.WorkTime = date.Date + workTime;
            details.Comment = comment;
            return SiteProvider.Calendar.InsertDetails(details);
        }

        /// <summary>
        /// Inserts item to database.
        /// </summary>
        /// <param name="id">ID of record to update.</param>
        /// <param name="date">Date.</param>
        /// <param name="workTime">Work time.</param>
        /// <param name="comment">Comment</param>
        /// <returns>If record was updated.</returns>
        public static bool UpdateCalendarItem(int id, DateTime date, TimeSpan workTime, string comment)
        {
            Cache.Remove(GetCacheDateKey(date.Date));

            CalendarDetails details = new CalendarDetails();
            details.ID = id;
            details.Date = date.Date;
            details.WorkTime = date.Date + workTime;
            details.Comment = comment;
            return SiteProvider.Calendar.UpdateDetails(details);
        }

        /// <summary>
        /// Returns if given date is holiday.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Is given date holiday.</returns>
        public static bool GetHoliday(DateTime date)
        {
            return GetCalendarItem(date).IsHoliday;
        }

        /// <summary>
        /// Returns work time for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Work time for given date.</returns>
        public static TimeSpan GetWorkTime(DateTime date)
        {
            return GetCalendarItem(date).WorkTime;
        }

        /// <summary>
        /// Returns key for cache.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Key for cache.</returns>
        private static string GetCacheDateKey(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        #endregion
    }

    /// <summary>
    /// Readonly interface for calendar items.
    /// </summary>
    public interface ICalendarItem
    {
        /// <summary>
        /// Date.
        /// </summary>
        DateTime Date { get; }
        /// <summary>
        /// Work time for date.
        /// </summary>
        TimeSpan WorkTime { get; }
        /// <summary>
        /// Comment for date.
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// Is this day holiday.
        /// </summary>
        bool IsHoliday { get; }
    }
}
