using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using Core;
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
            grdUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
            grdUsersList.DataBind();
            ViewState["isDescendingSortDirection"] = false;
        }
	}

	/// <summary>
	/// Заполняет список пользователей.
	/// </summary>
	protected void FillUsersGrid()
	{
        grdUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
        grdUsersList.DataBind();
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
        grdUsersList.DataSource = sortedUsers;
        grdUsersList.DataBind();
    }
}