using System;

namespace Portal.Admin
{
    public partial class AdminGroupEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlForEditingGroups.GroupSelectionChangingEventHandler += UsersGroupManipulationControl.OnGroupChanging;
            GroupCreator.RefreshGroupsListEventHandler += ControlForEditingGroups.RefreshGroupList;
        }
    }
}