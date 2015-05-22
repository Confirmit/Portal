using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.Rules;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository
{
    public class GroupRepository : IGroupRepository
    {
        private const string TableName = "AccordUserGroups";

        public IList<int> GetAllUserIdsByGroup(int groupId)
        {
            var userIds = new List<int>();
            var command = new Query(string.Format("Select UserId FROM {0} WHERE UserGroupId = @userGroupId", TableName));
            command.Add("@UserGroupId", groupId);

            using (var reader = command.ExecReader())
            {
                while (reader.Read())
                {
                    userIds.Add((int)reader["UserId"]);
                }
            }
            command.Command.Connection.Close();
            return userIds;
        }

        public void AddUserIdsToGroup(int groupId, params int[] userIds)
        {
            var usersFromDataBase = GetAllUserIdsByGroup(groupId);
            var nonAddingUsers = userIds.Except(usersFromDataBase);

            if (nonAddingUsers.Count() == 0) return;

            foreach (var userId in nonAddingUsers)
            {
                var command = new Query(string.Format("INSERT INTO {0} (UserId, GroupId) VALUES  (@userId, @groupId)", TableName));
                command.Add("@userId", userId);
                command.Add("@groupId", groupId);
                command.ExecNonQuery();
            }
        }

        public void DeleteUserIdsFromGroup(int groupId, params int[] userIds)
        {
            var usersFromDataBase = GetAllUserIdsByGroup(groupId);
            var nonDeletingusers = usersFromDataBase.Intersect(userIds);

            if (nonDeletingusers.Count() == 0) return;

            var usersIdForDeleting = string.Join(",", userIds);
            
            var command = new Query(string.Format("DELETE FROM {0} WHERE GroupId = @groupId and UserId in ({1})", TableName, usersIdForDeleting));
            command.Add("@groupId", groupId);
            command.ExecNonQuery();
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