using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using UlterSystems.PortalLib.BusinessObjects;

namespace Portal.Controls.GroupsControls
{
    public partial class UserListInGroupControl : UserControl
    {
        public Func<IList<Person>> GetPersonsForBindingFunction;

        private int CurrentGroupId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UncheckAllCheckoboxesButton.Click += UncheckAllCheckoboxesButtonOnClick;
            CheckAllCheckoboxesButton.Click += CheckAllCheckoboxesButtonOnClick;
        }

        public IList<int> GetIdsSelectedPersons()
        {
            var rows = UserGroupsSelectionGridView.Rows;
            var personIds = new List<int>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("UserContainsInGroupCheckBox") as CheckBox;
                    if (checkbox != null)
                    {
                        if (checkbox.Checked)
                        {
                            var id = int.Parse(rows[i].Cells[0].Text);
                            personIds.Add(id);
                        }
                    }
                }
            }
            return personIds;
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
            var rows = UserGroupsSelectionGridView.Rows;
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("UserContainsInGroupCheckBox") as CheckBox;
                    if (checkbox != null)
                        checkbox.Checked = isChecked;
                }
            }
        }

        public void OnGroupChanging(SelectedObjectEventArgs e)
        {
            CurrentGroupId = e.ObjectID;
            BindUsersInGroup();
        }

        public void BindUsersInGroup()
        {
            if (GetPersonsForBindingFunction != null)
            {
                var persons = GetPersonsForBindingFunction();
                UserGroupsSelectionGridView.DataSource = persons;
                UserGroupsSelectionGridView.DataBind();
            }
        }
    }
}