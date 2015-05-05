using System.Collections;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules.Helpers
{
    public class UserGroupFactory
    {
        public IUserGroup GetUserGroup(IList<int> usersId)
        {
            var testGroup = new TestUserGroup();
            foreach (var id in usersId)
            {
                testGroup.AddUserId(id);
            }
            return testGroup;
        }

        public List<IUserGroup> GetUserGroups(List<List<int>> groups)
        {
            var userGroups = new List<IUserGroup>();
            foreach (var group in groups)
            {
                userGroups.Add(GetUserGroup(group));
            }
            return userGroups;
        }
    }
}