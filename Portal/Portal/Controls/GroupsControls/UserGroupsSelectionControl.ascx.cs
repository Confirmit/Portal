using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls.GroupsControls
{
    public partial class UserGroupsSelectionControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {}

        public void OnGroupChanging(object sender, SelectedObjectEventArgs e)
        {
            var currentGroupID = e.ObjectID;
            var usersFromCurrentGroup = new GroupRepository().GetAllUserIdsByGroup(currentGroupID);
            SelectedGroupLabel.Text = string.Format("GroupID = {0}, Amount of Users = {1}", e.ObjectID,
                usersFromCurrentGroup.Count);

            //UserGroupsSelectionGridView.DataSource = usersFromCurrentGroup;
            //UserGroupsSelectionGridView.DataBind();
        }
    }
}