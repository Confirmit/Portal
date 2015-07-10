using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;

namespace TestConsoleExecutorRules.Factory
{
    public class GroupFactory
    {
        public List<UserGroup> GetUserGroupsForNotfyByTime()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("Name1", "NotfyByTime1"),
                new UserGroup("Name1","NotfyByTime2"),
                new UserGroup("Name1", "NotfyByTime3")
            };

            return result;
        }

        public List<UserGroup> GetUserGroupsForMoscow()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("Name1","Moscow1"),
                new UserGroup("Name1","Moscow2"),
                new UserGroup("Name1","Moscow3")
            };

            return result;
        }

        public List<UserGroup> GetUserGroupsForNotifyLastUser()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("Name1","LastUser1"),
                new UserGroup("Name1","LastUser2"),
                new UserGroup("Name1","LastUser3")
            };

            return result;
        }
    }
}
