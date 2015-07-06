using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls.GroupsControls
{
    public partial class GroupCreatorControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateGroupButton.Click += CreateGroupButtonOnClick;
        }

        private void CreateGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(GroupSettingsControl.GroupName.Text)
                && !string.IsNullOrEmpty(GroupSettingsControl.GroupDescription.Text))
            {
                var userGroup = new UserGroup(GroupSettingsControl.GroupName.Text, GroupSettingsControl.GroupDescription.Text);
                var groupRepository = new GroupRepository();
                groupRepository.SaveGroup(userGroup);
                var urlForRedirection = string.Format("{0}?GroupID={1}", Request.Url, userGroup.ID);
                Response.Redirect(urlForRedirection, false);
            }
        }
    }
}