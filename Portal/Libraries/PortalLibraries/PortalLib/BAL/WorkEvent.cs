using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConfirmIt.PortalLib.DAL;

namespace ConfirmIt.PortalLib.BAL
{
	/// <summary>
	/// Work event class.
	/// </summary>
	public class WorkEvent
	{
		#region Fields
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_ID;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_Name;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_BeginTime;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_EndTime;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_UserID;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_ProjectID = 1;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_WorkCategoryID = 1;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_EventTypeID;
		
		#endregion

		#region Properties

		/// <summary>
		/// ID of work event.
		/// </summary>
		public int ID
		{
			[DebuggerStepThrough]
			get { return m_ID; }
			[DebuggerStepThrough]
			set { m_ID = value; }
		}

		/// <summary>
		/// Name of event (not used).
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return m_Name; }
			[DebuggerStepThrough]
			set { m_Name = value; }
		}

		/// <summary>
		/// Time of event start.
		/// </summary>
		public DateTime BeginTime
		{
			[DebuggerStepThrough]
			get { return m_BeginTime; }
			[DebuggerStepThrough]
			set { m_BeginTime = value; }
		}

		/// <summary>
		/// Time of event end.
		/// </summary>
		public DateTime EndTime
		{
			[DebuggerStepThrough]
			get { return m_EndTime; }
			[DebuggerStepThrough]
			set { m_EndTime = value; }
		}

		/// <summary>
		/// Duration of event.
		/// </summary>
		public TimeSpan Duration
		{
			get
			{
				return IsOpen && (BeginTime.Date == DateTime.Today)
						   ? DateTime.Now - BeginTime
						   : EndTime - BeginTime;
			}
			set { } //need to view in WCF Service
		}

		/// <summary>
		/// User ID.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
			[DebuggerStepThrough]
			set { m_UserID = value; }
		}

		/// <summary>
		/// Project ID (not used).
		/// </summary>
		public int ProjectID
		{
			[DebuggerStepThrough]
			get { return m_ProjectID; }
			[DebuggerStepThrough]
			set { m_ProjectID = value; }
		}

		/// <summary>
		/// Work category ID (not used).
		/// </summary>
		public int WorkCategoryID
		{
			[DebuggerStepThrough]
			get { return m_WorkCategoryID; }
			[DebuggerStepThrough]
			set { m_WorkCategoryID = value; }
		}

		/// <summary>
		/// Event type ID.
		/// </summary>
		public int EventTypeID
		{
			[DebuggerStepThrough]
			get { return m_EventTypeID; }
			[DebuggerStepThrough]
			set { m_EventTypeID = value; }
		}

		/// <summary>
		/// Event type.
		/// </summary>
		public WorkEventType EventType
		{
			[DebuggerStepThrough]
			get
			{
				if( !Enum.IsDefined( typeof( WorkEventType ), m_EventTypeID ) )
					throw new Exception( "Invalid ID of event type." );

				return (WorkEventType) m_EventTypeID;
			}
			[DebuggerStepThrough]
			set { m_EventTypeID = (int) value; }
		}

		/// <summary>
		/// Is this event open.
		/// </summary>
		public bool IsOpen
		{
			[DebuggerStepThrough]
			get { return m_BeginTime == m_EndTime; }
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			if (!(obj is WorkEvent))
				return false;

			return this.ID == ((WorkEvent) obj).ID;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		#region Intersection methods

		/// <summary>
		/// Does this event contain another event inside.
		/// </summary>
		/// <param name="workEvent">Another work event.</param>
		/// <returns>True, if this event contains another event inside; false, otherwise.</returns>
		public bool Contains(WorkEvent workEvent)
		{
			if (workEvent == null)
				throw new ArgumentNullException("workEvent");

			return IsOpen
					   ? workEvent.BeginTime >= BeginTime
					   : (workEvent.BeginTime >= BeginTime) &&
						 (workEvent.BeginTime < EndTime) &&
						 (workEvent.EndTime > BeginTime) &&
						 (workEvent.EndTime <= EndTime);
		}

		/// <summary>
		/// Does this event intersect with another event.
		/// </summary>
		/// <param name="workEvent">Another work event.</param>
		/// <returns>True, if this event intersects with another event; false, otherwise.</returns>
		public bool Intersects( WorkEvent workEvent )
		{
			if( workEvent == null )
				throw new ArgumentNullException( "workEvent" );

			return IsOpen
					   ? workEvent.EndTime > BeginTime
					   : (workEvent.BeginTime < EndTime) &&
						 (workEvent.EndTime > BeginTime);
		}

		#endregion

		#endregion

		#region Static methods

		#region Helpers

		/// <summary>
		/// Check if time and date is correct.
		/// </summary>
		/// <param name="begin">Begin date.</param>
		/// <param name="end">End date.</param>
		private static void isDateIsCorrect(DateTime begin, DateTime end)
		{
			if (begin > end)
				throw new Exception("Begin time can't be greater than end time.");

			if (begin.Date != end.Date)
				throw new Exception("Date of begin and end times must be equal.");
		}

		#endregion

		#region CRUD methods

		/// <summary>
		/// Creates new work event in database.
		/// </summary>
		/// <param name="begin">Time of event beginning.</param>
		/// <param name="end">Time of event ending.</param>
		/// <param name="userID">User ID.</param>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>ID of new record.</returns>
		public static int CreateEvent(
			DateTime begin, DateTime end,
			int userID, int eventTypeID
			)
		{
			return CreateEvent(String.Empty, begin,
							   end, userID,
							   1, 1, eventTypeID);
		}

		/// <summary>
		/// Creates new work event in database.
		/// </summary>
		/// <param name="name">Name of event.</param>
		/// <param name="begin">Time of event beginning.</param>
		/// <param name="end">Time of event ending.</param>
		/// <param name="userID">User ID.</param>
		/// <param name="projectID">Project ID.</param>
		/// <param name="workCategoryID">Work category ID.</param>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>ID of new record.</returns>
		public static int CreateEvent(
			string name, DateTime begin,
			DateTime end, int userID,
			int projectID, int workCategoryID,
			int eventTypeID
			)
		{
			isDateIsCorrect(begin, end);
			WorkEventDetails details = new WorkEventDetails(name, begin,
															end, userID,
															projectID, workCategoryID,
															eventTypeID);

			Exception ex = CanCreateEvent(details);
			if( ex != null )
				throw ex;

			return SiteProvider.WorkEvents.CreateEvent(details);
		}

		/// <summary>
		/// Updates work event in database.
		/// </summary>
		/// <param name="id">ID of work event.</param>
		/// <param name="name">Name of event.</param>
		/// <param name="begin">Time of event beginning.</param>
		/// <param name="end">Time of event ending.</param>
		/// <param name="userID">User ID.</param>
		/// <param name="projectID">Project ID.</param>
		/// <param name="workCategoryID">Work category ID.</param>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if record was updated; false, otherwise.</returns>
		public static bool UpdateEvent(
			int id, String name,
			DateTime begin, DateTime end,
			int userID, int projectID,
			int workCategoryID, int eventTypeID
			)
		{
			isDateIsCorrect(begin, end);
			WorkEventDetails details = new WorkEventDetails(id, name, begin,
												end, userID,
												projectID, workCategoryID,
												eventTypeID);

			Exception ex = CanUpdateEvent( details );
			if( ex != null )
				throw ex;

			return SiteProvider.WorkEvents.UpdateEvent( details );
		}

		/// <summary>
		/// Deletes work event in database.
		/// </summary>
		/// <param name="id">ID of work event.</param>
		/// <returns>True if record was deleted; false, otherwise.</returns>
		public static bool DeleteEvent( int id )
		{
			WorkEvent deleteEvent = GetEventByID( id );
			if( deleteEvent != null )
			{
				if( IsMainWorkEvent( deleteEvent.EventTypeID ) )
				{
					foreach(WorkEvent workEvent in
							GetEventsOfDate(deleteEvent.UserID, deleteEvent.BeginTime))
					{
						if( deleteEvent.Contains( workEvent ) && !IsAbsenceEvent( workEvent.EventTypeID )
							&& !IsMainWorkEvent(workEvent.EventTypeID))
						{
							DeleteEvent( workEvent.ID );
						}
					}
				}
			}

			return SiteProvider.WorkEvents.DeleteEvent( id );
		}

		/// <summary>
		/// Returns all user events for date.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="date">Date.</param>
		/// <returns>All user events for given date.</returns>
		public static WorkEvent[] GetEventsOfDate(int userID, DateTime date)
		{
			List<WorkEvent> result = new List<WorkEvent>();
			WorkEventDetails[] details =
				SiteProvider.WorkEvents.GetEventsOfDate(userID, date);

			if (details != null)
				foreach (WorkEventDetails detail in details)
				{
					result.Add(GetEventFromDetails(detail));
				}

			return result.ToArray();
		}

		/// <summary>
		/// Returns all user events for dates range.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="begin">Start of range.</param>
		/// <param name="end">End of range.</param>
		/// <returns>All user events for given range.</returns>
		public static WorkEvent[] GetEventsOfRange( int userID, DateTime begin, DateTime end )
		{
			if( begin.Date > end.Date )
				throw new ArgumentException( "begin" );

			List<WorkEvent> result = new List<WorkEvent>();

			for( DateTime date = begin.Date; date <= end.Date; date = date.AddDays( 1 ) )
			{
				result.AddRange( GetEventsOfDate( userID, date ) );
			}

			return result.ToArray();
		}

		/// <summary>
		/// Returns main work event for given date.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="date">Date.</param>
		/// <returns>Main work event for given date; null, otherwise.</returns>
		public static WorkEvent GetMainWorkEvent( int userID, DateTime date )
		{
			WorkEvent[] workEvents = GetEventsOfDate( userID, date );
			if( workEvents == null )
				return null;

			foreach( WorkEvent workEvent in workEvents )
			{
				if( IsMainWorkEvent( workEvent.EventTypeID ) )
					return workEvent;
			}

			return null;
		}

		/// <summary>
		/// Returns business trip event for given date.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="date">Date.</param>
		/// <returns>Business trip event for given date; null, otherwise.</returns>
		public static WorkEvent GetBusinessTripEvent(int userID, DateTime date)
		{
			WorkEvent[] workEvents = GetEventsOfDate(userID, date);
			if (workEvents == null)
				return null;

			foreach (WorkEvent workEvent in workEvents)
			{
				if (workEvent.EventType == WorkEventType.BusinessTrip)
					return workEvent;
			}

			return null;
		}

		/// <summary>
		/// Returns current work event for given date.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="date">Date.</param>
		/// <returns>Current work event for given date; null, otherwise.</returns>
		public static WorkEvent GetCurrentEventOfDate(int userID, DateTime date)
		{
			return WorkEvent.GetCurrentEventOfDate(userID, date, false);
		}

		public static WorkEvent GetCurrentEventOfDate(int userID, DateTime date, bool returnEventIfClosed)
		{
			WorkEvent[] workEvents = GetEventsOfDate( userID, date );
			if( workEvents == null )
				return null;

			WorkEvent mainWorkEvent = null;
			WorkEvent openEvent = null;
			WorkEvent absenceEvent = null;

			foreach (WorkEvent workEvent in workEvents)
			{
				if (IsMainWorkEvent(workEvent.EventTypeID))
					mainWorkEvent = workEvent;
				else if (IsAbsenceEvent(workEvent.EventTypeID))
					absenceEvent = workEvent;
				else if (workEvent.IsOpen)
					openEvent = workEvent;
			}

			if (mainWorkEvent == null)
				return absenceEvent;
			else
			{
				if (openEvent != null)
					return openEvent;
				else if (mainWorkEvent.IsOpen || returnEventIfClosed)
					return mainWorkEvent;
				else return null;
			}
		}

		/// <summary>
		/// Returns work event with given ID.
		/// </summary>
		/// <param name="id">Work event ID.</param>
		/// <returns>Work event with given ID; null, otherwise.</returns>
		public static WorkEvent GetEventByID( int id )
		{
			WorkEventDetails details = SiteProvider.WorkEvents.GetEventByID( id );
			if( details == null )
				return null;

			return GetEventFromDetails( details );
		}

		/// <summary>
		/// Creates work event from details.
		/// </summary>
		/// <param name="details">Work event details.</param>
		/// <returns>Work event from details.</returns>
		private static WorkEvent GetEventFromDetails( WorkEventDetails details )
		{
			if( details == null )
				throw new ArgumentNullException( "details" );

			WorkEvent workEvent = new WorkEvent();
			workEvent.ID = details.ID;
			workEvent.Name = details.Name;
			workEvent.BeginTime = ( details.BeginTime <= details.EndTime ) ? details.BeginTime : details.EndTime;
			workEvent.EndTime = ( details.EndTime >= details.BeginTime ) ? details.EndTime : details.BeginTime;
			workEvent.UserID = details.UserID;
			workEvent.ProjectID = details.ProjectID;
			workEvent.WorkCategoryID = details.WorkCategoryID;
			workEvent.EventTypeID = details.UptimeEventTypeID;

			return workEvent;
		}
		#endregion

		#region Event type checking

		/// <summary>
		/// Returns if given event is main work event.
		/// </summary>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if given event is main work event; false, otherwise.</returns>
		public static bool IsMainWorkEvent( int eventTypeID )
		{
			if( !Enum.IsDefined( typeof( WorkEventType ), eventTypeID ) )
				throw new ArgumentException( "eventType" );

			WorkEventType eventType = (WorkEventType) eventTypeID;
			return ( eventType == WorkEventType.MainWork );
		}

		/// <summary>
		/// Returns if given event is work event.
		/// </summary>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if given event is work event; false, otherwise.</returns>
		public static bool IsWorkEvent( int eventTypeID )
		{
			if( !Enum.IsDefined( typeof( WorkEventType ), eventTypeID ) )
				throw new ArgumentException( "eventType" );

			WorkEventType eventType = (WorkEventType) eventTypeID;
			return (eventType == WorkEventType.MainWork
					|| eventType == WorkEventType.OfficeOut);
		}

		/// <summary>
		/// Returns if given event is absence event.
		/// </summary>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if given event is absence event; false, otherwise.</returns>
		public static bool IsAbsenceEvent( int eventTypeID )
		{
			if( !Enum.IsDefined( typeof( WorkEventType ), eventTypeID ) )
				throw new ArgumentException( "eventType" );

			WorkEventType eventType = (WorkEventType) eventTypeID;
			return (eventType == WorkEventType.BusinessTrip
					|| eventType == WorkEventType.Ill
					|| eventType == WorkEventType.TrustIll
					|| eventType == WorkEventType.Vacation);
		}

		/// <summary>
		/// Returns if given event is break event.
		/// </summary>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if given event is break event; false, otherwise.</returns>
		public static bool IsBreakEvent( int eventTypeID )
		{
			if( !Enum.IsDefined( typeof( WorkEventType ), eventTypeID ) )
				throw new ArgumentException( "eventType" );

			WorkEventType eventType = (WorkEventType) eventTypeID;
			return (eventType == WorkEventType.LanchTime
					|| eventType == WorkEventType.StudyEnglish
					|| eventType == WorkEventType.TimeOff);
		}

		/// <summary>
		/// Returns if given event is lunch event.
		/// </summary>
		/// <param name="eventTypeID">Event type ID.</param>
		/// <returns>True if given event is lunch event; false, otherwise.</returns>
		public static bool IsLunchEvent( int eventTypeID )
		{
			if( !Enum.IsDefined( typeof( WorkEventType ), eventTypeID ) )
				throw new ArgumentException( "eventType" );

			WorkEventType eventType = (WorkEventType) eventTypeID;
			return ( eventType == WorkEventType.LanchTime );
		}

		#endregion

		#region Business logic methods

		/// <summary>
		/// Check if two event intersects or both open.
		/// </summary>
		/// <param name="dateEvent">First event for checking.</param>
		/// <param name="checkEvent">Second event for checking.</param>
		/// <returns>Exception.</returns>
		private static Exception isIntersectsOrOpen(WorkEvent dateEvent, 
														WorkEvent checkEvent)
		{
			// ПРАВИЛО: События W и B не могут пересекаться.
			if (dateEvent.Intersects(checkEvent))
				return new Exception("Work events or breaks can't intersect.");

			// ПРАВИЛО: Не может быть двух открытых событий данного типа.
			if (dateEvent.IsOpen && checkEvent.IsOpen)
				return new Exception("There can't be two open work or break events.");

			return null;
		}

		private static Exception checkLunchEvent(WorkEventDetails eventDetails,
			 bool hasAbsenceEvent, TimeSpan lunchDuration)
		{
			if (IsLunchEvent(eventDetails.UptimeEventTypeID))
			{
				// ПРАВИЛО: Обед не может быть в выходные.
				if (CalendarItem.GetHoliday(eventDetails.BeginTime))
					return new Exception("Lunch can't be on holidays.");

				// ПРАВИЛО: Обед не может быть в выходные.
				if (hasAbsenceEvent)
					return new Exception("Lunch can't be on absence days.");

				// ПРАВИЛО: Обед не может превышать определенной длительности.
				if (lunchDuration > Globals.Settings.WorkTime.MaxLunchTime)
					return new Exception(string.Format("Lunch can't be longer than {0}.", Globals.Settings.WorkTime.MaxLunchTime));

				// ПРАВИЛО: Обед не может превышать определенной длительности.
				if (lunchDuration + (eventDetails.EndTime - eventDetails.BeginTime) > Globals.Settings.WorkTime.MaxLunchTime)
					return new Exception(string.Format("Lunch can't be longer than {0}.", Globals.Settings.WorkTime.MaxLunchTime));
			}

			return null;
		}

		/// <summary>
		/// Checks if work event can be created.
		/// </summary>
		/// <param name="details">Work event details.</param>
		/// <returns>Exception, if event can't be created; null, otherwise.</returns>
		private static Exception CanCreateEvent(WorkEventDetails details)
		{
			WorkEvent checkEvent = GetEventFromDetails( details );
			WorkEvent[] dateEvents = GetEventsOfDate( details.UserID, details.BeginTime );

			if(IsAbsenceEvent(details.UptimeEventTypeID))
			{
				// ПРАВИЛО: Только одно событие такого рода может быть в сутки.
				foreach (WorkEvent dateEvent in dateEvents)
				{
					if (IsAbsenceEvent(dateEvent.EventTypeID))
						//|| IsMainWorkEvent(dateEvent.EventTypeID))
					{
						DeleteEvent(dateEvent.ID);
						//return new Exception("Only one absence event could be a day.");
					}
				}

				// ПРАВИЛО: Длительность событий такого типа равна суточной норме работы.
				//details.BeginTime = new DateTime( details.BeginTime.Year, details.BeginTime.Month, details.BeginTime.Day, 12, 0, 0 );
				//details.EndTime = details.BeginTime +
				//						Globals.Settings.WorkTime.DefaultWorkTime;
				
				// ПРАВИЛО: События такого типа не могут быть открытыми.
				details.Duration = (int)( details.EndTime - details.BeginTime ).TotalSeconds;
			}
			else if(IsMainWorkEvent(details.UptimeEventTypeID))
			{
				foreach( WorkEvent dateEvent in dateEvents )
				{
					// ПРАВИЛО: В сутках не может быть 2-х событий MW.
					if( IsMainWorkEvent( dateEvent.EventTypeID ) )
						return new Exception( "There can't be two main work events." );
				}
			}
			else if( IsWorkEvent( details.UptimeEventTypeID ) )
			{
				bool inMainWorkEvent = false;

				foreach(WorkEvent dateEvent in dateEvents)
				{
					if( IsMainWorkEvent( dateEvent.EventTypeID ) )
					{
						// ПРАВИЛО: События данного типа должны находиться внутри MW.
						if( dateEvent.Contains( checkEvent ) )
							inMainWorkEvent = true; 
					}
					else
						if (IsWorkEvent(dateEvent.EventTypeID) || IsBreakEvent(dateEvent.EventTypeID))
						{
							Exception ex = isIntersectsOrOpen(dateEvent, checkEvent);
							if (ex != null)
								return ex;
						}
				}

				// ПРАВИЛО: События данного типа должны находиться внутри MW.
				if( !inMainWorkEvent )
					return new Exception( "Break event must be inside main work event." );
			}
			else if (IsBreakEvent(details.UptimeEventTypeID))
			{
				bool inMainWorkEvent = false;
				bool hasAbsenceEvent = false;
				TimeSpan lunchDuration = TimeSpan.Zero;

				foreach (WorkEvent dateEvent in dateEvents)
				{
					if (IsAbsenceEvent(dateEvent.EventTypeID))
						hasAbsenceEvent = true;

					if (IsMainWorkEvent(dateEvent.EventTypeID))
					{
						// ПРАВИЛО: События данного типа должны находиться внутри MW.
						if (dateEvent.Contains(checkEvent))
							inMainWorkEvent = true;
					}
					else if (IsBreakEvent(dateEvent.EventTypeID) || IsWorkEvent(dateEvent.EventTypeID))
					{
						Exception ex = isIntersectsOrOpen(dateEvent, checkEvent);
						if (ex != null)
							return ex;

						if (IsLunchEvent(dateEvent.EventTypeID))
							lunchDuration += dateEvent.Duration;
					}
				}

				// ПРАВИЛО: События данного типа должны находиться внутри MW.
				if (!inMainWorkEvent)
					return new Exception("Break event must be inside main work event.");

				Exception exception = checkLunchEvent(details, hasAbsenceEvent, lunchDuration);
				if (exception != null)
					return exception;
			}
			else
				if (details.UptimeEventTypeID != (int)WorkEventType.NoData)
					return new Exception("Unknown type of event.");

			return null;
		}

		/// <summary>
		/// Checks if work event can be updated.
		/// </summary>
		/// <param name="details">Work event details.</param>
		/// <returns>Exception, if event can't be updated; null, otherwise.</returns>
		private static Exception CanUpdateEvent(WorkEventDetails details)
		{
			WorkEvent checkEvent = GetEventFromDetails(details);
			WorkEvent oldEvent = GetEventByID(details.ID);

			if (oldEvent == null)
				return new Exception("There is no event for update.");

			//if( checkEvent.EventTypeID != oldEvent.EventTypeID )
			//{ return new Exception( "Event type can't be changed." ); }

			WorkEvent[] dateEvents = GetEventsOfDate(details.UserID, details.BeginTime);

			if (IsAbsenceEvent(details.UptimeEventTypeID))
			{
				// ПРАВИЛО: Только одно событие такого рода может быть в сутки.
				foreach (WorkEvent dateEvent in dateEvents)
				{
					//if( dateEvent.ID != checkEvent.ID )
					//	continue;

					if (dateEvent.ID != checkEvent.ID)
						if (IsAbsenceEvent(dateEvent.EventTypeID))
							return new Exception("Only one absence event could be a day.");
				}

				// ПРАВИЛО: Длительность событий такого типа равна суточной норме работы.
				//details.BeginTime = new DateTime( details.BeginTime.Year, details.BeginTime.Month, details.BeginTime.Day, 12, 0, 0 );
				//details.EndTime = details.BeginTime +
				//						Globals.Settings.WorkTime.DefaultWorkTime;
				
				// ПРАВИЛО: События такого типа не могут быть открытыми.
				details.Duration = (int)(details.EndTime - details.BeginTime).TotalSeconds;
			}
			else if (IsMainWorkEvent(details.UptimeEventTypeID))
			{
				// ПРАВИЛО: MW событие должно включать в себя все события W и B.
				foreach (WorkEvent dateEvent in dateEvents)
				{
					if (dateEvent.ID == checkEvent.ID)
						continue;

					if (IsMainWorkEvent(dateEvent.EventTypeID) || IsBreakEvent(dateEvent.EventTypeID))
					{
						if (!checkEvent.Contains(dateEvent))
							return new Exception("All work and break events must be inside main work event.");
					}
				}
			}
			else if (IsWorkEvent(details.UptimeEventTypeID))
			{
				bool inMainWorkEvent = false;

				foreach (WorkEvent dateEvent in dateEvents)
				{
					if (dateEvent.ID == checkEvent.ID)
						continue;

					if (IsMainWorkEvent(dateEvent.EventTypeID))
					{
						// ПРАВИЛО: MW событие должно включать в себя все события W и B.
						if (dateEvent.Contains(checkEvent))
							inMainWorkEvent = true;
					}
					else if (IsWorkEvent(dateEvent.EventTypeID) || IsBreakEvent(dateEvent.EventTypeID))
					{
						Exception exception = isIntersectsOrOpen(dateEvent, checkEvent);
						if (exception != null)
							return exception;
					}
				}

				if (!inMainWorkEvent)
					return new Exception("Work event must be inside main work event.");
			}
			else if (IsBreakEvent(details.UptimeEventTypeID))
			{
				bool inMainWorkEvent = false;
				TimeSpan lunchDuration = TimeSpan.Zero;

				foreach (WorkEvent dateEvent in dateEvents)
				{
					if (dateEvent.ID == checkEvent.ID)
						continue;

					if (IsMainWorkEvent(dateEvent.EventTypeID))
					{
						// ПРАВИЛО: События данного типа должны находиться внутри рабочих событий.
						if (dateEvent.Contains(checkEvent))
							inMainWorkEvent = true;
					}
					else if (IsBreakEvent(dateEvent.EventTypeID) || IsWorkEvent(dateEvent.EventTypeID))
					{
						Exception exception = isIntersectsOrOpen(dateEvent, checkEvent);
						if (exception != null)
							return exception;

						// ПРАВИЛО: Обед не может превышать определенной длительности.
						if (IsLunchEvent(dateEvent.EventTypeID))
							lunchDuration += dateEvent.Duration;
					}
				}

				if (!inMainWorkEvent)
					return new Exception("Break event must be inside work event.");

				if (IsLunchEvent(details.UptimeEventTypeID))
				{
					// ПРАВИЛО: Обед не может превышать определенной длительности.
					lunchDuration += (details.EndTime - details.BeginTime);

					if (lunchDuration > Globals.Settings.WorkTime.MaxLunchTime)
						return new Exception(string.Format("Lunch can't be longer than {0}.", Globals.Settings.WorkTime.MaxLunchTime));
				}
			}
			else
				if (details.UptimeEventTypeID != (int)WorkEventType.NoData)
					return new Exception("Unknown type of event.");

			return null;
		}

		#endregion

		#endregion
	}
}
