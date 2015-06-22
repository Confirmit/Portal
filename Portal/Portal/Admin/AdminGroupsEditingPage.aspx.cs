using System;

namespace Portal.Admin
{
    public partial class AdminGroupEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlForEditingGroups.EventHandler += UsersListForCurrentGroupControl.OnGroupChanging;
            GroupCreator.RefreshGroupsListEventHandler += ControlForEditingGroups.RefreshGroupList;
        }
    }
}