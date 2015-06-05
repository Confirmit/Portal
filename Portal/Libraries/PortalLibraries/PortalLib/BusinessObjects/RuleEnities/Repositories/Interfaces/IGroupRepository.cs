using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        IList<UserGroup> GetAllGroups();
        IList<int> GetAllUserIdsByGroup(int groupId);
        void SaveGroup(UserGroup group);
        void DeleteGroup(int groupId);
        UserGroup GetGroupById(int groupId);
        void AddUserIdsToGroup(int groupId, params int[] userIds);
        void DeleteUserIdsFromGroup(int groupId, params int[] userIds);
    }
}