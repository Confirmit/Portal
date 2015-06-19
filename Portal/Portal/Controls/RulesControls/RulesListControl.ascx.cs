using System;
using System.Web.UI;
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
    }
}