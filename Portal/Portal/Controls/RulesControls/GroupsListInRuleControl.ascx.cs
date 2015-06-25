using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.Rules;

namespace Portal.Controls.RulesControls
{
    public partial class GroupsListInRuleControl : UserControl
    {
        public Func<IList<UserGroup>> GetGroupsForBindingFunction;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAllCheckoboxesButton.Attributes.Add("onclick", string.Format("javaScript: {0};", GetJavaScriptCodeForCheckBoxSelection(true)));
                UncheckAllCheckoboxesButton.Attributes.Add("onclick", string.Format("javaScript: {0};", GetJavaScriptCodeForCheckBoxSelection(false)));
            }
        }

        private string GetJavaScriptCodeForCheckBoxSelection(bool isSelectCheckBox)
        {
            var jsCode = @"var dataGrid = document.getElementById('" + GroupsRuleSelectionGridView.ClientID
                + @"');
            var rows = dataGrid.rows;
            for (var index = 1; index < rows.length; index++) {
                var currentRow = rows[index];
                var cell = currentRow.cells[2];
                var checkBox = cell.children[0]; 
                checkBox.checked = " + isSelectCheckBox.ToString().ToLower() +
            @"}";
            return jsCode;
        }

        public IList<int> GetIdsSelectedGroups()
        {
            var rows = GroupsRuleSelectionGridView.Rows;
            var groupIds = new List<int>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("GroupContainingInRuleCheckBox") as CheckBox;
                    if (checkbox != null)
                    {
                        if (checkbox.Checked)
                        {
                            var id = int.Parse(rows[i].Cells[0].Text);
                            groupIds.Add(id);
                        }
                    }
                }
            }
            return groupIds;
        }

        public void OnRuleChanging()
        {
            BindGroupsInRule();
        }

        public void BindGroupsInRule()
        {
            if (GetGroupsForBindingFunction != null)
            {
                var groups = GetGroupsForBindingFunction();
                GroupsRuleSelectionGridView.DataSource = groups;
                GroupsRuleSelectionGridView.DataBind();
            }
        }
    }
}