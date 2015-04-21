using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Persons;
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

        GridViewUsers.Sorting += OnGridViewUsers_Sorting;
        if (!Page.IsPostBack)
        {
            GridViewUsers.VirtualItemCount = 140;
            GridViewUsers.PageSize = 10;
            GridViewUsers.Attributes["SortExpression"] = "LastName";
            BindPersons();
        }
        //TODO
        //objectDataSourcePersons.Deleted += OnDeleted;
    }

    private void OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        BindData();
    }

    private void OnGridViewUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        foreach (DataControlField column in GridViewUsers.Columns)
        {
            if (column.SortExpression == e.SortExpression)
            {
                column.HeaderStyle.CssClass = (GridViewUsers.Attributes["SortExpression"].Contains(" DESC"))
                                                  ? "DescSorting"
                                                  : "AscSorting";
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

    protected void GridViewUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex == -1 || e.NewPageIndex == GridViewUsers.PageCount)
            return;

        GridViewUsers.PageIndex = e.NewPageIndex;
        BindPersons();
    }

    private void BindPersons(String sortExpression = "")
    {
        if (sortExpression == "")
            sortExpression = GridViewUsers.Attributes["SortExpression"];
        var startRowIndex = GridViewUsers.PageIndex*GridViewUsers.PageSize;
        var dataSource = new PersonDataSource().Select(sortExpression, maximumRows: GridViewUsers.PageSize, startRowIndex: startRowIndex);
        GridViewUsers.DataSource = dataSource;
        GridViewUsers.DataBind();
    }

    protected void GridViewUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnUserChanging();
    }

    protected void GridViewUser_DataBound(object sender, EventArgs e)
    {
        var topPagerRow = GridViewUsers.TopPagerRow;
        var bottomPagerRow = GridViewUsers.BottomPagerRow;

        ShowPagerData(topPagerRow);
        ShowPagerData(bottomPagerRow);
    }

    #endregion

    #region Events of pager controls

    protected virtual void OnPageIndexChanged(object sender, EventArgs e)
    {
        var dropDownList = (DropDownList)sender;
        GridViewUsers.PageIndex = (Convert.ToInt32(dropDownList.SelectedValue) - 1);
        BindPersons();
    }

    protected virtual void OnPageSizeChanged(object sender, EventArgs e)
    {
        var dropDownList = (DropDownList)sender;
        var newPageSize = Convert.ToInt32(dropDownList.SelectedValue);
        GridViewUsers.PageIndex = GridViewUsers.PageIndex * GridViewUsers.PageSize / newPageSize;
        GridViewUsers.PageSize = newPageSize;
        BindPersons();
    }

    #endregion

    #region Properties

    public int SelectedIndex
    {
        get { return GridViewUsers.SelectedIndex; }
        set { GridViewUsers.SelectedIndex = value; }
    }

    private int selectedUserID
    {
        get
        {
            return GridViewUsers.SelectedDataKey == null
                       ? -1
                       : (int)GridViewUsers.SelectedDataKey.Value;
        }
    }

    #endregion

    #region Methods

    public void GridDataBind()
    {
        GridViewUsers.DataBind();
    }

    /// <summary>
    /// Shows correct pager information.
    /// </summary>
    /// <param name="pagerRow">Pager row.</param>
    private void ShowPagerData(Control pagerRow)
    {
        if (pagerRow == null)
            return;

        var dropDownListPages = pagerRow.FindControl("ddlPage") as DropDownList;
        var literal = pagerRow.FindControl("lblPageCount") as Literal;
        var dropDownListSize = pagerRow.FindControl("ddlPageSize") as DropDownList;

        if ((dropDownListPages != null) && (literal != null) && (dropDownListSize != null))
        {
            dropDownListPages.Items.Clear();
            //var realPageCount = (double)GridViewUsers.VirtualItemCount / GridViewUsers.PageSize;
            //var roundedPageCount = (int)(Math.Round(realPageCount));
            for (var pageIndex = 1; pageIndex <= GridViewUsers.PageCount; pageIndex++)
            {
                var item = new ListItem(pageIndex.ToString());
                if (pageIndex == GridViewUsers.PageIndex + 1)
                    item.Selected = true;
                dropDownListPages.Items.Add(item);
            }

            literal.Text = GridViewUsers.PageCount.ToString();
            dropDownListSize.SelectedValue = GridViewUsers.PageSize.ToString();
        }
    }

    #endregion

    public override void BindData()
    {
        GridViewUsers.SelectedIndex = -1;
        GridViewUsers.DataBind();
    }

    protected void GridViewUsers_OnSorting(object sender, GridViewSortEventArgs e)
    {
        if (GridViewUsers.Attributes["SortExpression"] == e.SortExpression)
            GridViewUsers.Attributes["SortExpression"] = GridViewUsers.Attributes["SortExpression"] + " DESC";
        else
            GridViewUsers.Attributes["SortExpression"] = e.SortExpression;
        BindPersons();
    }
}
