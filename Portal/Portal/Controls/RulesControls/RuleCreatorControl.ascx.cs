using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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

                DaysOfWeekCheckBoxList.DataSource = Enum.GetNames(typeof(DayOfWeek));
                DaysOfWeekCheckBoxList.DataBind();
            }
            CreateRuleButton.Click += CreateGroupButtonOnClick;
        }

        private void CreateGroupButtonOnClick(object sender, EventArgs eventArgs)
        {
            var userGroup = new UserGroup(RuleNameTextBox.Text);
            var groupRepository = new GroupRepository();
            groupRepository.SaveGroup(userGroup);
            var selectedCheckboxItems = DaysOfWeekCheckBoxList.Items.Cast<ListItem>().Where(x => x.Selected).Select(item => item.Value).ToArray();
            var selectedDaysOfWeek = selectedCheckboxItems.Select(selectedItem => (DayOfWeek) Enum.Parse(typeof (DayOfWeek), selectedItem)).ToList();

            if (RefreshRulesListAction != null)
                RefreshRulesListAction();
        }
    }
}