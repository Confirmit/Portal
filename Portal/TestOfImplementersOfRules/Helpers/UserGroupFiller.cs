using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using TestOfImplementersOfRules.Factories;

namespace TestOfImplementersOfRules.Helpers
{
    public class UserGroupFiller
    {
        public void FillGroupRepository(IGroupRepository groupRepository, int countGroups, int countUsers)
        {
            var groups = new GroupFactory().GetUserGroups(countGroups);
            var users = new PersonFactory().GetPersons(countUsers);

            foreach (var group in groups)
            {
                groupRepository.AddUserIdsToGroup(group.ID.Value, users.Select(user => user.ID.Value).ToArray());
            }
        }
    }
}