using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of rojects data for MS SQL Server.
    /// </summary>
    public class SqlProjectProvider : ProjectProvider
    {
        #region Constants

        private const string DBProjectsTableName = "Projects";
        private const string DBProjectsUserTableName = "ProjectUser";
        
        #endregion    

        /// <summary>
        /// Returns all project for current user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        public override IList<Project> GetAllUserProjects(int userID)
        {
            return getUserProjects(userID, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="returnCompleteProjects">указывает, какие проекты хотим вернуть (завершенные/незавершенные)</param>
        public override IList<Project> GetUserProjects(int userID, bool returnCompleteProjects)
        {
            return getUserProjects(userID,
                                   String.Format("{0}.UserInProject = {1}",
                                                 DBProjectsUserTableName,
                                                 returnCompleteProjects ? 1 : 0));
        }

        private IList<Project> getUserProjects(int userID, String condition)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                String subQuery = String.IsNullOrEmpty(condition)
                                      ? String.Empty
                                      : String.Format(" AND {0}", condition);

                StringBuilder query = new StringBuilder();
                query.AppendFormat(" SELECT {0}.ID, {0}.Name, {0}.Description", DBProjectsTableName);
                query.AppendFormat(" FROM {0} INNER JOIN {1} ON {0}.ID = {1}.ProjectID", DBProjectsTableName, DBProjectsUserTableName);
                query.AppendFormat(" WHERE {0}.UserID = @userID {1}",
                                   DBProjectsUserTableName,
                                   subQuery);
                query.AppendFormat(" ORDER BY {0}.Name", DBProjectsTableName);

                command.CommandText = query.ToString();
                command.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                command.CommandText = query.ToString();

                using (IDataReader reader = ExecuteReader(command))
                {
                    return getAllProjectsDataFromReader(reader);
                }
            }
        }

        private IList<Project> getAllProjectsDataFromReader(IDataReader reader)
        {
            IList<Project> list = new List<Project>();
            while (reader.Read())
            {
                list.Add(new Project
                             {
                                 ID = (int) reader["ID"],
                                 Name = (string) reader["Name"],
                                 Description = reader["Description"] == DBNull.Value
                                                   ? null
                                                   : (string) reader["Description"]
                             });
            }

            return list;
        }
    }
}