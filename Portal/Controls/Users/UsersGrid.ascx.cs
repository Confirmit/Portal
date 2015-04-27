using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.FiltersSupport;
using Core;
using Core.ORM;
using UlterSystems.PortalLib.BusinessObjects;

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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!Page.IsPostBack)
        {
            GridViewUsers.PageSize = 10;
            ViewState["CurrentGridViewSortEventSerializableArgs"] = new GridViewSortEventSerializableArgs("LastName", SortDirection.Ascending);
            BindPersons();
        }
        if (FilterControl.Filter != null)
        {
            var personFilter = (PersonsFilter)FilterControl.Filter;
            if (personFilter.IsContainsDataForFiltering())
            {
                var gridViewSortEventSerializableArgs =
                       (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
                var amountPersons = SiteProvider.Users.GetFilteredUsersCount((PersonsFilter)FilterControl.Filter);
                var startRowIndex = GridViewUsers.PageIndex * GridViewUsers.PageSize;
                String sortExpressionWithEnding;
                if (CultureManager.CurrentLanguage == CultureManager.Languages.Russian)
                    sortExpressionWithEnding = string.Format("{0}{1}", gridViewSortEventSerializableArgs.SortExpression,
                        ObjectMapper.RussianEnding);
                else
                    sortExpressionWithEnding = string.Format("{0}{1}", gridViewSortEventSerializableArgs.SortExpression,
                        ObjectMapper.EnglishEnding);
                var filtredPersons = SiteProvider.Users.GetFilteredUsers(sortExpressionWithEnding,
                    GridViewUsers.PageSize, startRowIndex, (PersonsFilter)FilterControl.Filter);
                GridViewUsers.VirtualItemCount = amountPersons;
                GridViewUsers.DataSource = filtredPersons;
                GridViewUsers.DataBind();
            }
            else
                BindPersons();
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
                btn.OnClientClick = "if (confirm(\'Are you sure?\') == false) return false; ";
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

    private void BindPersons()
    {
        var dataSource = SelectPersonPaging();
        GridViewUsers.DataSource = dataSource;
        GridViewUsers.DataBind();
    }

    public IList<Person> SelectPersonPaging()
    {
        var currentGridViewSortEventSerializableArgs =
           (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
        var startRowIndex = GridViewUsers.PageIndex * GridViewUsers.PageSize;
        var maximumRows = GridViewUsers.PageSize;
        var isAscendingOrder = currentGridViewSortEventSerializableArgs.CurrentDirection == SortDirection.Ascending;
        var sortExpression = currentGridViewSortEventSerializableArgs.SortExpression;
        var pagingResult = BasePlainObject.GetObjectsPage(typeof(Person), new PagingArgs(startRowIndex / maximumRows, maximumRows, sortExpression, isAscendingOrder));
        GridViewUsers.VirtualItemCount = pagingResult.TotalCount;
        return (IList<Person>)pagingResult.Result;
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
        var oldGridViewSortEventArgs = (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
        SortDirection newSortDirection;
        if (oldGridViewSortEventArgs.SortExpression == e.SortExpression)
        {
            if (oldGridViewSortEventArgs.CurrentDirection == SortDirection.Ascending)
                newSortDirection = SortDirection.Descending;
            else
                newSortDirection = SortDirection.Ascending;
        }
        else
            newSortDirection = SortDirection.Ascending;

        foreach (DataControlField column in GridViewUsers.Columns)
        {
            if (column.SortExpression == e.SortExpression)
            {
                column.HeaderStyle.CssClass = newSortDirection == SortDirection.Ascending
                                                  ? "AscSorting"
                                                  : "DescSorting";
            }
            else
                column.HeaderStyle.CssClass = "";
        }

        ViewState["CurrentGridViewSortEventSerializableArgs"] = new GridViewSortEventSerializableArgs(e.SortExpression, newSortDirection);
        BindPersons();
    }

    protected void GridViewUsers_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BasePlainObject.DeleteObjectByID(typeof(Person), (int)e.Keys[0]);
        BindPersons();
    }
}
