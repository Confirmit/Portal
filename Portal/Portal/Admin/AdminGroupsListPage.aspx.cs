using System;

namespace Portal.Admin
{
    public partial class AdminGroupsListPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GroupsListForEditingControl.GroupSelectionChangingEventHandler += GroupSelectionChangingEventHandler;
            AddNewGroupButton.Click += AddNewGroupButtonOnClick;
        }

        private void GroupSelectionChangingEventHandler(object sender, SelectedObjectEventArgs selectedObjectEventArgs)
        {
            Response.Redirect(string.Format("~/Admin/AdminGroupsEditingPage.aspx?GroupID={0}", selectedObjectEventArgs.ObjectID), false);
        }

        private void AddNewGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            Response.Redirect("~/Admin/AdminGroupsEditingPage.aspx", false);
        }
    }
}