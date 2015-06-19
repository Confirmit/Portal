using System;
using System.Collections.Generic;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using UlterSystems.PortalLib.BusinessObjects;

namespace Portal.Controls.GroupsControls
{
    public partial class UserListInGroupControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {}

        public void OnGroupChanging(object sender, SelectedObjectEventArgs e)
        {
            var currentGroupId = e.ObjectID;
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(currentGroupId);
            var persons = new List<Person>();
            foreach (var userId in allUserIdsByGroup)
            {
                var currentPerson = new Person();
                currentPerson.Load(userId);
                persons.Add(currentPerson);
            }
            UserGroupsSelectionGridView.DataSource = persons;
            UserGroupsSelectionGridView.DataBind();
        }
    }
}