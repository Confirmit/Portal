using System;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.BAL
{
    /// <summary>
    /// Class for time calculation for user.
    /// </summary>
    public class UserTimeCalculator
    {
        #region Fields
        private readonly int m_UserID;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userID">User ID.</param>
        [DebuggerStepThrough]
        public UserTimeCalculator(int userID)
        {
            m_UserID = userID;
        }
        #endregion

        #region Methods

        #region Absence reason checking

        /// <summary>
        /// Does user have business trip for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True, if user have business trip for given date.</returns>
        public bool HasBusinessTrip(DateTime date)
        {
            foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, date))
            {
                if (workEvent.EventType == WorkEventType.BusinessTrip)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Does user have absence reason for given date (like illness or business trip).
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True, if user have absence reason for given date.</returns>
        public bool HasAbsenceReason(DateTime date)
        {
            foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, date))
            {
                if (WorkEvent.IsAbsenceEvent(workEvent.EventTypeID))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Is given date holiday or does user have absence reason.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>True if given date is holiday or user has absence reason; false, otherwise.</returns>
        private bool IsHolidayOrAbsence(DateTime date)
        {
            if (CalendarItem.GetHoliday(date))
                return true;

            if (HasAbsenceReason(date))
                return true;

            return false;
        }
        #endregion

        #region Lunch time calculation

        /// <summary>
        /// Returns time of lunch events in given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Time of lunch events in given date.</returns>
        public TimeSpan GetLunchEventsTime(DateTime date)
        {
            TimeSpan lunchTime = TimeSpan.Zero;

            foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, date))
            {
                if (workEvent.EventType == WorkEventType.LanchTime)
                    lunchTime += workEvent.Duration;
            }

            return lunchTime;
        }

        #endregion

        #region Main work time calculation

        /// <summary>
        /// Returns full time of work for given date.
        /// It includes lunches and time offs.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Full time of work for given date.</returns>
        public TimeSpan GetMainWorkTime(DateTime date)
        {
            WorkEvent mainWorkEvent = WorkEvent.GetMainWorkEvent(m_UserID, date);
            return (mainWorkEvent == null)
                       ? TimeSpan.Zero
                       : mainWorkEvent.Duration;
        }

        /// <summary>
        /// Returns full time of business trip for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Full time of business trip for given date.</returns>
        public TimeSpan GetBusinessTripTime(DateTime date)
        {
            WorkEvent businessTripEvent = WorkEvent.GetBusinessTripEvent(m_UserID, date);
            return (businessTripEvent == null)
                       ? TimeSpan.Zero
                       : businessTripEvent.Duration;
        }

        /// <summary>
        /// Returns full time of work for given dates range.
        /// It includes lunches and time offs.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="begin">Bagin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Full time of work for given dates range.</returns>
        public TimeSpan GetMainWorkTime(DateTime begin, DateTime end)
        {
            if (begin.Date > end.Date)
                throw new ArgumentException("Begin date can't be greater than end date.");

            TimeSpan totalTime = TimeSpan.Zero;

            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                totalTime += GetMainWorkTime(date);
            }

            return totalTime;
        }

        #endregion

        #region Worked time calculation

        #region Calculation Helpers

        /// <summary>
        /// Calculates work and lunch times for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="workedTime">Work time (duration of presence at work without 
        /// duration time offs and studying). If it is a holiday lunches will be substracted too.</param>
        /// <param name="lunchTime">Lunch time (duration of all lunch events).</param>
        private void CalculateWorkAndLunchTimes(DateTime date,
            out TimeSpan workedTime, out TimeSpan lunchTime)
        {
            workedTime = TimeSpan.Zero;
            lunchTime = TimeSpan.Zero;

            if (date.Date > DateTime.Today)
                return; // TimeSpan.Zero;

            if (IsHolidayOrAbsence(date))
            {
                workedTime = TimeSpan.Zero;

                foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, date))
                {
                    if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
                    {
                        workedTime += workEvent.Duration;
                    }

                    if (WorkEvent.IsBreakEvent(workEvent.EventTypeID))
                        workedTime -= workEvent.Duration;
                }
            }
            else
            {
                workedTime = lunchTime = TimeSpan.Zero;

                foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, date))
                {
                    if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
                    {
                        workedTime += workEvent.Duration;
                    }

                    if (WorkEvent.IsBreakEvent(workEvent.EventTypeID))
                    {
                        if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
                            lunchTime += workEvent.Duration;
                        else
                            workedTime -= workEvent.Duration;
                    }
                }
            }

            if (workedTime < TimeSpan.Zero)
                workedTime = TimeSpan.Zero;
        }

        #endregion

        /// <summary>
        /// Returns worked time for date.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Worked time for date.</returns>
        public TimeSpan GetWorkedTimeWithLunch(DateTime date)
        {
            if (date.Date > DateTime.Today)
                return TimeSpan.Zero;

            TimeSpan workedTime;
            TimeSpan lunchTime;
            CalculateWorkAndLunchTimes(date, out workedTime, out lunchTime);

            // If lunch is greater than MaxLunchTime than it is not lunch but time off.
            // So it must be substracted.
            if (!IsHolidayOrAbsence(date))
            {
                if (lunchTime > Globals.Settings.WorkTime.MaxLunchTime)
                    workedTime -= (lunchTime - Globals.Settings.WorkTime.MaxLunchTime);
            }

            return (workedTime >= TimeSpan.Zero) ? workedTime : TimeSpan.Zero;
        }

        /// <summary>
        /// Returns worked time for date.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Worked time for date.</returns>
        public TimeSpan GetWorkedTimeWithoutLunch(DateTime date)
        {
            if (date.Date > DateTime.Today)
                return TimeSpan.Zero;

            TimeSpan workedTime;
            TimeSpan lunchTime;
            CalculateWorkAndLunchTimes(date, out workedTime, out lunchTime);

            if (!IsHolidayOrAbsence(date))
            {
                TimeSpan lunchNorm = Globals.Settings.WorkTime.MaxLunchTime;
                if (workedTime < Globals.Settings.WorkTime.MaxLunchTime)
                    lunchNorm = TimeSpan.Zero;

                workedTime -= (lunchTime <= lunchNorm) ? lunchNorm : lunchTime;
            }

            return (workedTime >= TimeSpan.Zero) ? workedTime : TimeSpan.Zero;
        }

        /// <summary>
        /// Returns worked time for dates range.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="begin">Bagin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Worked time for dates range.</returns>
        public TimeSpan GetWorkedTimeWithLunch(DateTime begin, DateTime end)
        {
            if (begin.Date > end.Date)
                throw new ArgumentException("Begin date can't be greater than end date.");

            TimeSpan workedTime = TimeSpan.Zero;

            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                workedTime += GetWorkedTimeWithLunch(date);
            }

            return workedTime;
        }

        /// <summary>
        /// Returns worked time for dates range.
        /// It does not take care of holidays, vocations, business trips, etc.
        /// </summary>
        /// <param name="begin">Bagin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Worked time for dates range.</returns>
        public TimeSpan GetWorkedTimeWithoutLunch(DateTime begin, DateTime end)
        {
            if (begin.Date > end.Date)
                throw new ArgumentException("Begin date can't be greater than end date.");

            TimeSpan workedTime = TimeSpan.Zero;

            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                workedTime += GetWorkedTimeWithoutLunch(date);
            }

            return workedTime;
        }

        #endregion

        #region Rest time calculation

        /// <summary>
        /// Returns rest work time for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Rest work time for given date.</returns>
        public TimeSpan GetRestTimeWithLunch(DateTime date)
        {
            if (HasBusinessTrip(date))
            {
                WorkEvent businessTripEvent = WorkEvent.GetBusinessTripEvent(m_UserID, date);
                if (businessTripEvent == null)
                    return CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime;
                else
                {
                    TimeSpan workTime = CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime;
                    TimeSpan workedTime = businessTripEvent.Duration;

                    return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
                }
            }
            else if (IsHolidayOrAbsence(date))
            { return TimeSpan.Zero; }
            else
            {
                TimeSpan workTime = CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime;
                TimeSpan workedTime = GetWorkedTimeWithLunch(date);

                return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
            }
        }

        /// <summary>
        /// Returns rest work time for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Rest work time for given date.</returns>
        public TimeSpan GetRestTimeWithoutLunch(DateTime date)
        {
            if (HasBusinessTrip(date))
            {
                WorkEvent businessTripEvent = WorkEvent.GetBusinessTripEvent(m_UserID, date);
                if (businessTripEvent == null)
                    return CalendarItem.GetWorkTime(date);
                else
                {
                    TimeSpan workTime = CalendarItem.GetWorkTime(date);
                    TimeSpan workedTime = businessTripEvent.Duration - Globals.Settings.WorkTime.MaxLunchTime;

                    return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
                }
            }
            else if (IsHolidayOrAbsence(date))
            { return TimeSpan.Zero; }
            else
            {
                TimeSpan workTime = CalendarItem.GetWorkTime(date);
                TimeSpan workedTime = GetWorkedTimeWithoutLunch(date);

                return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
            }
        }

        /// <summary>
        /// Returns rest work time for given dates range.
        /// </summary>
        /// <param name="begin">Bagin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Rest work time for given dates range.</returns>
        public TimeSpan GetRestTimeWithLunch(DateTime begin, DateTime end)
        {
            TimeSpan workTime = TimeSpan.Zero;
            TimeSpan workedTime = TimeSpan.Zero;

            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (date.Date >= DateTime.Today)
                {
                    workTime += GetRateWithLunch(date);
                    workedTime += GetWorkedTimeWithLunch(date);
                }
                else
                {
                    workTime += GetRateWithoutLunch(date);
                    workedTime += GetWorkedTimeWithoutLunch(date);
                }

                WorkEvent businessTripEvent = WorkEvent.GetBusinessTripEvent(m_UserID, date);
                if (businessTripEvent != null)
                {
                    workedTime += businessTripEvent.Duration;
                }
            }

            return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
        }


        /// <summary>
        /// Returns rest work time for given dates range.
        /// </summary>
        /// <param name="begin">Bagin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Rest work time for given dates range.</returns>
        public TimeSpan GetRestTimeWithoutLunch(DateTime begin, DateTime end)
        {
            TimeSpan workTime = TimeSpan.Zero;
            TimeSpan workedTime = TimeSpan.Zero;

            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                workTime += GetRateWithoutLunch(date);
                workedTime += GetWorkedTimeWithoutLunch(date);

                WorkEvent businessTripEvent = WorkEvent.GetBusinessTripEvent(m_UserID, date);
                if (businessTripEvent != null)
                {
                    workedTime += (businessTripEvent.Duration - Globals.Settings.WorkTime.MaxLunchTime);
                }
            }

            return (workedTime > workTime) ? TimeSpan.Zero : (workTime - workedTime);
        }

        #endregion

        #region Work rate calculation

        /// <summary>
        /// Returns work rate for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Work rate for given date.</returns>
        public TimeSpan GetRateWithLunch(DateTime date)
        {
            if (HasBusinessTrip(date))
            { return CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime - WorkEvent.GetBusinessTripEvent(m_UserID, date).Duration; }
            else if (IsHolidayOrAbsence(date))
            { return TimeSpan.Zero; }
            else
            { return CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime; }
        }

        /// <summary>
        /// Returns work rate for given date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Work rate for given date.</returns>
        public TimeSpan GetRateWithoutLunch(DateTime date)
        {
            if (HasBusinessTrip(date))
            { return CalendarItem.GetWorkTime(date) + Globals.Settings.WorkTime.MaxLunchTime - WorkEvent.GetBusinessTripEvent(m_UserID, date).Duration; }
            else if (IsHolidayOrAbsence(date))
            { return TimeSpan.Zero; }
            else
            { return CalendarItem.GetWorkTime(date); }
        }

        #endregion

        #endregion
    }
}
