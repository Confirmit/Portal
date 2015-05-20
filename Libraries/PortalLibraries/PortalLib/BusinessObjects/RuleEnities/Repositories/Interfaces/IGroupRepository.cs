using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IGroupRepository
    {
        IList<int> GetAllUserIdsByGroup(int groupId);
        void SaveGroup(UserGroup group);
        void DeleteGroup(int groupId);
        UserGroup GetGroupById(int groupId);
        void AddUserIdsToGroup(int groupId, params int[] userIds);
        void DeleteUserIdsFromGroup(int groupId, params int[] userIds);
    }
}