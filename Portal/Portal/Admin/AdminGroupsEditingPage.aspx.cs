using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects;
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
                var groupCreatorControl = (GroupCreatorControl) LoadControl("~/Controls/GroupsControls/GroupCreatorControl.ascx");
                GroupConfigurationPlaceHolder.Controls.Add(groupCreatorControl);
                UsersManipulationControl.Visible = false;
            }
            else
            {
                var groupId = int.Parse(Request.QueryString["GroupID"]);
                var groupEditorControl = (GroupEditorControl) LoadControl("~/Controls/GroupsControls/GroupEditorControl.ascx");
                groupEditorControl.GroupId = groupId;
                var groupRepository = new GroupRepository();
                groupEditorControl.SetGroupSettings(groupRepository.GetGroupById(groupId));
                GroupConfigurationPlaceHolder.Controls.Add(groupEditorControl);
                UsersManipulationControl.Visible = true;
                UsersManipulationControl.CurrentWrapperEntityId = groupId;
            }

            if (!Page.IsPostBack)
            {
                UsersManipulationControl.AddCommonColumnsToEntitiesGridView("IDColumn", "ID");
                UsersManipulationControl.AddCommonColumnsToEntitiesGridView("FullNameColumn", "FullName");
                UsersManipulationControl.AddCommonColumnsToEntitiesGridView("SexIDColumn", "SexID");
                UsersManipulationControl.AddCommonColumnsToEntitiesGridView("BirthdayColumn", "Birthday");
            }

            UsersManipulationControl.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
            UsersManipulationControl.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
            UsersManipulationControl.GetIncludedEntities += GetIncludedEntitiesForBinding;
            UsersManipulationControl.GetNotIncludedEntities += GetNotIncludedEntitiesForBinding;
        }

        public IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var personsContainingInGroup = GetPersonsContainingInGroup(wrapperEntityId);

            return personsContainingInGroup.Select(user => (object)user).ToArray();
        }

        public IList<object> GetNotIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var personsotNotContainingInGroup = GetPersonsotNotContainingInGroup(wrapperEntityId);

            return personsotNotContainingInGroup.Select(user => (object)user).ToArray();
        }

        public void AddEntitiesToWrapperEntity(object sender, EntitiesManipulationEventArgs entitiesManipulationEventArgs)
        {
            var groupRepository = new GroupRepository();
            groupRepository.AddUserIdsToGroup(entitiesManipulationEventArgs.WrapperEntityId, entitiesManipulationEventArgs.IdsSelectedEntities.ToArray());
        }

        public void RemoveEntitiesToWrapperEntity(object sender, EntitiesManipulationEventArgs entitiesManipulationEventArgs)
        {
            var groupRepository = new GroupRepository();
            groupRepository.DeleteUserIdsFromGroup(entitiesManipulationEventArgs.WrapperEntityId, entitiesManipulationEventArgs.IdsSelectedEntities.ToArray());
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