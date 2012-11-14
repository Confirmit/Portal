using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using ConfirmIt.PortalLib.BAL;
using Core.DB;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
	/// <summary>
	/// Provider of offices data for MS SQL Server.
	/// </summary>
    public class SqlEventsProvider : EventsProvider
	{
		#region ReadOnly Fields

		private const string DBEventsTableName = "Events";
        private const string DBUserEventsTableName = "UserEvents";
        private const string DBGroupEventsTableName = "GroupEvents";
		
        #endregion

		#region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Cache<int, Event> m_Cache = new Cache<int, Event>(delegate { return true; });
		
        #endregion

        #region Create, Update, Delete, GetAllEvents, GetEventByID

        /// <summary>
        /// Returns event by given ID.
        /// </summary>
        /// <param name="id">ID of event.</param>
        /// <returns>Event with given ID; null, otherwise.</returns>
        public override Event GetEventByID(int id)
        {
            if (m_Cache.ContainsKey(id))
                return m_Cache[id];

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM {0} WHERE ID = @ID", DBEventsTableName);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                using (IDataReader reader = ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        Event details = GetEventDataFromReader(reader);
                        m_Cache[id] = details;
                        return details;
                    }

                    return null;
                }
            }
        }

        /// <summary>
		/// Creates new event record in database.
		/// </summary>
		/// <param name="details">Event details.</param>
		/// <returns>ID of new record.</returns>
        public override int CreateEvent(Event details)
		{
			int id = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        @"INSERT INTO {0} (Title, Date, Description, DateFormat, OwnerID, IsPublic) VALUES  (@Title, @Date, @Description, @DateFormat, @OwnerID, @IsPublic) SELECT @@IDENTITY",
                        DBEventsTableName);
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value =
                    details.DateTime;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                    details.Description;
                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value =
                    details.Title;
                command.Parameters.Add("@DateFormat", SqlDbType.NVarChar).Value =
                    details.DateFormat;
                command.Parameters.Add("@OwnerID", SqlDbType.Int).Value =
                    details.OwnerID;
                command.Parameters.Add("@IsPublic", SqlDbType.Bit).Value =
                    details.IsPublic;

                try
                {
                    id = Convert.ToInt32(ExecuteScalar(command));
                    transaction.Commit();
                    details.ID = id;
                    m_Cache[id] = details;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
		    return id;
		}

		/// <summary>
		/// Updates event information in database.
		/// </summary>
		/// <param name="details">Event details.</param>
		/// <returns>True if record was successfully updated; false, otherwise.</returns>
        public override bool UpdateEvent(Event details)
		{
		    using (SqlConnection connection = new SqlConnection(ConnectionString))
		    {
		        connection.Open();
		        SqlTransaction transaction = connection.BeginTransaction();
		        SqlCommand command = connection.CreateCommand();
		        command.Transaction = transaction;
		        command.CommandText =
		            string.Format(
                        "UPDATE {0} SET Date = @Date, Description = @Description, Title = @Title, DateFormat = @DateFormat, IsPublic = @IsPublic WHERE ID = @ID",
		                DBEventsTableName);
		        command.Parameters.Add("@ID", SqlDbType.Int).Value = details.ID;
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value =
		            details.DateTime;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
		            details.Description;
		        command.Parameters.Add("@DateFormat", SqlDbType.NVarChar).Value =
		            details.DateFormat;
                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value =
                    details.Title;
                command.Parameters.Add("@IsPublic", SqlDbType.Bit).Value =
		            details.IsPublic;

		        try
		        {
		            bool result = (ExecuteNonQuery(command) == 1);

		            if (result)
		            {
		                transaction.Commit();
		                m_Cache[details.ID.Value] = details;
		            }
		            else
		                transaction.Rollback();

		            return result;
		        }
		        catch
		        {
		            transaction.Rollback();
		            return false;
		        }
		    }
		}

	    /// <summary>
		/// Deletes event from database.
		/// </summary>
		/// <param name="id">ID of event.</param>
		/// <returns>True if record was successfully deleted; false, otherwise.</returns>
        public override bool DeleteEvent(int id)
	    {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = string.Format("DELETE FROM {0} WHERE ID = @ID", DBEventsTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                try
                {
                    bool result = (ExecuteNonQuery(command) == 1);

                    if (result)
                    {
                        m_Cache.Remove(id);
                        transaction.Commit();
                    }
                    else
                        transaction.Rollback();

                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
	    }

	    /// <summary>
		/// Returns all events.
		/// </summary>
        public override IList<Event> GetAllEvents()
	    {
	        using (SqlConnection connection = new SqlConnection(ConnectionString))
	        {
	            SqlCommand command = connection.CreateCommand();
	            command.CommandText = string.Format("SELECT * FROM {0} ", DBEventsTableName);

	            connection.Open();
	            using (IDataReader reader = ExecuteReader(command))
	            {
	                return GetAllEventsDataFromReader(reader);
	            }
	        }
	    }

        /// <summary>
        /// Get count of all user events.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Number of events.</returns>
        public override int GetAllUserEventsCount(int userID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat(" SELECT COUNT(*) FROM {0} E WHERE ", DBEventsTableName);
                query.AppendFormat(" EXISTS ({0}) ", getUserEventsFilter(userID, false));
                query.AppendFormat(" OR EXISTS ({0}) ", getEventsInGroupEventsFilter(userID, true));
                command.CommandText = query.ToString();

                connection.Open();
                return (int)ExecuteScalar(command);
            }
        }

	    /// <summary>
        /// Returns all user potential events.
        /// </summary>
        public override IList<Event> GetAllUserPotentialEvents(int userID, string SortExpression, int pageIndex, int pageSize)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();
                
                int lowerBound = pageIndex * pageSize + 1;
                int upperBound = (pageIndex + 1) * pageSize;
                if (string.IsNullOrEmpty(SortExpression))
                    SortExpression = "Date";

                query.Append(" SELECT * FROM ");
                query.Append(" ( ");
                query.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum, * FROM {1} E ",
                                   SortExpression, DBEventsTableName);
                query.Append(" WHERE ");
                query.Append(" ( ");
                query.Append(" E.IsPublic = 'True' ");
                query.Append(" AND ");
                query.AppendFormat(" NOT EXISTS ({0}) ", getUserEventsFilter(userID, false));
                query.AppendFormat(" AND NOT EXISTS ({0}) ", getEventsInGroupEventsFilter(userID, false));
                query.Append(" ) ");
                query.AppendFormat(" OR EXISTS ({0}) ", getUserEventsFilter(userID, true));
                query.Append(" ) AS TMP ");
                query.AppendFormat(" WHERE TMP.RowNum BETWEEN {0} AND {1} ORDER BY RowNum ASC ",
                                   lowerBound, upperBound);

                command.CommandText = query.ToString();
                connection.Open();

                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllEventsDataFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Return count of filtered events.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Number of events.</returns>
        public override int GetAllUserPotentialEventsCount(int userID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();
                query.AppendFormat(" SELECT COUNT(*) FROM {0} E ", DBEventsTableName);
                query.Append(" WHERE ");
                query.Append(" ( ");
                query.Append(" E.IsPublic = 'True' ");
                query.Append(" AND ");
                query.AppendFormat(" NOT EXISTS ({0}) ", getUserEventsFilter(userID, false));
                query.AppendFormat(" AND NOT EXISTS ({0}) ", getEventsInGroupEventsFilter(userID, false));
                query.Append(" ) ");
                query.AppendFormat(" OR EXISTS ({0}) ", getUserEventsFilter(userID, true));
                
                command.CommandText = query.ToString();
                connection.Open();

                return (int)ExecuteScalar(command);
            }
        }

        #endregion

        #region Query constructor support

        /// <summary>
        /// Get condition for including events in UserEvents table.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="IsIgnored">Is ignore event or not.</param>
        private string getUserEventsFilter(int userID, bool IsIgnored)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat(" SELECT * FROM {0} UE ", DBUserEventsTableName);
            query.AppendFormat(" WHERE UE.EventID = E.ID AND UE.UserID = '{0}' AND UE.IsIgnore = '{1}' ",
                               userID, IsIgnored);

            return query.ToString();
        }

        /// <summary>
        /// Get condition for including events in GroupEvents table.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="includeUnSubscribeFilter">Need to filter unsubscribe group events.</param>
        private string getEventsInGroupEventsFilter(int userID, bool includeUnSubscribeFilter)
        {
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT * ");
            query.Append(" FROM Person2Group P2G ");
            query.AppendFormat(" WHERE P2G.PersonID = '{0}' ", userID);
            query.Append(" AND EXISTS ");
            query.Append(" ( ");
            query.Append(" SELECT * FROM GroupEvents GE ");
            query.Append(" WHERE ");
            query.Append(" GE.EventID = E.ID ");
            query.Append(" AND GE.GroupID = P2G.GroupID ");
            query.Append(" ) ");

            if (includeUnSubscribeFilter)
                query.AppendFormat(" AND NOT EXISTS ({0}) ", getUserEventsFilter(userID, true));

            return query.ToString();
        }

	    #endregion

        #region GetAllEventsFromReader, GetEventFromReader, GetAllEventsDataFromReader, GetEventDataFromReader

        /// <summary>
        /// Returns all events from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>All events from data reader.</returns>
        protected virtual List<UserEvent> GetAllEventsFromReader(IDataReader reader)
        {
            List<UserEvent> events = new List<UserEvent>();
            while (reader.Read())
            {
                events.Add(new UserEvent(GetEventDataFromReader(reader)));
            }
            return events;
        }

        /// <summary>
        /// Returns all events from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>All events from data reader.</returns>
        protected virtual List<Event> GetAllEventsDataFromReader(IDataReader reader)
        {
            List<Event> eventsData = new List<Event>();
            while (reader.Read())
            {
                eventsData.Add(GetEventDataFromReader(reader));
            }
            return eventsData;
        }

        /// <summary>
        /// Returns event from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Event from data reader.</returns>
        protected virtual Event GetEventDataFromReader(IDataReader reader)
        {
            Event details = new Event();
            details.ID = (int)reader["ID"];
            details.OwnerID = (int)reader["OwnerID"];
            details.DateTime = (DateTime)reader["Date"];
            details.Title = (string)reader["Title"];
            details.IsPublic = (bool)reader["IsPublic"];

            details.Description = reader["Description"] == DBNull.Value
                                      ? null
                                      : (string)reader["Description"];

            details.DateFormat = reader["DateFormat"] == DBNull.Value
                          ? null
                          : (string)reader["DateFormat"];

            return details;
        }

        #endregion

        #region Person events

        /// <summary>
        /// Return individual events of current user.
        /// </summary>
        /// <param name="userID">Current user id.</param>
        /// <param name="returnActualDateInformation">Bool parameter - is need to return actual data to current date or ignore DateTime.Now.</param>
        public override IList<UserEvent> GetIndividualUserEvents(int userID, bool returnActualDateInformation)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();
                query.AppendFormat("SELECT {0}.ID, {0}.Date, {0}.Title, (0}.Description, {0}.DateFormat, {0}.OwnerID, {0}.IsPublic ", DBEventsTableName);
                query.AppendFormat("FROM {0}, {1}", DBEventsTableName, DBUserEventsTableName);
                query.AppendFormat("WHERE {1}.UserID = @userID AND {1}.EventID = {0}.ID {2} AND {1}.IsIgnore = 'False' ",
                                   DBEventsTableName,
                                   DBUserEventsTableName,
                                   getDateFilter(DBEventsTableName, returnActualDateInformation));

                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@userID", userID);
                connection.Open();

                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllEventsFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Return all events of current user.
        /// </summary>
        /// <param name="userID">Current user id.</param>
        public override IList<UserEvent> GetAllUserEvents(int userID)
        {
            IList<UserEvent> list = new List<UserEvent>();
            foreach (Event eventData in GetAllUserEventsData(userID, true))
            {
                list.Add(new UserEvent(eventData));
            }
            return list;
        }

	    /// <summary>
        /// Return all events of current user. Using for SLSErvice.
        /// </summary>
        /// <param name="userID">Current user id.</param>
        /// <param name="returnActualDateInformation">Is need to return actual information of current date.</param>
        public override IList<Event> GetAllUserEventsData(int userID, bool returnActualDateInformation)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.Append(" SELECT * ");
                query.AppendFormat(" FROM {0} E WHERE ", DBEventsTableName);
                query.Append(" ( ");
                query.AppendFormat(" EXISTS ({0}) ", getUserEventsFilter(userID, false));
                query.AppendFormat(" OR EXISTS ({0}) ", getEventsInGroupEventsFilter(userID, true));
                query.AppendFormat(" ) {0} ", getDateFilter("E", true));
                
                command.CommandText = query.ToString();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;
                    command.CommandText = query.ToString();
                    command.Parameters.AddWithValue("@userID", userID);

                    using (IDataReader reader = ExecuteReader(command))
                    {
                        return GetAllEventsDataFromReader(reader);
                    }
                }
                finally
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Returns page of sorted events.
        /// </summary>
        /// <param name="userID">User identificator.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Array of sorted and filtered events.</returns>
        public override IList<Event> GetAllUserEvents(int userID, string sortExpr,
                                                 int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(sortExpr))
                sortExpr = "Date";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat(" SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum ", sortExpr);
                query.AppendFormat(" FROM {0} E WHERE ", DBEventsTableName);
                query.Append(" ( ");
                query.AppendFormat(" EXISTS ({0}) ", getUserEventsFilter(userID, false));
                query.AppendFormat(" OR EXISTS ({0}) ", getEventsInGroupEventsFilter(userID, true));
                query.Append(" ) ");

                command.CommandText = query.ToString();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int lowerBound = pageIndex*pageSize + 1;
                    int upperBound = (pageIndex + 1)*pageSize;

                    command.Transaction = transaction;
                    command.CommandText = query.ToString();
                    command.CommandText =
                        string.Format(
                            "SELECT * FROM ({0}) as temp WHERE temp.RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC",
                            command.CommandText, lowerBound, upperBound);
                    command.Parameters.AddWithValue("@userID", userID);

                    using (IDataReader reader = ExecuteReader(command))
                    {
                        return GetAllEventsDataFromReader(reader);
                    }
                }
                finally
                {
                    transaction.Rollback();
                }
            }
        }

	    /// <summary>
        /// Delete all custom events for given user.
        /// </summary>
        /// <param name="userId">User id.</param>
        public override bool DeleteUserEvents(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("DELETE FROM {0} as UE WHERE UE.UserID=@userID", DBUserEventsTableName);
                command.Parameters.AddWithValue("@userID", userId);
                connection.Open();

                bool result = (ExecuteNonQuery(command) == 1);
                return result;
            }
        }

        #endregion

        #region Group Events

        /// <summary>
        /// Return events of current group.
        /// </summary>
        /// <param name="groupID">Group id.</param>
        /// <returns>All events from data reader.</returns>
        /// <param name="returnActualDateInformation">Bool parameter - is need to return actual data to current date or ignore DateTime.Now.</param>
        public override IList<UserEvent> GetGroupEvents(int groupID, bool returnActualDateInformation)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();
                query.AppendFormat(" SELECT {0}.ID, {0}.Date, {0}.Title, {0}.Description, {0}.DateFormat, {0}.OwnerID, {0}.IsPublic ", DBEventsTableName);
                query.AppendFormat(" FROM {0}, {1} ", DBEventsTableName, DBGroupEventsTableName);
                query.AppendFormat(" WHERE {1}.GroupID = @groupID AND {1}.EventID = {0}.ID {2} ",
                                   DBEventsTableName,
                                   DBGroupEventsTableName,
                                   getDateFilter(DBEventsTableName, returnActualDateInformation));
                
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@groupID", groupID);
                connection.Open();

                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllEventsFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Return groups of current event.
        /// </summary>
        /// <param name="eventID">Event id.</param>
        /// <returns>All groups from data reader.</returns>
        public override IList<Role> GetGroupsOfEvent(int eventID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "SELECT G.ID, G.GroupID, G.Name, G.Description FROM {0} AS GE, Groups AS G WHERE GE.GroupID = G.ID AND GE.EventID = @eventID ", DBGroupEventsTableName);
                command.Parameters.AddWithValue("@eventID", eventID);
                connection.Open();

                using (IDataReader reader = ExecuteReader(command))
                {
                    IList<Role> list = new List<Role>();
                    foreach (RoleDetails details in SiteProvider.Roles.GetAllRoleDetailsFromReader(reader))
                    {
                        list.Add(Role.GetRoleFromDetails(details));
                    }

                    return list;
                }
            }
        }

        #endregion

        #region Adding(Deleteing) event to(from) person/group

        /// <summary>
        /// Add personal event to user.
        /// </summary>
        /// <param name="userID">User id.</param>
        /// <param name="details">Event details.</param>
        /// <returns>ID of event.</returns>
        public override int AddPersonalUserEvent(int userID, Event details)
        {
            return addEventToObject(userID, details, DBUserEventsTableName, "UserID");
        }

        /// <summary>
        /// Remove event from user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="eventID">Event ID.</param>
        public override void DeleteEventFromUser(int userID, int eventID)
        {
            deleteEventFromObject(userID, eventID, DBUserEventsTableName , "UserID");
        }

        /// <summary>
        /// Add event to group.
        /// </summary>
        /// <param name="groupID">Group id.</param>
        /// <param name="details">Event details.</param>
        /// <returns>ID of event.</returns>
        public override int AddGroupEvent(int groupID, Event details)
        {
            return addEventToObject(groupID, details, DBGroupEventsTableName, "GroupID");
        }

        /// <summary>
        /// Remove event from group.
        /// </summary>
        /// <param name="groupID">Group ID.</param>
        /// <param name="eventID">Event ID.</param>
        public override void DeleteEventFromGroup(int groupID, int eventID)
        {
            deleteEventFromObject(groupID, eventID, DBGroupEventsTableName, "GroupID");
        }

        /// <summary>
        /// Add event to object (user or group).
        /// </summary>
        /// <param name="objectID">ID of object.</param>
        /// <param name="details">Event details.</param>
        /// <param name="TableName">Name of the table in DB.</param>
        /// <param name="ColumnIDName">Name of column in DB.</param>
        /// <returns></returns>
        private int addEventToObject(int objectID, Event details,
                                    string TableName, string ColumnIDName)
        {
            if (details.IsSaved && isReferenceExist(TableName, ColumnIDName, objectID, details.ID.Value))
                return 0;

            int eventID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                eventID = (details.IsSaved)
                              ? details.ID.Value
                              : CreateEvent(details);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        @"INSERT INTO {0} ({1}, EventID) VALUES  (@objectID, @eventID) SELECT @@IDENTITY",
                        TableName,
                        ColumnIDName);
                command.Parameters.Add("@objectID", SqlDbType.Int).Value = objectID;
                command.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;

                try
                {
                    Convert.ToInt32(ExecuteScalar(command));
                    transaction.Commit();
                    //m_Cache[id] = details;
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return eventID;
        }

        /// <summary>
        /// Add event to object (user or group).
        /// </summary>
        /// <param name="objectID">ID of object.</param>
        /// <param name="eventID">Event ID.</param>
        /// <param name="TableName">Name of the table in DB.</param>
        /// <param name="ColumnIDName">Name of column in DB.</param>
        /// <returns></returns>
        private void deleteEventFromObject(int objectID, int eventID,
                                    string TableName, string ColumnIDName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        @"DELETE FROM {0} WHERE {1}=@objectID AND EventID=@eventID",
                        TableName,
                        ColumnIDName);
                command.Parameters.Add("@objectID", SqlDbType.Int).Value = objectID;
                command.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;

                try
                {
                    ExecuteNonQuery(command);
                    transaction.Commit();
                    //m_Cache.Remove(id);
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        private bool isReferenceExist(string TableName, string ObjectIDColumn,
                                    int objectID, int eventID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT ID FROM {0} WHERE {1}=@ObjectID AND EventID=@EventID",
                                                    TableName, ObjectIDColumn);
                command.Parameters.AddWithValue("@ObjectID", objectID);
                command.Parameters.AddWithValue("@EventID", eventID);

                connection.Open();

                int ID = Convert.ToInt32(ExecuteScalar(command));
                return ID == 0
                           ? false
                           : true;
            }
        }

	    #endregion

        #region Subscribe/UnSubscribe user on event.

        public override void SubscribeUserOnEvent(int userID, int eventID)
        {
            Procedure proc = new Procedure("SubscribeUserOnEvent");
            proc.Add("@UserID", userID, DbType.Int32);
            proc.Add("@EventID", eventID, DbType.Int32);
            proc.ExecNonQuery();
        }

        public override void UnSubscribeUserOnEvent(int userID, int eventID)
        {
            Procedure proc = new Procedure("UnSubscribeUserEvent");
            proc.Add("@UserID", userID, DbType.Int32);
            proc.Add("@EventID", eventID, DbType.Int32);
            proc.ExecNonQuery();
        }

        #endregion

        #region GetUsersOfEvent

        /// <summary>
        /// Return persons of current event.
        /// </summary>
        /// <param name="eventID">Event id.</param>
        /// <returns>All persons from data reader.</returns>
        public override IList<int> GetUsersOfEvent(int eventID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "SELECT UE.UserID FROM {0} AS UE WHERE UE.EventID = @eventID AND UE.IsIgnore = 'False' ", DBUserEventsTableName);

                command.Parameters.AddWithValue("@eventID", eventID);
                connection.Open();

                using (IDataReader reader = ExecuteReader(command))
                {
                    return getAllPersonsIDFromReader(reader);
                }
            }
        }

        private IList<int> getAllPersonsIDFromReader(IDataReader reader)
        {
            IList<int> personsID = new List<int>();
            while (reader.Read())
            {
                int personID = (int) reader["UserID"];
                personsID.Add(personID);
            }
            return personsID;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return subQuery to filter events to current date.
        /// </summary>
        /// <param name="returnActualDateInformation">Is need to ignore date.</param>
        /// <returns>Empty string if date is ignore. SubQuery otherwise.</returns>
        private String getDateFilter(string tableName, bool returnActualDateInformation)
        {
            return returnActualDateInformation
                       ? String.Format(" AND {0}.Date > GETDATE()-1 ", tableName)
                       : String.Empty;
        }

	    #endregion
    }
}
