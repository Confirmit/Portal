using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using UlterSystems.PortalLib.BusinessObjects;

namespace Portal.TestingEntitiesManipulation
{
    public partial class UsersManipulationPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesManipulationControl.CurrentWrapperEntityId = 50;
            EntitiesManipulationControl.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
            EntitiesManipulationControl.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
            EntitiesManipulationControl.GetIncludedEntities += GetIncludedEntitiesForBinding;
            EntitiesManipulationControl.GetNotIncludedEntities += GetNotIncludedEntitiesForBinding;
        }

        public IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var allGroupsByRule = GetPersonsContainingInGroup(wrapperEntityId);

            var entities =
                allGroupsByRule.Select(user => new {user.ID, user.FullName}).ToArray();
            return entities;
        }

        public IList<object> GetNotIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var personsotNotContainingInGroup = GetPersonsotNotContainingInGroup(wrapperEntityId);

            var entities =
               personsotNotContainingInGroup.Select(
                    person => new { person.ID, person.FullName }).ToArray();
            return entities;
        }

        public void AddEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            groupRepository.AddUserIdsToGroup(wrapperEntityId, idsSelectedEntities.ToArray());
        }

        public void RemoveEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            groupRepository.DeleteUserIdsFromGroup(wrapperEntityId, idsSelectedEntities.ToArray());
        }

        public IList<Person> GetPersonsContainingInGroup(int wrapperEntityId)
        {
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(wrapperEntityId);
            var personsInGroup = new List<Person>();
            foreach (var userId in allUserIdsByGroup)
            {
                var currentPerson = new Person();
                currentPerson.Load(userId);
                personsInGroup.Add(currentPerson);
            }
            return personsInGroup;
        }

        public IList<Person> GetPersonsotNotContainingInGroup(int wrapperEntityId)
        {
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(wrapperEntityId);
            var allPersons = UserList.GetUserList();
            var personsNotInGroup = allPersons.Where(user => !allUserIdsByGroup.Contains(user.ID.Value)).ToList();
            return personsNotInGroup;
        }
    }
}