using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.FiltersSupport;

public partial class UsersGrid : FilteredDataGrid
{
    #region Events

    public event EventHandler<SelectedObjectEventArgs> UserChanging;

    protected virtual void OnUserChanging()
    {
        if (selectedUserID == -1)
            throw new Exception("Selected user id equals -1.");

        if (UserChanging != null)
            UserChanging(this, new SelectedObjectEventArgs { ObjectID = selectedUserID });
    }

    #endregion

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!IsPostBack)
        {
            gridViewUsers.Columns[2].HeaderStyle.CssClass = "AscSorting";
        }

        gridViewUsers.Sorting += OnGridViewUsers_Sorting;
        objectDataSourcePersons.Deleted += OnDeleted;
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (FilterControl.FilterChanged)
        {
            gridViewUsers.PageIndex = 0;
        }
        base.OnPreRender(e);
    }

    private void OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        BindData();
    }

    private void OnGridViewUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        foreach (DataControlField column in gridViewUsers.Columns)
        {
            if (column.SortExpression == e.SortExpression)
            {
                column.HeaderStyle.CssClass = (e.SortDirection == SortDirection.Ascending)
                                                  ? "AscSorting"
                                                  : "DescSorting";
            }
            else
                column.HeaderStyle.CssClass = "";
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btn = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
            if (btn != null)
            {
                btn.Visible = ((BaseWebPage)Page).CurrentUser.IsInRole(RolesEnum.Administrator);
                btn.OnClientClick = string.Format("if (confirm('Are you sure?') == false) return false; ");
            }
        }
    }

    #region Events of grid view

    protected void gvGridUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //FilterControl.FilterChanged = false;

       /* if (gridViewUsers.SelectedIndex != -1)
        {
            gridViewUsers.SelectedIndex = -1;
            gridViewUsers.DataBind();
        }*/
    }

    protected void gvGridUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnUserChanging();
    }

    protected void gvGridUser_DataBound(object sender, EventArgs e)
    {
        GridViewRow topPagerRow = gridViewUsers.TopPagerRow;
        GridViewRow bottomPagerRow = gridViewUsers.BottomPagerRow;

        ShowPagerData(topPagerRow);
        ShowPagerData(bottomPagerRow);
    }

    #endregion

    #region Events of pager controls

    protected virtual void OnPageIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        gridViewUsers.PageIndex = Convert.ToInt32(ddl.SelectedValue) - 1;
    }

    protected virtual void OnPageSizeChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        var newPageSize = Convert.ToInt32(ddl.SelectedValue);
        gridViewUsers.PageIndex = gridViewUsers.PageIndex * gridViewUsers.PageSize / newPageSize;
        gridViewUsers.PageSize = newPageSize;
    }

    #endregion

    #region Properties

    public int SelectedIndex
    {
        get { return gridViewUsers.SelectedIndex; }
        set { gridViewUsers.SelectedIndex = value; }
    }

    private int selectedUserID
    {
        get
        {
            return gridViewUsers.SelectedDataKey == null
                       ? -1
                       : (int) gridViewUsers.SelectedDataKey.Value;
        }
    }

    #endregion

    #region Methods

    public void GridDataBind()
    {
        gridViewUsers.DataBind();
    }

    /// <summary>
    /// Shows correct pager information.
    /// </summary>
    /// <param name="pagerRow">Pager row.</param>
    private void ShowPagerData(Control pagerRow)
    {
        if (pagerRow == null)
            return;

        DropDownList ddlPages = pagerRow.FindControl("ddlPage") as DropDownList;
        Literal lbl = pagerRow.FindControl("lblPageCount") as Literal;
        DropDownList ddlPSize = pagerRow.FindControl("ddlPageSize") as DropDownList;

        if ((ddlPages != null) && (lbl != null) && (ddlPSize != null))
        {
            ddlPages.Items.Clear();
            for (int pageIndex = 1; pageIndex <= gridViewUsers.PageCount; pageIndex++)
            {
                ListItem item = new ListItem(pageIndex.ToString());
                if (pageIndex == gridViewUsers.PageIndex + 1)
                    item.Selected = true;
                ddlPages.Items.Add(item);
            }

            lbl.Text = gridViewUsers.PageCount.ToString();
            ddlPSize.SelectedValue = gridViewUsers.PageSize.ToString();
        }
    }

    #endregion

    public override void BindData()
    {
        gridViewUsers.SelectedIndex = -1;
        gridViewUsers.DataBind();
    }
}
