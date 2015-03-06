using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

using Core;
using Core.DB;
using Core.ORM;

using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.FiltersSupport;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of objects data for MS SQL Server.
    /// </summary>
    public class SqlRequestObjectsProvider : RequestObjectsProvider
    {
        #region [ Constants ]

        private readonly string DBRequestObjectTableName = "RequestObject";
        private readonly string DBRequestsTableName = "Requests";

        private const string DBThemesTableName = "Books_Themes";
        private const string DBBooksTableName = "Books_Books";
        private const string DBBookThemesTableName = "Books_BookThemes";
        
        private const string DBDisksTableName = "Disks";
        private const string DBCardsTableName = "DiscountCard";
        private const string DBUsersTableName = "Users";

        #endregion

        #region Properties
        public override string BooksTable
        {
            get
            {
                return DBBooksTableName;
            }
        }
        public override string DisksTable
        {
            get
            {
                return DBDisksTableName;
            }
        }
        public override string CardsTable
        {
            get
            {
                return DBCardsTableName;
            }
        }
        #endregion

       

       
       
        


       

        

        


        #region [ Request management ]

        public override void CreateRequest(int ObjectID, int? UserID, DateTime Date, bool IsTaken)
        {
            Procedure procedure = new Procedure("CreateRequest");
            if (UserID == null)
                procedure.Add("@UserID", DBNull.Value);
            else
                procedure.Add("@UserID", UserID);

            procedure.Add("@ObjectID", ObjectID);
            procedure.Add("@Date", Date);
            procedure.Add("@IsTaken", IsTaken);
            procedure.ExecNonQuery();
        }

        public override void DeleteAllObjectRequests(int ObjectID)
        {
            Query command = new Query(string.Format("DELETE FROM [{0}] WHERE ObjectID = @ObjectID", DBRequestsTableName));
            command.Add("@ObjectID", SqlDbType.Int).Value = ObjectID;
            command.ExecNonQuery();
        }

        public override string GetOwnerName(int ObjectID, out int? OwnerID)
        {
            string OwnerName = string.Empty;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.Append("SELECT FirstName, ");
                query.Append("CASE WHEN LastName IS NULL ");
                query.Append("THEN '<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
                query.Append("ELSE LastName END as LastName, ");
                query.AppendFormat("OwnerID FROM [{0}] ", DBRequestObjectTableName);
                query.AppendFormat("LEFT JOIN [{0}] ON [{1}].OwnerID = [{0}].ID ", DBUsersTableName, DBRequestObjectTableName);
                query.AppendFormat("WHERE [{0}].ID = @ObjectID ", DBRequestObjectTableName);

                command.CommandText = query.ToString();
                command.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID;

                using (IDataReader reader = ExecuteReader(command))
                {
                    reader.Read();
                    MLText firstname = new MLText();
                    MLText lastname = new MLText();

                    if (reader["FirstName"] != DBNull.Value)
                        firstname.LoadFromXML(reader["FirstName"].ToString());

                    lastname.LoadFromXML(reader["LastName"].ToString());

                    if (!string.IsNullOrEmpty(firstname.ToString()))
                        OwnerName += firstname.ToString() + " ";
                    OwnerName += lastname.ToString();

                    OwnerID = (reader["OwnerID"] != DBNull.Value)
                                                    ? (int?)reader["OwnerID"] : null;
                }
            }
            return OwnerName;
        }

        public override string GetHolderName(int ObjectID, out int? HolderID)
        {
            string HolderName = string.Empty;

            StringBuilder queryText = new StringBuilder();
            queryText.Append("DECLARE @HolderID int ");
            queryText.Append("SET @HolderID = [dbo].GetRequestObjectHolderID(@ObjectID) ");
            queryText.Append("IF (@HolderID IS NULL) ");
            queryText.Append("BEGIN ");
            queryText.Append("SELECT NULL as FirstName, ");
            queryText.Append("'<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
            queryText.Append("as LastName, null as holderID ");
            queryText.Append("END ");
            queryText.Append("ELSE ");
            queryText.Append("BEGIN ");
            queryText.Append("SELECT FirstName, LastName, ID as holderID ");
            queryText.AppendFormat("FROM [{0}] ", DBUsersTableName);
            queryText.AppendFormat("WHERE [{0}].ID = @HolderID ", DBUsersTableName);
            queryText.Append("END ");

            Query query = new Query(queryText.ToString());
            query.Add("@ObjectID", ObjectID);

            using (IDataReader reader = query.ExecReader())
            {
                reader.Read();
                MLText firstname = new MLText();
                MLText lastname = new MLText();

                if (reader["FirstName"] != DBNull.Value)
                    firstname.LoadFromXML(reader["FirstName"].ToString());
                lastname.LoadFromXML(reader["LastName"].ToString());

                if (!string.IsNullOrEmpty(firstname.ToString()))
                    HolderName += firstname.ToString() + " ";
                HolderName += lastname.ToString();

                if (reader["holderID"] != DBNull.Value)
                    HolderID = (int)reader["holderID"];
                else
                    HolderID = null;
            }
            return HolderName;
        }

        #endregion

        #region [ RequestObjects support ]

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string ensureValidSortExpression(string sortExpr, Type filterType)
        {
            if (string.IsNullOrEmpty(sortExpr))
                return "Title";

            var sortingExpr = sortExpr.Split(' ')[0].Trim().ToLower();
            if (!sortingExpr.StartsWith("title"))
                sortExpr += ", Title";

            return sortExpr;
        }

        private static List<string> getPropertiesNames(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => prop.Name).ToList<string>();
        }

        #region [ GetFilteredRequestObjects ]

       

        #endregion

        #endregion

        #region [ RequestObject History support ]

        public override DataSet GetRequestObjectHistory(int objectID, string sortExpr, int maximumRows, int startRowIndex)
        {
            int lowerBound = startRowIndex + 1;
            int upperBound = startRowIndex + maximumRows;

            if (string.IsNullOrEmpty(sortExpr))
                sortExpr = "Date";

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT [{0}].ID, [{1}].FirstName, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum, "
                , DBRequestsTableName, DBUsersTableName, sortExpr);
            sbQuery.AppendFormat("CASE WHEN [{0}].LastName is null ", DBUsersTableName);
            sbQuery.Append("THEN '<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
            sbQuery.AppendFormat("ELSE [{0}].LastName ", DBUsersTableName);
            sbQuery.AppendFormat("END as LastName, [{0}].Date, [{0}].IsTaken ", DBRequestsTableName);
            sbQuery.AppendFormat("FROM [{0}] ", DBRequestsTableName);
            sbQuery.AppendFormat("LEFT JOIN [{0}] ON [{1}].UserId = [{0}].ID ", DBUsersTableName, DBRequestsTableName);
            sbQuery.AppendFormat("WHERE [{0}].ObjectID = {1} ", DBRequestsTableName, objectID);

            string commandText = string.Format("SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC"
                                                    , sbQuery.ToString(), lowerBound, upperBound);
            var command = new Query(commandText);
            return command.ExecDataSet();
        }

        public override int GetRequestObjectHistoryCount(int objectID)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT COUNT(*) FROM {0} ", DBRequestsTableName);
            sbQuery.AppendFormat("WHERE ObjectID = {0} ", objectID);

            var query = new Query(sbQuery.ToString());
            return (int)query.ExecScalar();
        }

        #endregion

       
    }
}