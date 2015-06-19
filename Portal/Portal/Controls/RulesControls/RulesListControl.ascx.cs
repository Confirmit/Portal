using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls.RulesControls
{
    public partial class RulesListControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var groupRepository = new GroupRepository();
                var allRules = new RuleRepository(groupRepository).GetAllRules();
                RulesListGridView.DataSource = allRules;
                RulesListGridView.DataBind();
            }
        }

        protected void RulesListGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imageButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
                if (imageButton != null)
                {
                    imageButton.Visible = ((BaseWebPage)Page).CurrentUser.IsInRole(RolesEnum.Administrator);
                    imageButton.OnClientClick = string.Format("if (confirm('Are you sure?') == false) return false; ");
                }
            }
        }
    }
}