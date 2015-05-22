using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            command.Destroy();
            return userIds;
        }

        public void AddUserIdsToGroup(int groupId, params int[] userIds)
        {
            var usersFromDataBase = GetAllUserIdsByGroup(groupId);
            int[] nonAddingUsers = userIds.Except(usersFromDataBase).ToArray();


            var insertQuery = new StringBuilder();

            for (int i = 0; i < nonAddingUsers.Count(); i++)
            {
                insertQuery.Append(string.Format("INSERT INTO {0} (UserId, GroupId) VALUES  (@{1}userId, @groupId)", TableName, i));
            }
            var query = new Query(insertQuery.ToString());
            query.Add("@groupId", groupId);

            for (int i = 0; i < nonAddingUsers.Count(); i++)
            {
                query.Add(string.Format("@{0}userId", i), nonAddingUsers[i]);
            }

            query.ExecNonQuery();
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