using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using Portal.Controls.GroupsControls;
using UlterSystems.PortalLib.BusinessObjects;

namespace Portal.Admin
{
    public partial class AdminGroupEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isShowGroupCreatorControl = string.IsNullOrEmpty(Request.QueryString["GroupID"]);
            if (isShowGroupCreatorControl)
            {
                var groupCreatorControl = (GroupCreatorControl)
                             LoadControl("~/Controls/GroupsControls/GroupCreatorControl.ascx");
                GroupConfigurationPlaceHolder.Controls.Add(groupCreatorControl);
                UsersManipulationControl.Visible = false;
            }
            else
            {
                var groupId = int.Parse(Request.QueryString["GroupID"]);
                var groupEditorControl = (GroupEditorControl)
                             LoadControl("~/Controls/GroupsControls/GroupEditorControl.ascx");
                groupEditorControl.GroupId = groupId;
                var groupRepository = new GroupRepository();
                groupEditorControl.SetGroupSettings(groupRepository.GetGroupById(groupId));
                GroupConfigurationPlaceHolder.Controls.Add(groupEditorControl);
                UsersManipulationControl.Visible = true;
                UsersManipulationControl.CurrentWrapperEntityId = groupId;
            }
            UsersManipulationControl.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
            UsersManipulationControl.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
            UsersManipulationControl.GetIncludedEntities += GetIncludedEntitiesForBinding;
            UsersManipulationControl.GetNotIncludedEntities += GetNotIncludedEntitiesForBinding;
        }

        public IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var personsContainingInGroup = GetPersonsContainingInGroup(wrapperEntityId);

            var entities =
                personsContainingInGroup.Select(user => new { user.ID, user.FullName }).ToArray();
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