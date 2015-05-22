using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.DAL
{
    public abstract class EventsProvider : DataAccess
    {
        #region Fields

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private static EventsProvider m_Instance;

		#endregion

		#region Properties

		/// <summary>
		/// Instance of events provider.
		/// </summary>
		public static EventsProvider Instance
		{
			//[DebuggerStepThrough]
			get
			{
				if( m_Instance == null )
					m_Instance = (EventsProvider) Activator.CreateInstance(
						Type.GetType( Globals.Settings.Events.ProviderType ) );
				return m_Instance;
			}
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		protected EventsProvider()
		{
			ConnectionString = Globals.Settings.Events.ConnectionString;
		}

		#endregion

        /// <summary>
        /// Creates new event in database.
        /// </summary>
        /// <param name="details">Event.</param>
        /// <returns>ID of new database record.</returns>
        public abstract int CreateEvent(Event details);

        /// <summary>
        /// Add personal user event.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="details">Event details.</param>
        /// <returns>ID of added event.</returns>
        public abstract int AddPersonalUserEvent(int userID, Event details);

        /// <summary>
        /// Add group event.
        /// </summary>
        /// <param name="groupID">Group id.</param>
        /// <param name="details">Event details.</param>
        /// <returns>ID of added event.</returns>
        public abstract int AddGroupEvent(int groupID, Event details);

        /// <summary>
        /// Subscribe user on event.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="eventID">Event id.</param>
        public abstract void SubscribeUserOnEvent(int userID, int eventID);

        /// <summary>
        /// UnSubscribe user on event.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="eventID">Event id.</param>
        public abstract void UnSubscribeUserOnEvent(int userID, int eventID);

        /// <summary>
        /// Updates event information in database.
        /// </summary>
        /// <param name="details">Event details.</param>
        /// <returns>True if information was updated; false, otherwise.</returns>
        public abstract bool UpdateEvent(Event details);

        /// <summary>
        /// Deletes work event from database.
        /// </summary>
        /// <param name="id">ID of work event.</param>
        /// <returns>True if work event was deleted; false, otherwise.</returns>
        public abstract bool DeleteEvent(int id);
        
        /// <summary>
        /// Deletes all events of user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>True if events were deleted; false, otherwise.</returns>
        public abstract bool DeleteUserEvents(int userId);

        /// <summary>
        /// Returns all events for given user.
        /// </summary>
        /// <param name="userID">User id.</param>
        public abstract IList<UserEvent> GetAllUserEvents(int userID);

        /// <summary>
        /// Returns all events for given user.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="returnActualDateInformation">Is need to return actual information of current date.</param>
        public abstract IList<Event> GetAllUserEventsData(int userID, bool returnActualDateInformation);

        /// <summary>
        /// return all user events with sorting and paging.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns></returns>
        public abstract IList<Event> GetAllUserEvents(int userID,
                                                          string sortExpr,
                                                          int pageIndex, int pageSize);

        /// <summary>
        /// Get count of events for user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Number of events.</returns>
        public abstract int GetAllUserEventsCount(int userID);

        /// <summary>
        /// Returns all individual events for given user.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="returnActualDateInformation">Bool parameter - is need to return actual data to current date or ignore DateTime.Now.</param>
        public abstract IList<UserEvent> GetIndividualUserEvents(int userID, bool returnActualDateInformation);

        /// <summary>
        /// Returns all events for given group.
        /// </summary>
        /// <param name="groupID">Group id.</param>
        /// <param name="returnActualDateInformation">Bool parameter - is need to return actual data to current date or ignore DateTime.Now.</param>
        public abstract IList<UserEvent> GetGroupEvents(int groupID, bool returnActualDateInformation);

        /// <summary>
        /// Returns all events.
        /// </summary>
        /// <returns>Array of all events.</returns>
        public abstract IList<Event> GetAllEvents();

        /// <summary>
        /// Returns all events.
        /// </summary>
        /// <param name="userID">ID of user.</param>
        /// <param name="SortExpression">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Array of all events.</returns>
        public abstract IList<Event> GetAllUserPotentialEvents(int userID, string SortExpression, int pageIndex, int pageSize);

        /// <summary>
        /// Returns ocunt of filtered events.
        /// </summary>
        /// <param name="userID">ID of user.</param>
        /// <returns>Count of events.</returns>
        public abstract int GetAllUserPotentialEventsCount(int userID);

        /// <summary>
        /// Returns event with given ID.
        /// </summary>
        /// <param name="id">ID of event.</param>
        /// <returns>Event with given ID. Null, otherwise.</returns>
        public abstract Event GetEventByID(int id);

        /// <summary>
        /// Remove event from group.
        /// </summary>
        /// <param name="groupID">Group ID.</param>
        /// <param name="eventID">Event ID.</param>
        public abstract void DeleteEventFromGroup(int groupID, int eventID);

        /// <summary>
        /// Remove event from user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="eventID">Event ID.</param>
        public abstract void DeleteEventFromUser(int userID, int eventID);

        /// <summary>
        /// Return users ID's - owners of current event.
        /// </summary>
        /// <param name="eventID">Event ID.</param>
        /// <returns>ID's list.</returns>
        public abstract IList<int> GetUsersOfEvent(int eventID);

        /// <summary>
        /// Return griups - owners of current event.
        /// </summary>
        /// <param name="eventID">Event ID</param>
        /// <returns>List of groups.</returns>
        public abstract IList<Role> GetGroupsOfEvent(int eventID);
    }
}
