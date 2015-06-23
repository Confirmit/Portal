﻿using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.Rules;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestGroupRepository : IGroupRepository
    {
        private IDictionary<int, List<int>> groups = new Dictionary<int, List<int>>();

        public IList<UserGroup> GetAllRules()
        {
            throw new System.NotImplementedException();
        }

        public IList<UserGroup> GetAllGroups()
        {
            throw new System.NotImplementedException();
        }

        public IList<int> GetAllUserIdsByGroup(int groupId)
        {
            return groups[groupId];
        }

        public void SaveGroup(UserGroup userGroup)
        {
            if (groups.ContainsKey(userGroup.ID.Value))
                return;
            groups.Add(userGroup.ID.Value, new List<int>());
        }

        public void DeleteGroup(int groupId)
        {
            groups.Remove(groupId);
        }

        public UserGroup GetGroupById(int groupId)
        {
            if (!groups.ContainsKey(groupId))
                return null;

            if(!groups.ContainsKey(groupId)) throw new KeyNotFoundException("Id of group was not found");
            return new UserGroup() {ID = groupId};
        }

        public void AddUserIdsToGroup(int groupId, params int[] userIds)
        {
            if (!groups.ContainsKey(groupId))
                return;

            var users = new HashSet<int>(groups[groupId]);
            users.UnionWith(userIds);

            groups[groupId]= new List<int>(userIds); 
        }

        public void DeleteUserIdsFromGroup(int groupId, params int[] userIds)
        {
            if (!groups.ContainsKey(groupId))
                return;

            foreach (var userId in userIds)
            {
                groups[groupId].Remove(userId);
            }
        }
    }
}