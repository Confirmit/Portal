using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Controls.EntitiesManipulationControls
{
    public partial class EntitiesListControl : UserControl
    {
        public Func<IList<object>> GetGroupsForBindingFunction;

        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesGridView.RowDataBound += EntitiesGridViewOnRowDataBound;
        }

        private void EntitiesGridViewOnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            CheckBox checkBox;
            if (e.Row.RowType == DataControlRowType.Header)
                checkBox = e.Row.FindControl("SelectAllCheckboxes") as CheckBox;
            else
                checkBox = e.Row.FindControl("EntitySelectionCheckBox") as CheckBox;

            if (checkBox != null)
            {
                var clientId = checkBox.ClientID;
                var jsCode = string.Format("javascript: selectAllEntitiesCheckBoxes({0}, {1}, {2});",
                    EntitiesListGridView.ClientID, clientId, (e.Row.RowType == DataControlRowType.Header).ToString().ToLower());
                checkBox.Attributes.Add("onclick", jsCode);
            }
        }

        public GridView EntitiesGridView
        {
            get { return EntitiesListGridView; }
        }

        public IList<int> GetIdsSelectedEntities()
        {
            var rows = EntitiesListGridView.Rows;
            var groupIds = new List<int>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].RowType == DataControlRowType.DataRow)
                {
                    var checkbox = rows[i].FindControl("EntitySelectionCheckBox") as CheckBox;
                    if (checkbox != null)
                    {
                        if (checkbox.Checked)
                        {
                            var id = int.Parse(rows[i].Cells[1].Text);
                            groupIds.Add(id);
                        }
                    }
                }
            }
            return groupIds;
        }

        public void OnEntityChanging()
        {
            BindEntities();
        }

        public void BindEntities()
        {
            if (GetGroupsForBindingFunction != null)
            {
                var entities = GetGroupsForBindingFunction();
                EntitiesListGridView.DataSource = entities;
                EntitiesListGridView.DataBind();
            }
        }
    }
}