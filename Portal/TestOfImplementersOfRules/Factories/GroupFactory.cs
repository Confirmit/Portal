using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;

namespace TestOfImplementersOfRules.Factories
{
    public class GroupFactory
    {
        private int _userGroupCount = 0;

        public List<UserGroup> GetUserGroups(int number)
        {
            var userGroups = new List<UserGroup>();
            for (int i = 0; i < number; i++)
            {
                userGroups.Add(new UserGroup{ID = _userGroupCount++});
            }
            return userGroups;
        }
    }
}