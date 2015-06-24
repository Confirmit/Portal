using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;

namespace TestOfImplementersOfRules.Factories
{
    public class GroupRepositoryFactory : IGroupRepositoryFactory
    {
        public IGroupRepository GetGroupRepository()
        {
            var groupRepository = new TestGroupRepository();
            var userGroupIds = new GroupFactory().GetUserGroups();
            userGroupIds.ForEach(groupRepository.SaveGroup);

            var userIds = new PersonFactory().GetPersons().Select(user => user.ID.Value);
            userGroupIds.ForEach(group => groupRepository.AddUserIdsToGroup(group.ID.Value, userIds.ToArray()));

            return groupRepository;
        }
    }
}