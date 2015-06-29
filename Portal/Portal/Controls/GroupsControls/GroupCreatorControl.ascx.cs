using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.Rules;

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
            if (!string.IsNullOrEmpty(GroupDescriptionTextBox.Text))
            {
                var userGroup = new UserGroup(GroupDescriptionTextBox.Text);
                var groupRepository = new GroupRepository();
                groupRepository.SaveGroup(userGroup);
                var urlForRedirection = string.Format("{0}?GroupID={1}", Request.Url, userGroup.ID);
                Response.Redirect(urlForRedirection, false);
            }
        }
    }
}