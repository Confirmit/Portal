using System;
using System.Linq;
using System.Diagnostics;
using System.Web;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL
{
	/// <summary>
	/// Class for work with user work events.
	/// </summary>
	public class UserWorkEvents
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
		public UserWorkEvents(int userID)
		{
			m_UserID = userID;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns main work event for given date.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <returns>Main work event of given date; null, otherwise.</returns>
		private WorkEvent GetMainWorkEvent( DateTime date )
		{
			return WorkEvent.GetMainWorkEvent( m_UserID, date );
		}

		public static void LogUserWorkEvent(int userId, string eventType, bool isOpenAction)
		{
			var ip = "Unknown";
			if (HttpContext.Current != null)
				ip = HttpContext.Current.Request.UserHostAddress;

			var action = isOpenAction ? "open" : "close";

			var user = Person.GetPersonByID(userId);
			var userFullName = "Cannot found";
			
			if (user != null)
				userFullName = user.FullName;

			Logger.Logger.Instance.
				InfoFormat("UserId {0} ({1}) {2} {3} event type from {4} ip address.",
				           userId, userFullName, action, eventType, ip);
		}

		#region Opening and closing methods

		public void AddLatestClosedWorkEvent(TimeSpan duration, WorkEventType eventType)
		{
			var mainWorkEvent = GetMainWorkEvent(DateTime.Today);

			if (mainWorkEvent == null)
				throw new Exception("There is no main work event.");

			if (mainWorkEvent.BeginTime > DateTime.Now)
				throw new Exception("Can't open work event outside main work event.");

			// Получим все события за сегодня отсортированные по дате создания.
			var workEvents = WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today).
				OrderByDescending(workEvent => workEvent.BeginTime).ToList();

			var startDate = mainWorkEvent.BeginTime;
			var endDate = mainWorkEvent.IsOpen
			                       	? DateTime.Now
			                       	: mainWorkEvent.EndTime;

			foreach (var workEvent in workEvents)
			{
				var isMainWorkEvent = WorkEvent.IsMainWorkEvent(workEvent.EventTypeID);

				if (workEvent.IsOpen && !isMainWorkEvent)
				{
					endDate = workEvent.BeginTime;
					continue;
				}

				startDate = isMainWorkEvent
								? workEvent.BeginTime
								: workEvent.EndTime;

				if (endDate - startDate >= duration)
					break;
				
				endDate = workEvent.BeginTime;
			}

			if (endDate - startDate >= duration)
			    WorkEvent.CreateEvent(endDate - duration, endDate, m_UserID, (int) WorkEventType.TimeOff);
			else
			    throw new Exception("Cannot add closed interval. Not enough time.");
		}

		/// <summary>
		/// Creates absence event for current date.
		/// </summary>
		/// <param name="eventType">Absence event type.</param>
		/// <exception cref="Exception">There already is open main work event.</exception>
		public void CreateAbsenceEvent( WorkEventType eventType )
		{
			CreateAbsenceEvent(eventType, DateTime.Today);
		}

		/// <summary>
		/// Creates absence event for given date.
		/// </summary>
		/// <param name="eventType">Absence event type.</param>
		/// <param name="date">Date of absence.</param>
		/// <exception cref="Exception">There already is open main work event.</exception>
        public void CreateAbsenceEvent(WorkEventType eventType, DateTime date)
		{
		    if (!WorkEvent.IsAbsenceEvent((int) eventType))
		        throw new ArgumentException("Event type is incorrect.");

			LogUserWorkEvent(m_UserID, "Absence", true);

		    date = date.Date.AddHours(12);

		    WorkEvent.CreateEvent(date,
		                          date + Globals.Settings.WorkTime.DefaultWorkTime +
		                          Globals.Settings.WorkTime.MaxLunchTime,
		                          m_UserID, (int) eventType);
		}

	    /// <summary>
		/// Starts main work event for current date.
		/// </summary>
		public void OpenMainWorkEvent()
		{
			var mainWorkEvent = GetMainWorkEvent( DateTime.Today );
			LogUserWorkEvent(m_UserID, "MainWork", true);

            if (mainWorkEvent == null)
            {
                var begTime = DateTime.Now.AddMinutes(-Globals.Settings.GlobalSettings.BonusWorkMinutes);
                WorkEvent.CreateEvent(begTime, begTime,
                                      m_UserID, (int)WorkEventType.MainWork);
            }
            else
            {
                if (mainWorkEvent.IsOpen)
                    throw new Exception("There already is open main work event.");

                var endTime = mainWorkEvent.EndTime;

                WorkEvent.UpdateEvent(
                    mainWorkEvent.ID, mainWorkEvent.Name,
                    mainWorkEvent.BeginTime, mainWorkEvent.BeginTime,
                    mainWorkEvent.UserID, mainWorkEvent.ProjectID,
                    mainWorkEvent.WorkCategoryID, mainWorkEvent.EventTypeID);

                WorkEvent.CreateEvent(endTime, DateTime.Now,
                                      m_UserID, (int) WorkEventType.TimeOff);
            }
		}

		/// <summary>
		/// Closes main work event for current date.
		/// </summary>
		/// <exception cref="Exception">There is no open main work event.</exception>
        public void CloseMainWorkEvent()
		{
		    WorkEvent mainWorkEvent = GetMainWorkEvent(DateTime.Today);

		    if (mainWorkEvent == null)
		        throw new Exception("There is no main work event.");

		    if (!mainWorkEvent.IsOpen)
		        throw new Exception("There is no main work event.");

		    // Закрыть все открытые в рамках текущего MW события.
		    foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today))
		    {
		        if (!workEvent.IsOpen)
		            continue;

		        if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
		            continue;

		        if (WorkEvent.IsWorkEvent(workEvent.EventTypeID))
		        {
		            CloseWorkBreakEvent();
		        }
		        else if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
		        {
		            CloseLunchEvent();
		        }
		        else if (WorkEvent.IsBreakEvent(workEvent.EventTypeID))
		        {
		            CloseWorkBreakEvent();
		        }
		    }

			LogUserWorkEvent(m_UserID, "MainWork", false);

		    WorkEvent.UpdateEvent(
		        mainWorkEvent.ID, mainWorkEvent.Name,
		        mainWorkEvent.BeginTime, DateTime.Now,
		        mainWorkEvent.UserID, mainWorkEvent.ProjectID,
		        mainWorkEvent.WorkCategoryID, mainWorkEvent.EventTypeID);
		}

	    /// <summary>
		/// Opens work or break event.
		/// </summary>
		/// <param name="eventType">Work event type.</param>
        public void OpenWorkBreakEvent(WorkEventType eventType)
	    {
	        if (WorkEvent.IsMainWorkEvent((int) eventType))
	            throw new ArgumentException("Event type is incorrect.");

	        if (WorkEvent.IsLunchEvent((int) eventType))
	            throw new ArgumentException("Event type is incorrect.");

	        if (!WorkEvent.IsWorkEvent((int) eventType) && !WorkEvent.IsBreakEvent((int) eventType))
	            throw new ArgumentException("Event type is incorrect.");

	        WorkEvent mainWorkEvent = GetMainWorkEvent(DateTime.Today);

	        if (mainWorkEvent == null)
	            throw new Exception("There is no main work event.");

	        if (!mainWorkEvent.IsOpen)
	            throw new Exception("There is no open main work event.");

	        if (mainWorkEvent.BeginTime > DateTime.Now)
	            throw new Exception("Can't open work event outside main work event.");

	        // Закрыть все открытые в рамках текущего MW события.
	        foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today))
	        {
                if (!workEvent.IsOpen || WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
	                continue;

	            if (WorkEvent.IsWorkEvent(workEvent.EventTypeID))
	            {
	                CloseWorkBreakEvent();
	            }
	            else if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
	            {
	                CloseLunchEvent();
	            }
	            else if (WorkEvent.IsBreakEvent(workEvent.EventTypeID))
	            {
	                CloseWorkBreakEvent();
	            }
	        }

            WorkEvent.CreateEvent(DateTime.Now, DateTime.Now,
	                              m_UserID, (int) eventType);
	    }

	    /// <summary>
		/// Closes open work or break event.
		/// </summary>
        public void CloseWorkBreakEvent()
	    {
	        WorkEvent[] workEvents =
	            WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today);

	        foreach (WorkEvent workEvent in workEvents)
	        {
	            if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
	                continue;

	            if (WorkEvent.IsBreakEvent(workEvent.EventTypeID) && workEvent.IsOpen)
	            {
					LogUserWorkEvent(m_UserID, "Break", false);

	                WorkEvent.UpdateEvent(
	                    workEvent.ID, workEvent.Name,
	                    workEvent.BeginTime, DateTime.Now,
	                    workEvent.UserID, workEvent.ProjectID,
	                    workEvent.WorkCategoryID, workEvent.EventTypeID);
	            }
	            else if (WorkEvent.IsWorkEvent(workEvent.EventTypeID) && workEvent.IsOpen)
	            {
					LogUserWorkEvent(m_UserID, "MainWork", false);

	                WorkEvent.UpdateEvent(
	                    workEvent.ID, workEvent.Name,
	                    workEvent.BeginTime, DateTime.Now,
	                    workEvent.UserID, workEvent.ProjectID,
	                    workEvent.WorkCategoryID, workEvent.EventTypeID);
	            }
	        }
	    }

	    /// <summary>
		/// Opens lunch event.
		/// </summary>
        public void OpenLunchEvent()
	    {
	        WorkEvent[] workEvents =
	            WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today);

	        WorkEvent mainWorkEvent = null;
	        bool hasAbsenceReason = false;

	        foreach (WorkEvent workEvent in workEvents)
	        {
	            if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
	                mainWorkEvent = workEvent;
	            else if (WorkEvent.IsAbsenceEvent(workEvent.EventTypeID))
	                hasAbsenceReason = true;
	        }

	        if (mainWorkEvent == null)
	            throw new Exception("There is no main work event.");

	        if (!mainWorkEvent.IsOpen)
	            throw new Exception("There is no open main work event.");

            if (mainWorkEvent.BeginTime > DateTime.Now)
	            throw new Exception("Can't open work event outside main work event.");

	        // Закрыть все открытые в рамках текущего MW события.
	        foreach (WorkEvent workEvent in workEvents)
	        {
	            if (!workEvent.IsOpen)
	                continue;

	            if (WorkEvent.IsMainWorkEvent(workEvent.EventTypeID))
	                continue;

	            if (WorkEvent.IsWorkEvent(workEvent.EventTypeID))
	            {
	                CloseWorkBreakEvent();
	            }
	            else if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
	            {
	                CloseLunchEvent();
	            }
	            else if (WorkEvent.IsBreakEvent(workEvent.EventTypeID))
	            {
	                CloseWorkBreakEvent();
	            }
	        }

	        // В выходные дни вместо обеда создаются события перерыва.
	        if (hasAbsenceReason)
	        {
                WorkEvent.CreateEvent(DateTime.Now, DateTime.Now,
	                                  m_UserID, (int) WorkEventType.TimeOff);
	        }
	        else if (CalendarItem.GetHoliday(DateTime.Today))
	        {
                WorkEvent.CreateEvent(DateTime.Now, DateTime.Now,
	                                  m_UserID, (int) WorkEventType.TimeOff);
	        }
	        else
	        {
	            TimeSpan lunchDuration = TimeSpan.Zero;

	            foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today))
	            {
	                if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
	                {
	                    lunchDuration += workEvent.Duration;
	                }
	            }

	            if (lunchDuration >= Globals.Settings.WorkTime.MaxLunchTime)
	            {
                    WorkEvent.CreateEvent(DateTime.Now, DateTime.Now,
	                                      m_UserID, (int) WorkEventType.TimeOff);
	            }
	            else
	            {
                    WorkEvent.CreateEvent(DateTime.Now, DateTime.Now,
	                                      m_UserID, (int) WorkEventType.LanchTime);
	            }
	        }
	    }

	    /// <summary>
		/// Closes open lunch event.
		/// </summary>
        public void CloseLunchEvent()
	    {
	        WorkEvent openLunchEvent = null;
	        TimeSpan lunchDuration = TimeSpan.Zero;
	        TimeSpan openLunchEventDuration = TimeSpan.Zero;

	        foreach (WorkEvent workEvent in WorkEvent.GetEventsOfDate(m_UserID, DateTime.Today))
	        {
	            if (WorkEvent.IsLunchEvent(workEvent.EventTypeID))
	            {
	                if (workEvent.IsOpen)
	                {
	                    openLunchEvent = workEvent;
	                    openLunchEventDuration = openLunchEvent.Duration;
	                    lunchDuration += openLunchEventDuration;
	                }
	                else
	                    lunchDuration += workEvent.Duration;
	            }
	        }

	        if (openLunchEvent == null)
	            throw new Exception("There is no open lunch event.");

			LogUserWorkEvent(m_UserID, "Lunch", false);

	        if (lunchDuration <= Globals.Settings.WorkTime.MaxLunchTime)
	        {
	            WorkEvent.UpdateEvent(
	                openLunchEvent.ID, openLunchEvent.Name,
	                openLunchEvent.BeginTime, DateTime.Now,
	                openLunchEvent.UserID, openLunchEvent.ProjectID,
	                openLunchEvent.WorkCategoryID, openLunchEvent.EventTypeID);
	        }
	        else
	        {
	            lunchDuration = lunchDuration - openLunchEventDuration;
	            TimeSpan rest = Globals.Settings.WorkTime.MaxLunchTime - lunchDuration;

	            WorkEvent.UpdateEvent(
	                openLunchEvent.ID, openLunchEvent.Name,
	                openLunchEvent.BeginTime, openLunchEvent.BeginTime + rest,
	                openLunchEvent.UserID, openLunchEvent.ProjectID,
	                openLunchEvent.WorkCategoryID, openLunchEvent.EventTypeID);

	            WorkEvent.CreateEvent(
	                openLunchEvent.BeginTime + rest, DateTime.Now,
	                m_UserID, (int) WorkEventType.TimeOff);
	        }
	    }

	    #endregion

		#endregion
	}
}