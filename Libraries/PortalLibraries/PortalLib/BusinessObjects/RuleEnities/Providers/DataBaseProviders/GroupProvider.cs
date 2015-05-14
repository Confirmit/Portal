using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ConfirmIt.PortalLib.Rules;
using Core;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class GroupProvider : IGroupProvider
    {
        private const string TableName = "AccordUserGroups";

        public IList<int> GetAllUserIdsByGroup(int groupId)
        {
            var userIds = new List<int>();
            using (var connection = new SqlConnection(ConnectionManager.DefaultConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("Select UserId FROM {0} WHERE UserGroupId = @userGroupId", TableName);
                command.Parameters.AddWithValue("@UserGroupId", groupId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userIds.Add((int)reader["UserId"]);
                    }
                }

                connection.Close();
            }
            return userIds;
        }

        public void AddUserIdsToGroup(int groupId, params int[] userIds)
        {
            var usersFromDataBase = GetAllUserIdsByGroup(groupId);
            var nonAddingUsers = userIds.Except(usersFromDataBase);

            if (nonAddingUsers.Count() == 0) return;

            using (var connection = new SqlConnection(ConnectionManager.DefaultConnectionString))
            {
                connection.Open();

                foreach (var userId in nonAddingUsers)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (UserId, GroupId) VALUES  (@userId, @groupId)", TableName);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void DeleteUserIdsFromGroup(int groupId, params int[] userIds)
        {
            var usersFromDataBase = GetAllUserIdsByGroup(groupId);
            var nonDeletingusers = usersFromDataBase.Intersect(userIds);

            if (nonDeletingusers.Count() == 0) return;

            var usersIdForDeleting = string.Join(",", userIds);

            using (var connection = new SqlConnection(ConnectionManager.DefaultConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE GroupId = @groupId and UserId in ({1})", TableName, usersIdForDeleting);
                command.Parameters.AddWithValue("@groupId", groupId);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void SaveGroup(UserGroup group)
        {
            group.Save();
        }

        public void DeleteGroup(int id)
        {
            GetGroupById(id).Delete();
        }

        public UserGroup GetGroupById(int id)
        {
            var instance = new UserGroup();
            instance.Load(id);
            return instance;
        }
    }
}