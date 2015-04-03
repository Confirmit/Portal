using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_UsersList : BaseUserControl
{
	#region Классы
	
	/// <summary>
	/// Режимы работы контрола.
	/// </summary>
	public enum Mode
	{
		Standard,
		Admin
	}

	#endregion

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

	/// <summary>
	/// Режим работы контрола.
	/// </summary>
	public Mode ControlMode
	{
		get
		{
			return ViewState["Mode"] == null
					   ? Mode.Standard
					   : (Mode) Enum.Parse(typeof (Mode), (string) ViewState["Mode"]);
		}
		set 
		{
			// Проверка допустимости.
			if( Page.CurrentUser != null && !Page.CurrentUser.IsInRole( "Administrator" ) )
				value = Mode.Standard;

			ViewState["Mode"] = value.ToString();
			// Установка видимости контролов.
			    EnableControls();
		}
	}

	/// <summary>
	/// Ссылка на страницу пользователя в стандартном режиме.
	/// </summary>
	public string StandardNavigateURL
	{
		get
		{
			return ViewState["StandardNavigateURL"] == null
					   ? null
					   : (string) ViewState["StandardNavigateURL"];
		}
		set { ViewState["StandardNavigateURL"] = value; }
	}

	/// <summary>
	/// Ссылка на страницу пользователя в административном режиме.
	/// </summary>
	public string AdminNavigateURL
	{
		get
		{
			return ViewState["AdminNavigateURL"] == null
					   ? null
					   : (string) ViewState["AdminNavigateURL"];
		}
		set { ViewState["AdminNavigateURL"] = value; }
	}

	/// <summary>
	/// Ширина контрола.
	/// </summary>
	public Unit Width
	{
		get { return grdUsersList.Width; }
		set { grdUsersList.Width = value; }
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		// Установка режима работы.
	    if (Page.CurrentUser != null)
	    {
	        if( !Page.CurrentUser.IsInRole(RolesEnum.Administrator))
                ControlMode = Mode.Standard;
	        else
                ControlMode = Mode.Admin;
	    }

		// Заполнить таблицу пользователей
	    if (!IsPostBack)
	    {
            grdUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
            grdUsersList.DataBind();
            ViewState["isDescendingSortDirection"] = false;
	    }
	}

	/// <summary>
	/// Установка видимости контролов.
	/// </summary>
	private void EnableControls()
	{
        switch (ControlMode)
        {
            case Mode.Standard:
                grdUsersList.Columns[2].Visible = false;
                break;

            case Mode.Admin:
                grdUsersList.Columns[2].Visible = true;
                break;
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

	/// <summary>
	/// Привязка данных пользователей к элементам управления.
	/// </summary>
	protected void OnUserInfoBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.DataItem == null || !(e.Item.DataItem is UserStatusInfo))
			return;

		// Получить информацию о статусе пользователя.
		var usInfo = (UserStatusInfo)e.Item.DataItem;

		if (Page.CurrentUser != null && (Page.CurrentUser.ID.HasValue && usInfo.UserID == Page.CurrentUser.ID.Value))
			e.Item.CssClass = "gridview-selectedrow";
	
		// Найти гиперссылку
		var hl = (HyperLink)e.Item.FindControl("hlUserName");
		if (hl == null)
			return;

		// Установить параметры ссылки
		switch (ControlMode)
		{
			case Mode.Standard:
				hl.NavigateUrl = !string.IsNullOrEmpty(StandardNavigateURL)
									 ? StandardNavigateURL + (@"?UserID=" + usInfo.UserID)
									 : string.Empty;
				break;

			case Mode.Admin:
				hl.NavigateUrl = !string.IsNullOrEmpty(AdminNavigateURL)
									 ? AdminNavigateURL + (@"?UserID=" + usInfo.UserID)
									 : string.Empty;
				break;
		}

		// Найти кнопки и установить аргументы.
		ImageButton b = (ImageButton)e.Item.FindControl("btnIll");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnTrustIll");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnBusinessTrip");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnVacation");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();

		b = (ImageButton)e.Item.FindControl("btnLesson");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();

		// Найти кнопку редактирования и установить параметры.
		var lb = (LinkButton)e.Item.FindControl("lbtnEdit");
		if (lb != null)
			lb.PostBackUrl = hl.NavigateUrl;
	}

	protected virtual void OnSetIll(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.Ill);
	}

	protected virtual void OnSetTrustIll(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.TrustIll);
	}

	protected virtual void OnSetBusinessTrip(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.BusinessTrip);
	}

	protected virtual void OnSetVacation(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.Vacation);
	}

	protected void OnSetLesson(object sender, EventArgs e)
	{
		try
		{
			if (!(sender is ImageButton))
				return;

			var b = (ImageButton)sender;

			int userID;
			if (!Int32.TryParse(b.CommandArgument, out userID))
				return;

			var userEvents = new UserWorkEvents(userID);

			var duration = TimeSpan.FromMinutes(45);
			userEvents.AddLatestClosedWorkEvent(duration, WorkEventType.TimeOff);

			FillUsersGrid();
		}
		catch (Exception ex)
		{
			lblException.Text = ex.Message;
			lblException.Visible = true;
		}
	}

	/// <summary>
	/// Create absence event for user.
	/// </summary>
	/// <param name="sender">Image button.</param>
	/// <param name="eventType">Type of event.</param>
	private void createAbsenceEvent(Object sender, WorkEventType eventType)
	{
		try
		{
			if (!(sender is ImageButton))
				return;

			var b = (ImageButton)sender;

			int userID;
			if (!Int32.TryParse(b.CommandArgument, out userID))
				return;

			var userEvents = new UserWorkEvents(userID);
			userEvents.CreateAbsenceEvent(eventType, Date);

			FillUsersGrid();
		}
		catch (Exception ex)
		{
			lblException.Text = ex.Message;
			lblException.Visible = true;
		}
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