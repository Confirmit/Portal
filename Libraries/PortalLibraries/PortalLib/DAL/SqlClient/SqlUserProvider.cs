using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using Core;
using Core.ORM;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of users data for MS SQL Server.
    /// </summary>
    public class SqlUsersProvider : UsersProvider
    {   
        #region Fields

        private readonly string DBUsersTableName = "Users";
        private readonly string DBPersonGroupTable = "Person2Group";
        private readonly string DBEventsTable = "UptimeEvents";

        // События, которые хранятся в БД как закрытые, то есть BeginTime != EndTime
        // 11 - Ill
        // 12 - Business Trip
        // 13 - Vacation
        // 14 - Trust Ill
        private readonly IList<int> m_closeUptimeEvents = new List<int> {11, 12, 13, 14};
        
        #endregion

        public override IList<Person> GetFilteredUsers(string SortExpression, int maximumRows, int startRowIndex, PersonsFilter filter)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SortExpression = EnsureValidSortExpression(SortExpression);
                SqlCommand getCommand = connection.CreateCommand();
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string strSubCommand = string.Format(
                        "SELECT * FROM {0} A {1} ",
                        DBUsersTableName,
                        constructWhereClause(filter));

                    strSubCommand = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS RowNum,* FROM ({0}) AS TMP",
                                                  strSubCommand, SortExpression);

                    int lowerBound = startRowIndex + 1;
                    int upperBound = startRowIndex +  maximumRows;

                    getCommand.Transaction = transaction;
                    getCommand.CommandText =
                        string.Format(
                            "SELECT * FROM ({0}) as temp WHERE temp.RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC",
                            strSubCommand, lowerBound, upperBound);

                    using (IDataReader reader = getCommand.ExecuteReader())
                    {
                        return GetAllUserDetailsFromReader(reader);
                    }
                }
                finally
                {
                    transaction.Rollback();
                }
            }
        }

        public override int GetFilteredUsersCount(PersonsFilter filter)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand getCommand = connection.CreateCommand();
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" SELECT COUNT(*) ");
                    query.AppendFormat(" FROM {0} A", DBUsersTableName);
                    query.Append(constructWhereClause(filter));

                    getCommand.Transaction = transaction;
                    getCommand.CommandText = query.ToString();

                    return (int)getCommand.ExecuteScalar();
                }
                finally
                {
                    transaction.Rollback();
                }
            }
        }

        protected virtual IList<Person> GetAllUserDetailsFromReader(IDataReader reader)
        {
            IList<Person> users = new List<Person>();
            while (reader.Read())
            {
                users.Add(GetUserDetailsFromReader(reader));
            }

            return users;
        }

        protected virtual Person GetUserDetailsFromReader(IDataReader reader)
        {
            int userID = (int) reader["ID"];
            Person user = new Person();
            user.Load(userID);

            return user;
        }

        #region Filter support

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpression">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string EnsureValidSortExpression(string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                sortExpression = "LastName_ru";

            sortExpression = sortExpression.Trim().ToLower();
            var allowableSortExpressions = new List<string>()
            {
                "firstname_en",
                "firstname_ru",
                "lastname_en",
                "lastname_ru",
                "middlename_en",
                "middlename_ru",
            };
            if (!allowableSortExpressions.Contains(sortExpression))
                sortExpression = "LastName_ru";

            return sortExpression;
        }

        private StringBuilder constructWhereClause(PersonsFilter filter)
        {
            #region TODO
            /*if (filter.ProjectID != -1)
            {
                if (!string.IsNullOrEmpty(whereClause.ToString()))
                    whereClause.Append(" AND ");

                whereClause.AppendFormat(" C.ProjectID = '{0}' ",
                                         filter.ProjectID);
            }*/
            #endregion

            string personBaseClause = getBasePersonClause(filter);
            string groupClause = getGroupFilter(filter);
            string uptimeEventsClause = getUptimeEventsFilter(filter);
            string uptimeEvents_NOT_IN_Clause = getUptimeEvents_NOT_IN_Filter(filter);

            if (string.IsNullOrEmpty(groupClause)
                && string.IsNullOrEmpty(uptimeEventsClause)
                && string.IsNullOrEmpty(personBaseClause))
                return new StringBuilder();

            StringBuilder whereClause = new StringBuilder();

            // construct basic part of filter like firstnaem, surname, role etc.
            if (!string.IsNullOrEmpty(personBaseClause)
                || !string.IsNullOrEmpty(groupClause))
            {
                StringBuilder basicFilterClause = new StringBuilder();
                if (!string.IsNullOrEmpty(personBaseClause))
                    basicFilterClause.Append(personBaseClause);

                if (!string.IsNullOrEmpty(groupClause))
                {
                    if (!string.IsNullOrEmpty(personBaseClause))
                        basicFilterClause.Append(" AND ");

                    basicFilterClause.AppendFormat(" A.ID IN ({0}) ", groupClause);
                }

                whereClause.AppendFormat(" ({0}) ", basicFilterClause);
            }

            // construct uptimeEvent part of filter.
            StringBuilder fullUptimeEventClause = new StringBuilder();
            if (!string.IsNullOrEmpty(uptimeEventsClause))
            {
                if (!string.IsNullOrEmpty(uptimeEvents_NOT_IN_Clause))
                    fullUptimeEventClause.AppendFormat(" A.ID NOT IN ({0}) OR ",
                                                       uptimeEvents_NOT_IN_Clause);

                if (!string.IsNullOrEmpty(uptimeEventsClause))
                    fullUptimeEventClause.AppendFormat(" A.ID IN ({0}) ", uptimeEventsClause);
            }

            if (!string.IsNullOrEmpty(fullUptimeEventClause.ToString()))
            {
                if (!string.IsNullOrEmpty(whereClause.ToString()))
                    whereClause.Append(" AND ");

                whereClause.AppendFormat(" ({0}) ", fullUptimeEventClause);
            }

            return new StringBuilder(" WHERE ").Append(whereClause);
        }

        private string getBasePersonClause(PersonsFilter filter)
        {
            StringBuilder whereClause = new StringBuilder();

            if (filter.OfficeID > 0)
            {
                var employeesUlterSysMoscow = (filter.OfficeID == 2);
                if (!string.IsNullOrEmpty(whereClause.ToString()))
                    whereClause.Append(" AND ");

                whereClause.AppendFormat(" EmployeesUlterSYSMoscow = '{0}' ",
                                         employeesUlterSysMoscow);
            }

            if (filter.ProjectID != -1)
            {
                if (!string.IsNullOrEmpty(whereClause.ToString()))
                    whereClause.Append(" AND ");

                whereClause.AppendFormat(" C.ProjectID = '{0}' ",
                                         filter.ProjectID);
            }

            AppendMLStringClause(filter.FirstName, whereClause, "FirstName");
            AppendMLStringClause(filter.LastName, whereClause, "LastName");

            return whereClause.ToString();
        }

        private void AppendMLStringClause(MLString mlString, StringBuilder whereClause, string columnNameWithoutPrefix)
        {
            if (!string.IsNullOrEmpty(mlString.ToString()))
            {
                if (!string.IsNullOrEmpty(whereClause.ToString()))
                    whereClause.Append(" AND ");

                if (CultureManager.CurrentLanguage == CultureManager.Languages.Russian)
                {
                    whereClause.AppendFormat(" A.{0}{1} LIKE '{2}' ", columnNameWithoutPrefix, ObjectMapper.RussianEnding,
                        mlString.RussianValue);
                }
                else
                    whereClause.AppendFormat(" A.{0}{1} LIKE '{2}' ", columnNameWithoutPrefix, ObjectMapper.EnglishEnding,
                       mlString.EnglishValue);
            }
        }

        /// <summary>
        /// Get persons group filter.
        /// </summary>
        /// <param name="filter">Persons filter class.</param>
        /// <returns>String of person group clause.</returns>
        private string getGroupFilter(PersonsFilter filter)
        {
            StringBuilder groupClause = new StringBuilder();
            if (filter.RoleID != -1)
            {
                groupClause.Append(" SELECT B.PersonID ");
                groupClause.AppendFormat(" FROM {0} B ", DBPersonGroupTable);
                groupClause.AppendFormat(" WHERE B.GroupID = '{0}' ", filter.RoleID);
            }
            return groupClause.ToString();
        }

        /// <summary>
        /// Get persons uptime events which will ignore.
        /// </summary>
        /// <param name="filter">Persons filter class.</param>
        /// <returns>String of ignored person uptime events clause.</returns>
        private string getUptimeEvents_NOT_IN_Filter(PersonsFilter filter)
        {
            if (filter.Events == null
                || filter.Events.Count == 0
                || !filter.Events.Contains(9))
                return string.Empty;

            StringBuilder uptimeEvents_NOT_IN_Clause = new StringBuilder();

            uptimeEvents_NOT_IN_Clause.Append(" SELECT C.UserID ");
            uptimeEvents_NOT_IN_Clause.AppendFormat(" FROM {0} C ", DBEventsTable);
            uptimeEvents_NOT_IN_Clause.Append(" WHERE ");
            //uptimeEvents_NOT_IN_Clause.Append(" C.BeginTime = C.EndTime ");
            //uptimeEvents_NOT_IN_Clause.Append(" AND ");
            uptimeEvents_NOT_IN_Clause.Append(
                " CONVERT(varchar(10),C.BeginTime,104) = CONVERT(varchar(10),GETDATE(),104) ");

            return uptimeEvents_NOT_IN_Clause.ToString();
        }

        /// <summary>
        /// Get persons uptime events filter.
        /// </summary>
        /// <param name="filter">Persons filter class.</param>
        /// <returns>String of person uptime events clause.</returns>
        private string getUptimeEventsFilter(PersonsFilter filter)
        {
            if (filter.Events == null || filter.Events.Count == 0)
                return string.Empty;

            StringBuilder uptimeEventsClause = new StringBuilder();
            string openEventsIDs = string.Empty;
            string closeEventsIDs = string.Empty;

            foreach (int uptimeEventID in filter.Events)
            {
                if (!string.IsNullOrEmpty(openEventsIDs))
                    openEventsIDs += ", ";

                if (!string.IsNullOrEmpty(closeEventsIDs))
                    closeEventsIDs += ", ";

                if (m_closeUptimeEvents.Contains(uptimeEventID))
                    closeEventsIDs += uptimeEventID.ToString();
                else
                    openEventsIDs += uptimeEventID.ToString();
            }

            uptimeEventsClause.Append(" SELECT C.UserID ");
            uptimeEventsClause.AppendFormat(" FROM {0} C ", DBEventsTable);
            uptimeEventsClause.AppendFormat(" WHERE ");

            if (!string.IsNullOrEmpty(openEventsIDs))
            {
                uptimeEventsClause.AppendFormat(" ( ");
                uptimeEventsClause.AppendFormat(" C.UptimeEventTypeID IN ({0}) ", openEventsIDs);
                uptimeEventsClause.Append(" AND C.BeginTime = C.EndTime ");
                uptimeEventsClause.AppendFormat(" ) ");
            }

            if (!string.IsNullOrEmpty(closeEventsIDs))
            {
                if (!string.IsNullOrEmpty(openEventsIDs))
                    uptimeEventsClause.AppendFormat(" OR ");

                uptimeEventsClause.AppendFormat(" ( ");
                uptimeEventsClause.AppendFormat(" C.UptimeEventTypeID IN ({0}) ", closeEventsIDs);
                uptimeEventsClause.AppendFormat(" ) ");
            }

            uptimeEventsClause.Append(" AND ");
            uptimeEventsClause.Append(" CONVERT(varchar(10),C.BeginTime,104) = CONVERT(varchar(10),GETDATE(),104) ");

            return uptimeEventsClause.ToString();
        }

        #endregion
    }
}
