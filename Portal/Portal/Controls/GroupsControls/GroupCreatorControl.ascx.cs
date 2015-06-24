using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls.GroupsControls
{
    public partial class GroupCreatorControl : System.Web.UI.UserControl
    {
        public Action RefreshGroupsListEventHandler;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateGroupButton.Click += CreateGroupButtonOnClick;
            AddNewGroupButton.Click += AddNewGroupButtonOnClick;
            AddNewGroupButton.Visible = true;
            GroupConfigurationPanel.Visible = false;
        }

        private void AddNewGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            AddNewGroupButton.Visible = false;
            GroupConfigurationPanel.Visible = true;
        }

        private void CreateGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(GroupNameTextBox.Text))
            {
                var userGroup = new UserGroup(GroupNameTextBox.Text);
                var groupRepository = new GroupRepository();
                groupRepository.SaveGroup(userGroup);
                if (RefreshGroupsListEventHandler != null)
                    RefreshGroupsListEventHandler();
            }
        }
    }
}