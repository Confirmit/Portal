using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class RuleCreatorControl : UserControl
    {
        public Action RefreshRulesListAction;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var ruleKinds = Enum.GetNames(typeof(RuleKind));
                RuleTypesDropDownList.DataSource = ruleKinds;
                RuleTypesDropDownList.DataBind();
            }
            CreateRuleButton.Click += CreateGroupButtonOnClick;
            AddNewRuleButton.Click += AddNewGroupButtonOnClick;
            AddNewRuleButton.Visible = true;
            RuleConfigurationPanel.Visible = false;
        }

        private void AddNewGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            AddNewRuleButton.Visible = false;
            RuleConfigurationPanel.Visible = true;
        }

        private void CreateGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var userGroup = new UserGroup(RuleNameTextBox.Text);
            var groupRepository = new GroupRepository();
            groupRepository.SaveGroup(userGroup);
            if (RefreshRulesListAction != null)
                RefreshRulesListAction();
        }
    }
}