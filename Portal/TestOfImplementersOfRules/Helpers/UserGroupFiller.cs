using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using TestOfImplementersOfRules.Factories;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestOfImplementersOfRules.Helpers
{
    public class UserGroupFiller
    {
        public void FillGroupRepository(IGroupRepository groupRepository, int countGroups, int countUsers)
        {
            var groups = new GroupFactory().GetUserGroups(countGroups);
            var users = new PersonFactory().GetPersons(countUsers);

            FillGroupRepository(groupRepository, groups, users);
        }

        public void FillGroupRepository(IGroupRepository groupRepository, IList<UserGroup> groups, IList<Person> users)
        {
            foreach (var group in groups)
            {
                groupRepository.SaveGroup(group);
                groupRepository.AddUserIdsToGroup(group.ID.Value, users.Select(user => user.ID.Value).ToArray());
            }
        }
    }
}