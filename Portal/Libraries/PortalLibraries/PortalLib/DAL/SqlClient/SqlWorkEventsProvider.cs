using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UlterSystems.PortalLib;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of work events data for MS SQL Server.
    /// </summary>
    public class SqlWorkEventsProvider : WorkEventsProvider
    {
        #region Constants

        private const string DBEventsTableName = "UptimeEvents";

        #endregion

        #region Fields
        private static readonly Cache<string, List<WorkEventDetails>> m_Cache = new Cache<string, List<WorkEventDetails>>(delegate { return Globals.Settings.WorkTime.CacheEnabled; }, StringComparer.InvariantCultureIgnoreCase);
        #endregion

        /// <summary>
        /// Creates new work event in database.
        /// </summary>
        /// <param name="eventDetails">Work event details.</param>
        /// <returns>ID of new database record.</returns>
        public override int CreateEvent(WorkEventDetails eventDetails)
        {
            int id = -1;
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        "INSERT INTO {0} (Name, BeginTime, EndTime, Duration, UserID, ProjectID, WorkCategoryID, UptimeEventTypeID) VALUES  (@Name, @BeginTime, @EndTime, @Duration, @UserID, @ProjectID, @WorkCategoryID, @UptimeEventTypeID) SELECT @@IDENTITY",
                        DBEventsTableName);
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    eventDetails.Name;
                command.Parameters.Add("@BeginTime", SqlDbType.DateTime).Value =
                    eventDetails.BeginTime;
                command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value =
                    eventDetails.EndTime;
                command.Parameters.Add("@Duration", SqlDbType.Int).Value =
                    eventDetails.Duration;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    eventDetails.UserID;
                command.Parameters.Add("@ProjectID", SqlDbType.Int).Value =
                    eventDetails.ProjectID;
                command.Parameters.Add("@WorkCategoryID", SqlDbType.Int).Value =
                    eventDetails.WorkCategoryID;
                command.Parameters.Add("@UptimeEventTypeID", SqlDbType.Int).Value =
                    eventDetails.UptimeEventTypeID;

                try
                {
                    id = Convert.ToInt32(ExecuteScalar(command));
                    transaction.Commit();
                    DeleteEventIfExist(eventDetails.UserID, eventDetails.BeginTime);
                }
                catch
                { transaction.Rollback(); }
            }
            return id;
        }

        /// <summary>
        /// Updates work event information in database.
        /// </summary>
        /// <param name="eventDetails">Work event details.</param>
        /// <returns>True if information was updated; false, otherwise.</returns>
        public override bool UpdateEvent(WorkEventDetails eventDetails)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        "UPDATE {0} SET Name=@Name, BeginTime=@BeginTime, EndTime=@EndTime, Duration=@Duration, UserID=@UserID, ProjectID=@ProjectID, WorkCategoryID=@WorkCategoryID, UptimeEventTypeID=@UptimeEventTypeID WHERE ID=@ID",
                        DBEventsTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = eventDetails.ID;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    eventDetails.Name;
                command.Parameters.Add("@BeginTime", SqlDbType.DateTime).Value =
                    eventDetails.BeginTime;
                command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value =
                    eventDetails.EndTime;
                command.Parameters.Add("@Duration", SqlDbType.Int).Value =
                    eventDetails.Duration;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    eventDetails.UserID;
                command.Parameters.Add("@ProjectID", SqlDbType.Int).Value =
                    eventDetails.ProjectID;
                command.Parameters.Add("@WorkCategoryID", SqlDbType.Int).Value =
                    eventDetails.WorkCategoryID;
                command.Parameters.Add("@UptimeEventTypeID", SqlDbType.Int).Value =
                    eventDetails.UptimeEventTypeID;

                try
                {
                    bool result = (ExecuteNonQuery(command) == 1);

                    if (result)
                    {
                        transaction.Commit();
                        DeleteEventIfExist(eventDetails.UserID, eventDetails.BeginTime);
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
        /// Deletes work event from database.
        /// </summary>
        /// <param name="id">ID of work event.</param>
        /// <returns>True if work event was deleted; false, otherwise.</returns>
        public override bool DeleteEvent(int id)
        {
            WorkEventDetails deletedEvent = GetEventByID(id);

            if (deletedEvent != null)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString)
                    )
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText =
                        string.Format("DELETE FROM {0} WHERE ID = @ID", DBEventsTableName);
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                    try
                    {
                        bool result = (ExecuteNonQuery(command) == 1);

                        if (result)
                        {
                            transaction.Commit();
                            DeleteEventIfExist(deletedEvent.UserID, deletedEvent.BeginTime);
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

            return false;
        }

        /// <summary>
        /// Deletes all events of user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>True if events were deleted; false, otherwise.</returns>
        /// <remarks>Is user while user deleting.</remarks>
        public override bool DeleteUserEvents(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString)
                )
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE UserID = @UserID", DBEventsTableName);
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                try
                {
                    bool result = (ExecuteNonQuery(command) == 1);

                    if (result)
                    {
                        transaction.Commit();
                        m_Cache.Clear();
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
        /// Returns all work events for given date.
        /// <param name="userID">User ID.</param>
        /// <param name="date">Date.</param>
        /// </summary>
        /// <returns>Array of all work events for given date.</returns>
        public override WorkEventDetails[] GetEventsOfDate(int userID, DateTime date)
        {
            string key = GetCacheKey(userID, date);
            if (m_Cache.ContainsKey(key))
                return m_Cache[key].ToArray();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "SELECT * FROM {0} WHERE (UserID = @UserID) AND (BeginTime >= @BeginTime) AND (BeginTime <= @EndTime)",
                        DBEventsTableName);
                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    userID;
                command.Parameters.Add("@BeginTime", SqlDbType.DateTime).Value =
                    date.Date;
                command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value =
                    date.Date.AddDays(1).AddSeconds(-1);

                connection.Open();

                using (IDataReader eventsReader = ExecuteReader(command))
                {
                    List<WorkEventDetails> events = GetAllDetailsFromReader(eventsReader);

                    m_Cache[key] = events;

                    return events.ToArray();
                }
            }
        }

        /// <summary>
        /// Returns work event with given ID.
        /// </summary>
        /// <param name="id">ID of work event.</param>
        /// <returns>Work event with given ID. Null, otherwise.</returns>
        public override WorkEventDetails GetEventByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("SELECT * FROM {0} WHERE ID = @ID", DBEventsTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                connection.Open();
                using (IDataReader eventsReader = ExecuteReader(command))
                {
                    if (eventsReader.Read())
                    { return GetDetailsFromReader(eventsReader); }
                    else
                    { return null; }
                }
            }
        }

        /// <summary>
        /// Constructs cache key for given user and date.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="date">Date.</param>
        /// <returns>Cache key for given user and date.</returns>
        private static string GetCacheKey(int userID, DateTime date)
        {
            return string.Format("User {0} date {1}", userID, date.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Creates work event details from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Work event details.</returns>
        protected virtual WorkEventDetails GetDetailsFromReader(IDataReader reader)
        {
            WorkEventDetails details = new WorkEventDetails();

            details.ID = (int)reader["ID"];
            details.Name = (string)reader["Name"];
            details.BeginTime = (DateTime)reader["BeginTime"];
            details.EndTime = (DateTime)reader["EndTime"];
            details.Duration = (int)reader["Duration"];
            details.UserID = (int)reader["UserID"];
            details.ProjectID = (int)reader["ProjectID"];
            details.WorkCategoryID = (int)reader["WorkCategoryID"];
            details.UptimeEventTypeID = (int)reader["UptimeEventTypeID"];

            return details;
        }

        /// <summary>
        /// Returns all work event details from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>List of all work event details.</returns>
        protected virtual List<WorkEventDetails> GetAllDetailsFromReader(IDataReader reader)
        {
            List<WorkEventDetails> details = new List<WorkEventDetails>();
            while (reader.Read())
            {
                details.Add(GetDetailsFromReader(reader));
            }

            return details;
        }

        /// <summary>
        /// Checking if event exists in cahe than delete it.
        /// </summary>
        /// <param name="userID">User identificator.</param>
        /// <param name="dateTime">Date time.</param>
        private void DeleteEventIfExist(int userID, DateTime dateTime)
        {
            string key = GetCacheKey(userID, dateTime);
            if (m_Cache.ContainsKey(key))
                m_Cache.Remove(key);
        }
    }
}
