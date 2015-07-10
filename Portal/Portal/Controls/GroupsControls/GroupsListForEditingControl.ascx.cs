using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls.GroupsControls
{
    public partial class GroupsListForEditingControl : UserControl
    {
        public event EventHandler<SelectedObjectEventArgs> GroupSelectionChangingEventHandler;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            GroupsEditingGridView.SelectedIndexChanged += GroupsEditingGridView_OnSelectedIndexChanged;
            GroupsEditingGridView.RowDeleting += GroupsEditingGridViewOnRowDeleting;
            GroupsEditingGridView.RowDataBound += GroupsEditingGridView_OnRowDataBound;
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                var groupRepository = new GroupRepository();
                var currentListOfGroups = groupRepository.GetAllGroups();

                GroupsEditingGridView.DataSource = currentListOfGroups;
                GroupsEditingGridView.DataBind();
            }
        }

        private void GroupsEditingGridViewOnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var groupRepository = new GroupRepository();
            var groupId = int.Parse(GroupsEditingGridView.Rows[e.RowIndex].Cells[0].Text);
            groupRepository.DeleteGroup(groupId);

            var currentListOfGroups = groupRepository.GetAllGroups();
            GroupsEditingGridView.DataSource = currentListOfGroups;
            GroupsEditingGridView.DataBind();
        }

        protected void GroupsEditingGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedGroupId == -1)
                throw new Exception("Selected group id equals -1.");

            if (GroupSelectionChangingEventHandler != null && SelectedGroupId != -1)
                GroupSelectionChangingEventHandler(this, new SelectedObjectEventArgs { ObjectID = SelectedGroupId });
        }

        private int SelectedGroupId
        {
            get
            {
                return GroupsEditingGridView.SelectedDataKey == null
                           ? -1
                           : (int)GroupsEditingGridView.SelectedDataKey.Value;
            }
        }

        public void RefreshGroupList()
        {
            var groupRepository = new GroupRepository();
            var currentListOfGroups = groupRepository.GetAllGroups();

            GroupsEditingGridView.DataSource = currentListOfGroups;
            GroupsEditingGridView.DataBind();
        }

        protected void GroupsEditingGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imageButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
                if (imageButton != null)
                {
                    imageButton.Visible = ((BaseWebPage)Page).CurrentUser.IsInRole(RolesEnum.Administrator);
                    imageButton.OnClientClick = "if (confirm(\'Are you sure?\') == false) return false; ";
                }
            }
        }
    }
}