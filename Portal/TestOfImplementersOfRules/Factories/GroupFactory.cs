using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace TestOfImplementersOfRules.Factories
{
    public class GroupFactory
    {
        private const int defaultNumber = 5;

        public List<UserGroup> GetUserGroups(int number = defaultNumber)
        {
            var userGroups = new List<UserGroup>();
            for (int i = 0; i < number; i++)
            {
                userGroups.Add(new UserGroup{ID = i});
            }
            return userGroups;
        }
    }
}