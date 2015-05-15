using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
	/// <summary>
	/// Provider of ability data for MS SQL Server.
	/// </summary>
	public class SqlAbilityProvider : AbilityProvider
	{
        #region Constants

        private const string DBAbilityTableName = "Ability";
        private const string DBAbilityUserTableName = "AbilityUser";
        
        #endregion

        /// <summary>
        /// Creates new ability in database.
        /// </summary>
        /// <param name="abilityname">Name of ability.</param>
        /// <returns>ID of new database record.</returns>
        public override int CreateAbility(string abilityname)
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
                        "INSERT INTO {0} (Name) VALUES  (@Ability) SELECT @@IDENTITY",
                        DBAbilityTableName);
                command.Parameters.Add("@Ability", SqlDbType.NVarChar).Value =
                    abilityname;
                try
                {
                    id = Convert.ToInt32(ExecuteScalar(command));
                    transaction.Commit();
                }
                catch
                { transaction.Rollback(); }
            }
            return id;		
        }

        /// <summary>
        /// Returns all abilities for current user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        public override IList<Ability> GetAllUserAbilities(int userID)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                StringBuilder query = new StringBuilder();
                query.AppendFormat(" SELECT {0}.ID, {0}.Name", DBAbilityTableName);
                query.AppendFormat(" FROM {0}, {1}", DBAbilityTableName, DBAbilityUserTableName);
                query.AppendFormat(" WHERE {0}.PersonID = @userID AND {0}.AbilityID = {1}.ID", DBAbilityUserTableName, DBAbilityTableName);
                query.AppendFormat(" ORDER BY {0}.Name", DBAbilityTableName);

                command.CommandText = query.ToString();
                command.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllAbilitiesDataFromReader(reader);
                }
            }
        }

        private IList<Ability> GetAllAbilitiesDataFromReader(IDataReader reader)
        {
            IList<Ability> list = new List<Ability>();
            while (reader.Read())
            {
                list.Add(new Ability
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"]
                });
            }

            return list;
        }
    }
}
