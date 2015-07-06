using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls.GroupsControls
{
    public partial class GroupEditorControl : UserControl
    {
        public int GroupId
        {
            get { return (int) ViewState["GroupID"]; }
            set { ViewState["GroupID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SaveGroupChangesButton.Click += SaveGroupChangesButtonOnClick;
        }

        private void SaveGroupChangesButtonOnClick(object sender, EventArgs eventArgs)
        {
            var groupRepository = new GroupRepository();
            var modifyingUserGroup = groupRepository.GetGroupById(GroupId);
            modifyingUserGroup.Name = GroupSettingsControl.GroupName.Text;
            modifyingUserGroup.Description= GroupSettingsControl.GroupDescription.Text;
            groupRepository.SaveGroup(modifyingUserGroup);
            Response.Redirect(Request.Url.ToString(), false);
        }

        public void SetGroupSettings(UserGroup userGroup)
        {
            GroupSettingsControl.GroupName.Text = userGroup.Name;
            GroupSettingsControl.GroupDescription.Text = userGroup.Description;
        }
    }
}