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
			FillUsersGrid();
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

	/// <summary>
	/// Заполняет список пользователей.
	/// </summary>
	protected void FillUsersGrid()
	{
        GridUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
        GridUsersList.DataBind();
	}

    protected void SortingCommand_Click(object sender, GridViewSortEventArgs e)
    {
        bool isDescendingSortDirection;

        if ((bool)ViewState["isDescendingSortDirection"])
        {
            ViewState["isDescendingSortDirection"] = false;
            isDescendingSortDirection = true;
        }
        else
        {
            ViewState["isDescendingSortDirection"] = true;
            isDescendingSortDirection = false;
        }

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