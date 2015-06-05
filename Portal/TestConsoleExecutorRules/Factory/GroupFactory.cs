using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace TestConsoleExecutorRules.Factory
{
    public class GroupFactory
    {
        public List<UserGroup> GetUserGroupsForNotfyByTime()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("NotfyByTime1"),
                new UserGroup("NotfyByTime2"),
                new UserGroup("NotfyByTime3")
            };

            return result;
        }

        public List<UserGroup> GetUserGroupsForMoscow()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("Moscow1"),
                new UserGroup("Moscow2"),
                new UserGroup("Moscow3")
            };

            return result;
        }

        public List<UserGroup> GetUserGroupsForNotifyLastUser()
        {
            var result = new List<UserGroup>
            {
                new UserGroup("LastUser1"),
                new UserGroup("LastUser2"),
                new UserGroup("LastUser3")
            };

            return result;
        }
    }
}
