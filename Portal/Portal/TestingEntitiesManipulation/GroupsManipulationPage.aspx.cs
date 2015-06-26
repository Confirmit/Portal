using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.TestingEntitiesManipulation
{
    public partial class GroupsManipulationPage : BaseWebPage, IEntityManipulationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesManipulationControl.CurrentWrapperEntityId = 58;
            EntitiesManipulationControl.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
            EntitiesManipulationControl.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
            EntitiesManipulationControl.GetIncludedEntities += GetIncludedEntitiesForBinding;
            EntitiesManipulationControl.GetNotIncludedEntities += GetNotIncludedEntitiesForBinding;
        }

        public IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);

            var entities =
               allGroupsByRule.Select(
                   group => new {group.ID, group.Description }).ToArray();
            return entities;
        }

        public IList<object> GetNotIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);
            var allGroups = new GroupRepository().GetAllGroups();
            var userGroupsNotContainingInCurrentRule = allGroups
                .Where(userGroupFromAllGroups => !allGroupsByRule.Any(userGroupByRule => userGroupByRule.ID.Value == userGroupFromAllGroups.ID.Value)).ToList();

            var entities =
                userGroupsNotContainingInCurrentRule.Select(
                    group => new {group.ID, group.Description}).ToArray();
            return entities;
        }

        public void AddEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.AddGroupIdsToRule(wrapperEntityId, idsSelectedEntities.ToArray());
        }

        public void RemoveEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.DeleteGroupIdsFromRule(wrapperEntityId, idsSelectedEntities.ToArray());
        }
    }
}