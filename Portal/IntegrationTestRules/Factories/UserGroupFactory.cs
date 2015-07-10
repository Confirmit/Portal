using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;

namespace IntegrationTestRules
{
    public class UserGroupFactory
    {
        public List<UserGroup> GetUserGroups(int count)
        {
            List<UserGroup> groups = new List<UserGroup>();
            for (int i = 0; i < count; i++)
            {
                var group = new UserGroup(i.ToString(), i.ToString());
                groups.Add(group);
            }
            return groups;
        }
    }
}