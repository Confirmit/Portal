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
        private int CurrentGroupId
        {
            get { return ViewState["CurrentGroupId"] is int ? (int)ViewState["CurrentGroupId"] : -1; }
            set { ViewState["CurrentGroupId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserGroupsSelectionGridView.RowDataBound += UserGroupsSelectionGridViewOnRowDataBound;
            UncheckAllCheckoboxesButton.Click += UncheckAllCheckoboxesButtonOnClick;
            CheckAllCheckoboxesButton.Click += CheckAllCheckoboxesButtonOnClick;
            SaveChangesButton.Click += SaveChangesButtonOnClick;
        }

        private void SaveChangesButtonOnClick(object sender, EventArgs eventArgs)
        {
            var groupRepository = new GroupRepository();
            var rows = UserGroupsSelectionGridView.Rows;
            var deletingPersonIds = new List<int>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("UserContainsInGroupCheckBox") as CheckBox;
                    if (checkbox != null)
                    {
                        if (!checkbox.Checked)
                        {
                            var id = int.Parse(rows[i].Cells[0].Text);
                            deletingPersonIds.Add(id);
                        }
                    }
                }
            }
            groupRepository.DeleteUserIdsFromGroup(CurrentGroupId, deletingPersonIds.ToArray());
            BindUsersInGroup();
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

        private void UserGroupsSelectionGridViewOnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var checkBox = e.Row.Cells[e.Row.Cells.Count - 1].FindControl("UserContainsInGroupCheckBox") as CheckBox;
                if (checkBox != null)
                {
                    checkBox.Checked = true;
                }
            }
        }

        public void OnGroupChanging(object sender, SelectedObjectEventArgs e)
        {
            CurrentGroupId = e.ObjectID;
            BindUsersInGroup();
        }

        private void BindUsersInGroup()
        {
            var allUserIdsByGroup = new GroupRepository().GetAllUserIdsByGroup(CurrentGroupId);
            var persons = new List<Person>();
            foreach (var userId in allUserIdsByGroup)
            {
                var currentPerson = new Person();
                currentPerson.Load(userId);
                persons.Add(currentPerson);
            }
            UserGroupsSelectionGridView.DataSource = persons;
            UserGroupsSelectionGridView.DataBind();
        }
    }
}