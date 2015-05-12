using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IGroupProvider
    {
        IList<UserGroup> GetGroupsByRule(int ruleId);
        void SaveGroup(UserGroup group);
        void DeleteGroup(int groupId);
        UserGroup GetGroupById(int groupId);
    }
}