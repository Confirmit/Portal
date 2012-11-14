using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.Text;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of roles data for MS SQL Server.
    /// </summary>
    public class SqlRolesProvider : RolesProvider
    {
        #region Fields
        private RolesProvider m_Provider;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlRolesProvider()
        {
            if (Globals.Settings.Roles.CacheEnabled)
                m_Provider = new SqlRolesProviderWithCache();
            else
                m_Provider = new SqlRolesProviderWithoutCache();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds users to roles.
        /// </summary>
        /// <param name="usersIDs">Array of user IDs.</param>
        /// <param name="roleIDs">Array of role string IDs.</param>
        public override void AddUsersToRoles(int[] usersIDs, string[] roleIDs)
        {
            m_Provider.AddUsersToRoles(usersIDs, roleIDs);
        }

        public override List<RoleDetails> GetAllRoleDetailsFromReader(IDataReader reader)
        {
           return m_Provider.GetAllRoleDetailsFromReader(reader);
        }

        /// <summary>
        /// Creates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>ID of new role.</returns>
        public override int CreateRole(RoleDetails role)
        {
            return m_Provider.CreateRole(role);
        }

        /// <summary>
        /// Updates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>True if role was successfully updated; false, otherwise.</returns>
        public override bool UpdateRole(RoleDetails role)
        {
            return m_Provider.UpdateRole(role);
        }

        /// <summary>
        /// Deletes role.
        /// </summary>
        /// <param name="id">ID of role.</param>
        /// <returns>True if role was successfully deleted; false, otherwise.</returns>
        public override bool DeleteRole(int id)
        {
            return m_Provider.DeleteRole(id);
        }

        /// <summary>
        /// Returns array of all roles.
        /// </summary>
        /// <returns>Array of all roles.</returns>
        public override RoleDetails[] GetAllRoles()
        {
            return m_Provider.GetAllRoles();
        }

        /// <summary>
        /// Returns all roles of given user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Array of all roles of given user.</returns>
        public override RoleDetails[] GetRolesForUser(int userID)
        {
            return m_Provider.GetRolesForUser(userID);
        }

        /// <summary>
        /// Returns IDs of all users in given role.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>Array of IDs of all users in given role.</returns>
        public override int[] GetUsersInRole(string roleID)
        {
            return m_Provider.GetUsersInRole(roleID);
        }

        /// <summary>
        /// Is given user in given role.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if user is in role; false, otherwise.</returns>
        public override bool IsUserInRole(int userID, string roleID)
        {
            return m_Provider.IsUserInRole(userID, roleID);
        }

        /// <summary>
        /// Removes users from roles.
        /// </summary>
        /// <param name="usersIDs">IDs of users.</param>
        /// <param name="roleIDs">Role string IDs.</param>
        public override void RemoveUsersFromRoles(int[] usersIDs, string[] roleIDs)
        {
            m_Provider.RemoveUsersFromRoles(usersIDs, roleIDs);
        }

        /// <summary>
        /// Does given role exist.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if role exists; false, otherwise.</returns>
        public override bool RoleExists(string roleID)
        {
            return m_Provider.RoleExists(roleID);
        }
        #endregion
    }

    /// <summary>
    /// Provider of roles data for MS SQL Server using caches.
    /// </summary>
    public class SqlRolesProviderWithCache : RolesProvider
    {
        #region Constants
        private const string DBRolesTableName = "Groups";
        private const string DBUserToRoleTableName = "Person2Group";
        #endregion

        #region Fields

        private readonly Dictionary<int, RoleDetails> m_IDToRoleMap = new Dictionary<int, RoleDetails>();
        private readonly Dictionary<string, RoleDetails> m_RoleIDToRoleMap = new Dictionary<string, RoleDetails>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<int, List<int>> m_UserToRoleMap = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> m_RoleToUserMap = new Dictionary<int, List<int>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlRolesProviderWithCache()
        {
            Initialize();
        }

        #endregion

        #region Initialization method

        /// <summary>
        /// Initializes information about roles.
        /// </summary>
        private void Initialize()
        {
            LoadRolesDictioanry();
            LoadUsersRolesDictioanry();
        }

        #region Loading dictionaries

        /// <summary>
        /// Loads all roles from database into cache.
        /// </summary>
        private void LoadRolesDictioanry()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand rolesCommand = connection.CreateCommand();
                rolesCommand.CommandText = string.Format("SELECT * FROM {0}", DBRolesTableName);

                connection.Open();
                using (IDataReader rolesReader = ExecuteReader(rolesCommand))
                {
                    List<RoleDetails> roles = GetAllRoleDetailsFromReader(rolesReader);
                    foreach (RoleDetails role in roles)
                    {
                        m_IDToRoleMap[role.ID] = role;
                        m_RoleIDToRoleMap[role.RoleID] = role;
                    }
                }
            }
        }

        /// <summary>
        /// Loads all roles of all users into cache.
        /// </summary>
        private void LoadUsersRolesDictioanry()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand userToRolesCommand = connection.CreateCommand();
                userToRolesCommand.CommandText = string.Format("SELECT * FROM {0}", DBUserToRoleTableName);

                connection.Open();
                using (IDataReader userToRoleReader = ExecuteReader(userToRolesCommand))
                {
                    while (userToRoleReader.Read())
                    {
                        int userID = (int)userToRoleReader["PersonID"];
                        int roleID = (int)userToRoleReader["GroupID"];

                        if (!m_UserToRoleMap.ContainsKey(userID))
                            m_UserToRoleMap[userID] = new List<int>();

                        if (!m_UserToRoleMap[userID].Contains(roleID))
                            m_UserToRoleMap[userID].Add(roleID);

                        if (!m_RoleToUserMap.ContainsKey(roleID))
                            m_RoleToUserMap[roleID] = new List<int>();

                        if (!m_RoleToUserMap[roleID].Contains(userID))
                            m_RoleToUserMap[roleID].Add(userID);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region RolesProvider methods

        /// <summary>
        /// Adds users to roles.
        /// </summary>
        /// <param name="usersIDs">Array of user IDs.</param>
        /// <param name="roleIDs">Array of role string IDs.</param>
        public override void AddUsersToRoles(int[] usersIDs, string[] roleIDs)
        {
            foreach (int userID in usersIDs)
            {
                foreach (string roleID in roleIDs)
                {
                    if (!m_RoleIDToRoleMap.ContainsKey(roleID))
                        continue;

                    RoleDetails role = m_RoleIDToRoleMap[roleID];

                    if (m_UserToRoleMap.ContainsKey(userID)
                        && m_UserToRoleMap[userID].Contains(role.ID))
                    {
                        continue;
                    }

                    using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = string.Format(
                            "INSERT INTO {0} (PersonID, GroupID) VALUES  (@PersonID, @GroupID)",
                            DBUserToRoleTableName);
                        command.Parameters.Add("@PersonID", SqlDbType.Int).Value =
                            userID;
                        command.Parameters.Add("@GroupID", SqlDbType.Int).Value =
                            role.ID;
                        connection.Open();

                        if (ExecuteNonQuery(command) == 1)
                        {
                            if (!m_UserToRoleMap.ContainsKey(userID))
                                m_UserToRoleMap[userID] = new List<int>();

                            if (!m_UserToRoleMap[userID].Contains(role.ID))
                                m_UserToRoleMap[userID].Add(role.ID);

                            if (!m_RoleToUserMap.ContainsKey(role.ID))
                                m_RoleToUserMap[role.ID] = new List<int>();

                            if (!m_RoleToUserMap[role.ID].Contains(userID))
                                m_RoleToUserMap[role.ID].Add(userID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>ID of new role.</returns>
        public override int CreateRole(RoleDetails role)
        {
            if (m_RoleIDToRoleMap.ContainsKey(role.RoleID))
                throw new ArgumentException(
                    string.Format("Role with ID '{0}' already exists.", role.RoleID));

            int id = -1;
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
                    string.Format(
                        "INSERT INTO {0} (GroupID, Name, Description) VALUES  (@GroupID, @Name, @Description) SELECT @@IDENTITY",
                        DBRolesTableName);
                command.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value =
                    role.RoleID;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    role.Name;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                    role.Description;

                try
                {
                    id = Convert.ToInt32(ExecuteScalar(command));
                    transaction.Commit();
                    role.ID = id;
                    m_IDToRoleMap[id] = role;
                    m_RoleIDToRoleMap[role.RoleID] = role;
                }
                catch
                { transaction.Rollback(); }
            }
            return id;
        }

        /// <summary>
        /// Updates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>True if role was successfully updated; false, otherwise.</returns>
        public override bool UpdateRole(RoleDetails role)
        {
            if (!m_IDToRoleMap.ContainsKey(role.ID))
                throw new ArgumentException(string.Format("Role with ID {0} is not in database.", role.ID));

            RoleDetails oldRole = m_IDToRoleMap[role.ID];

            // check if another role with the same roleID exists.
            foreach (RoleDetails roleDetails in GetAllRoles())
            {
                if (roleDetails.ID == oldRole.ID)
                    continue;

                if (roleDetails.RoleID == role.RoleID)
                    throw new Exception(string.Format("Role with RoleID '{0}' already exists.", role.RoleID));
            }

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "UPDATE {0} SET GroupID=@GroupID, Name=@Name, Description=@Description WHERE ID=@ID",
                        DBRolesTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = role.ID;
                command.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value =
                    role.RoleID;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    role.Name;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                    role.Description;
                connection.Open();
                bool result = (ExecuteNonQuery(command) == 1);

                // Update cache.
                m_IDToRoleMap[role.ID] = role;
                m_RoleIDToRoleMap.Remove(oldRole.RoleID);
                m_RoleIDToRoleMap[role.RoleID] = role;

                return result;
            }
        }

        /// <summary>
        /// Deletes role.
        /// </summary>
        /// <param name="id">ID of role.</param>
        /// <returns>True if role was successfully deleted; false, otherwise.</returns>
        public override bool DeleteRole(int id)
        {
            if (!m_IDToRoleMap.ContainsKey(id))
                throw new ArgumentException(string.Format("Role with ID {0} is not in database.", id));

            RoleDetails role = m_IDToRoleMap[id];

            int[] userIDs = GetUsersInRole(role.RoleID);
            RemoveUsersFromRoles(userIDs, new string[] { role.RoleID });

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("DELETE FROM {0} WHERE ID = @ID", DBRolesTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                connection.Open();

                bool result = (ExecuteNonQuery(command) == 1);
                if (result)
                {
                    m_IDToRoleMap.Remove(id);
                    m_RoleIDToRoleMap.Remove(role.RoleID);

                    if (m_RoleToUserMap.ContainsKey(id))
                        m_RoleToUserMap.Remove(id);
                    foreach (KeyValuePair<int, List<int>> pair in m_UserToRoleMap)
                    {
                        if (pair.Value.Contains(id))
                            pair.Value.Remove(id);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Returns array of all roles.
        /// </summary>
        /// <returns>Array of all roles.</returns>
        public override RoleDetails[] GetAllRoles()
        {
            RoleDetails[] roles = new RoleDetails[m_IDToRoleMap.Count];
            m_IDToRoleMap.Values.CopyTo(roles, 0);
            return roles;
        }

        /// <summary>
        /// Returns all roles of given user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Array of all roles of given user.</returns>
        public override RoleDetails[] GetRolesForUser(int userID)
        {
            if (!m_UserToRoleMap.ContainsKey(userID))
                return new RoleDetails[0];

            List<int> roleIDs = m_UserToRoleMap[userID];
            List<RoleDetails> roles = new List<RoleDetails>(roleIDs.Count);
            foreach (int roleID in roleIDs)
            {
                roles.Add(m_IDToRoleMap[roleID]);
            }

            return roles.ToArray();
        }

        /// <summary>
        /// Returns IDs of all users in given role.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>Array of IDs of all users in given role.</returns>
        public override int[] GetUsersInRole(string roleID)
        {
            if (!m_RoleIDToRoleMap.ContainsKey(roleID))
                return new int[0];

            RoleDetails role = m_RoleIDToRoleMap[roleID];

            if (!m_RoleToUserMap.ContainsKey(role.ID))
                return new int[0];

            return m_RoleToUserMap[role.ID].ToArray();
        }

        /// <summary>
        /// Is given user in given role.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if user is in role; false, otherwise.</returns>
        public override bool IsUserInRole(int userID, string roleID)
        {
            if (!m_UserToRoleMap.ContainsKey(userID))
                return false;

            List<int> userRolesIDs = m_UserToRoleMap[userID];

            if (!m_RoleIDToRoleMap.ContainsKey(roleID))
                return false;

            RoleDetails role = m_RoleIDToRoleMap[roleID];

            return userRolesIDs.Contains(role.ID);
        }

        /// <summary>
        /// Removes users from roles.
        /// </summary>
        /// <param name="usersIDs">IDs of users.</param>
        /// <param name="roleIDs">Role string IDs.</param>
        public override void RemoveUsersFromRoles(int[] usersIDs, string[] roleIDs)
        {
            foreach (string roleID in roleIDs)
            {
                if (!m_RoleIDToRoleMap.ContainsKey(roleID))
                    continue;

                RoleDetails role = m_RoleIDToRoleMap[roleID];

                foreach (int userID in usersIDs)
                {
                    if (m_RoleToUserMap.ContainsKey(role.ID))
                    {
                        if (m_RoleToUserMap[role.ID].Contains(userID))
                        {
                            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                            {
                                SqlCommand command = connection.CreateCommand();
                                command.CommandText = string.Format("DELETE FROM {0} WHERE (PersonID=@PersonID) AND (GroupID=@GroupID)", DBUserToRoleTableName);
                                command.Parameters.Add("@PersonID", SqlDbType.Int).Value =
                                    userID;
                                command.Parameters.Add("@GroupID", SqlDbType.Int).Value =
                                    role.ID;
                                connection.Open();
                                if (ExecuteNonQuery(command) == 1)
                                {
                                    m_RoleToUserMap[role.ID].Remove(userID);
                                    m_UserToRoleMap[userID].Remove(role.ID);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Does given role exist.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if role exists; false, otherwise.</returns>
        public override bool RoleExists(string roleID)
        {
            return m_RoleIDToRoleMap.ContainsKey(roleID);
        }

        #endregion

        #region Support methods

        /// <summary>
        /// Returns role information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Role information from reader.</returns>
        protected virtual RoleDetails GetRoleDetailsFromReader(IDataReader reader)
        {
            RoleDetails details = new RoleDetails();

            details.ID = (int)reader["ID"];
            details.RoleID = (string)reader["GroupID"];
            details.Name = (string)reader["Name"];
            details.Description = (string)reader["Description"];

            return details;
        }

        /// <summary>
        /// Returns role information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Role information from reader.</returns>
        public override List<RoleDetails> GetAllRoleDetailsFromReader(IDataReader reader)
        {
            List<RoleDetails> roles = new List<RoleDetails>();

            while (reader.Read())
            {
                roles.Add(GetRoleDetailsFromReader(reader));
            }

            return roles;
        }

        #endregion
    }

    /// <summary>
    /// Provider of roles data for MS SQL Server without cache.
    /// </summary>
    public class SqlRolesProviderWithoutCache : RolesProvider
    {
        #region Constants
        private const string DBRolesTableName = "Groups";
        private const string DBUserToRoleTableName = "Person2Group";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlRolesProviderWithoutCache()
        { }
        #endregion

        #region RolesProvider methods
        /// <summary>
        /// Adds users to roles.
        /// </summary>
        /// <param name="usersIDs">Array of user IDs.</param>
        /// <param name="roleIDs">Array of role string IDs.</param>
        public override void AddUsersToRoles(int[] usersIDs, string[] roleIDs)
        {
            if (roleIDs == null)
                return;
            if (usersIDs == null)
                return;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                foreach (string roleID in roleIDs)
                {
                    int groupID = GetIDOfRole(roleID);
                    if (groupID == -1)
                        continue;

                    foreach (int userID in usersIDs)
                    {
                        if (IsUserInRole(userID, roleID))
                            continue;

                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = string.Format(
                            "INSERT INTO {0} (PersonID, GroupID) VALUES  (@PersonID, @GroupID)",
                            DBUserToRoleTableName);
                        command.Parameters.Add("@PersonID", SqlDbType.Int).Value =
                            userID;
                        command.Parameters.Add("@GroupID", SqlDbType.Int).Value =
                            groupID;

                        ExecuteNonQuery(command);
                    }
                }
            }
        }

        /// <summary>
        /// Creates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>ID of new role.</returns>
        /// <exception cref="ArgumentNullException">Role is null.</exception>
        /// <exception cref="ArgumentException">Role already exists.</exception>
        public override int CreateRole(RoleDetails role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (RoleExists(role.RoleID))
                return -1;

            int id = -1;
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "INSERT INTO {0} (GroupID, Name, Description) VALUES  (@GroupID, @Name, @Description) SELECT @@IDENTITY",
                        DBRolesTableName);
                command.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value =
                    role.RoleID;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    role.Name;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                    role.Description;

                id = Convert.ToInt32(ExecuteScalar(command));
                role.ID = id;
            }
            return id;
        }

        /// <summary>
        /// Updates new role.
        /// </summary>
        /// <param name="role">Role.</param>
        /// <returns>True if role was successfully updated; false, otherwise.</returns>
        public override bool UpdateRole(RoleDetails role)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format(
                        "UPDATE {0} SET GroupID=@GroupID, Name=@Name, Description=@Description WHERE ID=@ID",
                        DBRolesTableName);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = role.ID;
                command.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value =
                    role.RoleID;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value =
                    role.Name;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                    role.Description;
                connection.Open();
                return (ExecuteNonQuery(command) == 1);
            }
        }

        /// <summary>
        /// Deletes role.
        /// </summary>
        /// <param name="id">ID of role.</param>
        /// <returns>True if role was successfully deleted; false, otherwise.</returns>
        public override bool DeleteRole(int id)
        {
            RoleDetails role = null;
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM {0} WHERE ID = @ID", DBRolesTableName);
                command.Parameters.AddWithValue("@ID", id);

                using (IDataReader reader = ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        role = GetRoleDetailsFromReader(reader);
                    }
                }
            }

            if (role == null)
                return false;

            int[] usersIDs = GetUsersInRole(role.RoleID);

            using (TransactionScope tScope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand command;
                    connection.Open();

                    foreach (int userID in usersIDs)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = string.Format("DELETE FROM {0} WHERE (PersonID = @PersonID) AND (GroupID = @GroupID)", DBUserToRoleTableName);
                        command.Parameters.AddWithValue("@PersonID", userID);
                        command.Parameters.AddWithValue("@GroupID", role.ID);

                        ExecuteNonQuery(command);
                    }

                    command = connection.CreateCommand();
                    command.CommandText = string.Format("DELETE FROM {0} WHERE ID = @ID", DBRolesTableName);
                    command.Parameters.AddWithValue("@ID", id);

                    bool result = ExecuteNonQuery(command) == 1;
                    if (result)
                        tScope.Complete();
                    return result;
                }
            }
        }

        /// <summary>
        /// Returns array of all roles.
        /// </summary>
        /// <returns>Array of all roles.</returns>
        public override RoleDetails[] GetAllRoles()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM {0}", DBRolesTableName);

                connection.Open();
                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllRoleDetailsFromReader(reader).ToArray();
                }
            }
        }

        /// <summary>
        /// Returns all roles of given user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>Array of all roles of given user.</returns>
        public override RoleDetails[] GetRolesForUser(int userID)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM {0} WHERE PersonID = @PersonID", DBUserToRoleTableName);
                command.Parameters.AddWithValue("@PersonID", userID);

                StringBuilder sb = new StringBuilder();
                using (IDataReader reader = ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");
                        sb.Append(reader["GroupID"].ToString());
                    }
                }

                command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM {0} WHERE ID IN ({1})", DBRolesTableName, sb.ToString());

                using (IDataReader reader = ExecuteReader(command))
                {
                    return GetAllRoleDetailsFromReader(reader).ToArray();
                }
            }
        }

        /// <summary>
        /// Returns IDs of all users in given role.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>Array of IDs of all users in given role.</returns>
        public override int[] GetUsersInRole(string roleID)
        {
            int groupID = GetIDOfRole(roleID);
            if (groupID == -1)
                return new int[0];

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT PersonID FROM {0} WHERE GroupID = @GroupID", DBUserToRoleTableName);
                command.Parameters.AddWithValue("@GroupID", groupID);

                List<int> userIDs = new List<int>();
                using (IDataReader reader = ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        userIDs.Add((int)reader["PersonID"]);
                    }
                }

                return userIDs.ToArray();
            }
        }

        /// <summary>
        /// Is given user in given role.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if user is in role; false, otherwise.</returns>
        public override bool IsUserInRole(int userID, string roleID)
        {
            int groupID = GetIDOfRole(roleID);
            if (groupID == -1)
                return false;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT 1 FROM {0} WHERE (GroupID = @GroupID) AND (PersonID = @PersonID)", DBUserToRoleTableName);
                command.Parameters.AddWithValue("@GroupID", groupID);
                command.Parameters.AddWithValue("@PersonID", userID);

                using (IDataReader reader = ExecuteReader(command))
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// Removes users from roles.
        /// </summary>
        /// <param name="usersIDs">IDs of users.</param>
        /// <param name="roleIDs">Role string IDs.</param>
        public override void RemoveUsersFromRoles(int[] usersIDs, string[] roleIDs)
        {
            if (roleIDs == null)
                return;
            if (usersIDs == null)
                return;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                foreach (string roleID in roleIDs)
                {
                    int groupID = GetIDOfRole(roleID);
                    if (groupID == -1)
                        continue;

                    foreach (int userID in usersIDs)
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = string.Format("DELETE FROM {0} WHERE (PersonID = @PersonID) AND (GroupID = @GroupID)", DBUserToRoleTableName);
                        command.Parameters.AddWithValue("@PersonID", userID);
                        command.Parameters.AddWithValue("@GroupID", groupID);

                        ExecuteNonQuery(command);
                    }
                }
            }
        }

        /// <summary>
        /// Does given role exist.
        /// </summary>
        /// <param name="roleID">Role string ID.</param>
        /// <returns>True if role exists; false, otherwise.</returns>
        public override bool RoleExists(string roleID)
        {
            return (GetIDOfRole(roleID) != -1);
        }
        #endregion

        #region Support methods

        /// <summary>
        /// Returns database ID of role record.
        /// </summary>
        /// <param name="roleID">Role identifier.</param>
        /// <returns>Database ID of role record. -1 otherwise.</returns>
        protected virtual int GetIDOfRole(string roleID)
        {
            if (string.IsNullOrEmpty(roleID))
                return -1;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT ID FROM {0} WHERE GroupID = @GroupID", DBRolesTableName);
                command.Parameters.AddWithValue("@GroupID", roleID);

                using (IDataReader reader = ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        return (int)reader["ID"];
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns role information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Role information from reader.</returns>
        protected virtual RoleDetails GetRoleDetailsFromReader(IDataReader reader)
        {
            RoleDetails details = new RoleDetails();

            details.ID = (int)reader["ID"];
            details.RoleID = (string)reader["GroupID"];
            details.Name = (string)reader["Name"];
            details.Description = (string)reader["Description"];

            return details;
        }

        /// <summary>
        /// Returns role information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Role information from reader.</returns>
        public override List<RoleDetails> GetAllRoleDetailsFromReader(IDataReader reader)
        {
            List<RoleDetails> roles = new List<RoleDetails>();

            while (reader.Read())
            {
                roles.Add(GetRoleDetailsFromReader(reader));
            }

            return roles;
        }

        #endregion
    }
}
