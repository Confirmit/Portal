using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls
{
    public partial class AdminGroupsEditingControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var groupRepository = new GroupRepository();
                var currentListOfGroups = groupRepository.GetAllGroups();

                GroupsEditingGridView.DataSource = currentListOfGroups;
                GroupsEditingGridView.DataBind();
            }
        }

        protected void EditGroupImageButton_OnClick(object sender, ImageClickEventArgs e)
        {


        }

        protected void RemoveGroupImageButton_OnClick(object sender, ImageClickEventArgs e)
        {


        }

        protected void OnGroupRowBound(object sender, GridViewRowEventArgs e)
        {


        }
    }
}