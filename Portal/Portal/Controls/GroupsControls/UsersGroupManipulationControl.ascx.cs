using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using UlterSystems.PortalLib.BusinessObjects;

namespace Portal.Controls.GroupsControls
{
    public partial class UsersGroupManipulationControl : UserControl
    {
        private int CurrentGroupId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Visible = false;
            }
            AddUsersInGroupButton.Click += AddUsersInGroupButtonOnClick;
            RemoveUsersFromGroupButton.Click += RemoveUsersFromGroupButtonOnClick;

            UsersListContainingInCurrentGroupControl.GetPersonsForBindingFunction += GetPersonsContainingInGroup;
            UsersListNotContainingInCurrentGroupControl.GetPersonsForBindingFunction += GetPersonsotNotContainingInGroup;
        }

        private void AddUsersInGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedPersons = UsersListNotContainingInCurrentGroupControl.GetIdsSelectedPersons();
            var groupRepository = new GroupRepository();
            groupRepository.AddUserIdsToGroup(CurrentGroupId, idsSelectedPersons.ToArray());

            UsersListContainingInCurrentGroupControl.BindUsersInGroup();
            UsersListNotContainingInCurrentGroupControl.BindUsersInGroup();
        }

        private void RemoveUsersFromGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedPersons = UsersListContainingInCurrentGroupControl.GetIdsSelectedPersons();
            var groupRepository = new GroupRepository();
            groupRepository.DeleteUserIdsFromGroup(CurrentGroupId, idsSelectedPersons.ToArray());

            UsersListContainingInCurrentGroupControl.BindUsersInGroup();
            UsersListNotContainingInCurrentGroupControl.BindUsersInGroup();
        }

        public void OnGroupChanging(SelectedObjectEventArgs e)
        {
            Visible = true;
            CurrentGroupId = e.ObjectID;
            UsersListContainingInCurrentGroupControl.OnGroupChanging(e);
            UsersListNotContainingInCurrentGroupControl.OnGroupChanging(e);
        }

        public IList<Person> GetPersonsContainingInGroup()
        {
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(CurrentGroupId);
            var personsInGroup = new List<Person>();
            foreach (var userId in allUserIdsByGroup)
            {
                var currentPerson = new Person();
                currentPerson.Load(userId);
                personsInGroup.Add(currentPerson);
            }
            return personsInGroup;
        }

        public IList<Person> GetPersonsotNotContainingInGroup()
        {
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(CurrentGroupId);
            var allPersons = UserList.GetUserList();
            var personsNotInGroup = allPersons.Where(user => !allUserIdsByGroup.Contains(user.ID.Value)).ToList();
            return personsNotInGroup;
        }
    }
}