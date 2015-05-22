using System.Collections.Generic;

using ConfirmIt.PortalLib.DAL;
using UIPProcess.DataSources;

namespace UlterSystems.PortalLib.BusinessObjects
{
    public class UserEventsProvider : DataSourceBase<Event>
    {
        /// <summary>
        /// Returns number of events.
        /// </summary>
        /// <returns>Number of events.</returns>
        public override int SelectCount()
        {
            if (UserID == 0)
                return 0;

            return SiteProvider.Events.GetAllUserEventsCount(UserID);
        }

        /// <summary>
        /// Returns page of sorted events.
        /// </summary>
        /// <param name="SortExpression">Sorting expression.</param>
        /// <param name="startRowIndex">Index of starting row.</param>
        /// <param name="maximumRows">Page size.</param>
        /// <returns>Array of sorted and filtered events.</returns>
        public override IList<Event> Select(string SortExpression, int maximumRows, int startRowIndex)
        {
            if (UserID == 0)
                return new List<Event>();

            if (maximumRows == 0)
                maximumRows = 10;

            int pageIndex = (startRowIndex / maximumRows);

            IList<Event> eventsDetails =
                SiteProvider.Events.GetAllUserEvents(UserID, SortExpression,
                                                     pageIndex, maximumRows);

            return eventsDetails;
        }

        public override object Filter
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Current user id.
        /// </summary>
        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }
        private int m_userID = 0;

        #region DataSource Methods

        /// <summary>
        /// Returns page of sorted events.
        /// </summary>
        /// <param name="userId">Filter user id.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="rowIndex">Index of starting row.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Array of sorted and filtered events.</returns>
        public static IList<Event> GetEvents(int userId, string sortExpr,
                                        int rowIndex, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 10;

            int pageIndex = (rowIndex / pageSize);

            IList<Event> eventsDetails =
                SiteProvider.Events.GetAllUserEvents(userId, sortExpr,
                                                     pageIndex, pageSize);

            return eventsDetails;
        }

        /// <summary>
        /// Returns number of events.
        /// </summary>
        /// <param name="userId">Filter user id.</param>
        /// <returns>Number of events.</returns>
        public static int GetEventsCount(int userId)
        {
            return SiteProvider.Events.GetAllUserEventsCount(userId);
        }

        /// <summary>
        /// Delete event with the id.
        /// </summary>
        /// <param name="id">Event id.</param>
        public static void DeleteEvent(int id)
        {
            if (id <= 0)
                return;

            UserEvent userEvent = new UserEvent();
            userEvent.Load(id);
            userEvent.Delete();
        }

        #endregion
    }

    public class UserSubscribeEventsDataSource : DataSourceBase<Event>
    {
        /// <summary>
        /// Returns number of events for subscribe.
        /// </summary>
        /// <returns>Number of events.</returns>
        public override int SelectCount()
        {
            if (UserID == 0)
                return 0;

            return SiteProvider.Events.GetAllUserPotentialEventsCount(UserID);
        }

        /// <summary>
        /// Returns page of sorted events for subscribe.
        /// </summary>
        /// <param name="SortExpression">Sorting expression.</param>
        /// <param name="startRowIndex">Index of starting row.</param>
        /// <param name="maximumRows">Page size.</param>
        /// <returns>Array of sorted and filtered events.</returns>
        public override IList<Event> Select(string SortExpression, int maximumRows, int startRowIndex)
        {
            if (UserID == 0)
                return new List<Event>();

            if (maximumRows == 0)
                maximumRows = 10;

            int pageIndex = (startRowIndex / maximumRows);

            IList<Event> eventsDetails =
                SiteProvider.Events.GetAllUserPotentialEvents(UserID
                                                              , SortExpression
                                                              , pageIndex
                                                              , maximumRows);

            return eventsDetails;
        }

        public override object Filter
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Current user id.
        /// </summary>
        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }
        private int m_userID = 0;
    }
}
