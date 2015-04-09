using System;
using System.Web.UI.WebControls;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_UsersList : BaseUserControl
{
	#region Свойства

	/// <summary>
	/// Дата, за которую отображаются события.
	/// </summary>
	public DateTime Date
	{
		get
		{
			return ViewState["Date"] == null
					   ? DateTime.Today
					   : (DateTime) ViewState["Date"];
		}
		set
		{
			ViewState["Date"] = value;
		}
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
            GridUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
            GridUsersList.DataBind();
            ViewState["isDescendingSortDirection"] = false;
        }
	}

    protected void SortingCommand_Click(object sender, GridViewSortEventArgs e)
    {
        var isDescendingSortDirection = (bool) ViewState["isDescendingSortDirection"];
        ViewState["isDescendingSortDirection"] = !(bool)ViewState["isDescendingSortDirection"];
        var sortedUsers = UserList.GetStatusesList(Date, isDescendingSortDirection, e.SortExpression);
        GridUsersList.DataSource = sortedUsers;
        GridUsersList.DataBind();

        foreach (DataControlField column in GridUsersList.Columns)
        {
            if (column.SortExpression == e.SortExpression)
            {
                if (isDescendingSortDirection)
                    column.HeaderStyle.CssClass = "AscSorting";
                else
                    column.HeaderStyle.CssClass = "DescSorting";
            }
            else
                column.HeaderStyle.CssClass = "";
        }
    }
}