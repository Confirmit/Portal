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

        private int CurrentRuleId
        {
            get { return ViewState["CurrentRuleId"] is int ? (int)ViewState["CurrentRuleId"] : -1; }
            set { ViewState["CurrentRuleId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UncheckAllCheckoboxesButton.Click += UncheckAllCheckoboxesButtonOnClick;
            CheckAllCheckoboxesButton.Click += CheckAllCheckoboxesButtonOnClick;
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

        private void CheckAllCheckoboxesButtonOnClick(object sender, EventArgs eventArgs)
        {
            SetCheckingInRows(true);
        }

        private void UncheckAllCheckoboxesButtonOnClick(object sender, EventArgs eventArgs)
        {
            SetCheckingInRows(false);
        }

        private void SetCheckingInRows(bool isChecked)
        {
            var rows = GroupsRuleSelectionGridView.Rows;
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("GroupContainingInRuleCheckBox") as CheckBox;
                    if (checkbox != null)
                        checkbox.Checked = isChecked;
                }
            }
        }

        public void OnRuleChanging(SelectedObjectEventArgs e)
        {
            CurrentRuleId = e.ObjectID;
            BindGroupsInRule();
        }

        public void BindGroupsInRule()
        {
            if (GetGroupsForBindingFunction != null)
            {
                var persons = GetGroupsForBindingFunction();
                GroupsRuleSelectionGridView.DataSource = persons;
                GroupsRuleSelectionGridView.DataBind();
            }
        }
    }
}