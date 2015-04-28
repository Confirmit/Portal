using System;
using System.Web;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Logger;
using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Admin_AdminUserInfo : BaseWebPage
{
	#region Fields

	private Person m_InfoUser;

	#endregion

    #region Page events 

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        dgUserEventsDataGrid.EditCommand += OnEditCommand;
        dgUserEventsDataGrid.CancelCommand += OnCancelCommand;
        dgUserEventsDataGrid.UpdateCommand += OnUpdateCommand;
        dgUserEventsDataGrid.DeleteCommand += OnDeleteCommand;
        dgUserEventsDataGrid.ItemDataBound += OnItemDataBound;
    }

    protected void Page_Load( object sender, EventArgs e )
	{
		// Получить ID пользователя, информация которого отображается.
		string userIDStr = Request.QueryString["UserID"];
        if (string.IsNullOrEmpty(userIDStr))
            Response.Redirect(hlMain.NavigateUrl);

		int userID;
		if(!Int32.TryParse(userIDStr, out userID))
		    Response.Redirect( hlMain.NavigateUrl );

		// Получить пользователя, информация которого отображается.
	    m_InfoUser = new Person();
		m_InfoUser.Load(userID);

		// Показать имя пользователя.
		lblUserName.Text = m_InfoUser.FullName;

		if(!IsPostBack)
		{
			Calendar.SelectedDate = DateTime.Today;
			Calendar_SelectionChanged( Calendar, new EventArgs() );
		}
    }

    #endregion

    #region DATA GRID EVENTS

    protected void OnEditCommand(object source, DataGridCommandEventArgs e)
	{
		dgUserEventsDataGrid.EditItemIndex = e.Item.ItemIndex;
		RefreshDataGridState();
	}

	protected void OnCancelCommand(object source, DataGridCommandEventArgs e)
	{
        lblException.Visible = false;
		dgUserEventsDataGrid.EditItemIndex = -1;
		RefreshDataGridState();
	}

    //обновляем текущую запись в DataGrid
	protected void OnUpdateCommand(object source, DataGridCommandEventArgs e)
	{
        lblException.Visible = false;
		
        // Получить ID редактируемого события.
        DataGridItem dgi = e.Item;
		int eventID = (int)dgUserEventsDataGrid.DataKeys[dgi.ItemIndex];

        try
        {
            Panel panel = (Panel) dgi.Cells[0].FindControl("dataPanelEdit");
            if (panel == null)
                throw new Exception("Could not find controls on row.");

            TextBox tbText = (TextBox) panel.Controls[1];
            DateTime begTime = Convert.ToDateTime(Calendar.SelectedDate.ToShortDateString() + " " + tbText.Text);

            tbText = (TextBox)panel.Controls[3];
            DateTime endTime = Convert.ToDateTime(Calendar.SelectedDate.ToShortDateString() + " " + tbText.Text);

            DropDownExtender dropDown = (DropDownExtender)panel.Controls[9];
            int eventTypeID = dropDown.SelectedObjectID;
            WorkEvent utEvent = WorkEvent.GetEventByID(eventID);

            if (utEvent == null)
                return;

			if (CurrentUser.ID.HasValue)
			{
				var ip = "Unknown";
				if (HttpContext.Current != null)
					ip = HttpContext.Current.Request.UserHostAddress;

				var targetPerson = Person.GetPersonByID(utEvent.UserID);
				Logger.Instance.InfoFormat("Administrator {0} ({1}) update {2} event type for {3} ({4}) from {5} ip address:",
				                           CurrentUser.ID.Value, CurrentUser.FullName,
				                           eventTypeID,
				                           utEvent.UserID, targetPerson.FullName, ip);

				Logger.Instance.InfoFormat("{0} - {1} -> {2} - {3}", utEvent.BeginTime, utEvent.EndTime, begTime, endTime);
									  
			}

        	WorkEvent.UpdateEvent(
                utEvent.ID, utEvent.Name,
                begTime, endTime,
                utEvent.UserID, utEvent.ProjectID, utEvent.WorkCategoryID,
                eventTypeID);
        }
        catch (Exception ex)
        {
            showException(ex.Message);
        }
        finally
		{
            LoadDataForCurDayCurUser();

            if (!lblException.Visible)
                dgUserEventsDataGrid.EditItemIndex = -1;

            RefreshDataGridState();
		}
	}

    protected void OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.EditItem)
            return;

        try
        {
            WorkEvent uEvent = (WorkEvent) e.Item.DataItem;
            if (uEvent == null)
                return;

            DropDownExtender dde = (DropDownExtender)
                               e.Item.FindControl("dropDownExtender");
            if (dde != null)
            {
                dde.SelectedObjectID = uEvent.EventTypeID;
                //dde.Text = uEvent.EventType.ToString();
            }
        }
        catch (Exception ex)
        {
            showException(ex.Message);
        }
    }

    void OnDeleteCommand(object source, DataGridCommandEventArgs e)
    {
        // Получить ID редактируемого события.
        int eventID = (int)dgUserEventsDataGrid.DataKeys[e.Item.ItemIndex];
        WorkEvent.DeleteEvent(eventID);

        bool needToReloadData;
        if (e.CommandArgument == null)
            needToReloadData = true;
        else
        {
            if (e.CommandArgument is string)
                Boolean.TryParse(e.CommandArgument.ToString(), out needToReloadData);
            else
                needToReloadData = (bool) e.CommandArgument;
        }

        if (needToReloadData)
        {
            dgUserEventsDataGrid.EditItemIndex = -1;
            LoadDataForCurDayCurUser();
            RefreshDataGridState();
        }
    }

    #endregion

	#region PAGE  BUTTONS, LIST, CALENDAR  EVENTS

	/// <summary>
	/// Функция для добавления новой записи.
	/// </summary>
    protected void bt_NewUptimeEvent_Click(object sender, EventArgs e)
	{
	    //Добавляем новую запись
	    DateTime DTime = (DateTime) (ViewState["SelectedDate"]);

	    //добавляем новую запись непосредственно в базу
	    int newEventID = WorkEvent.CreateEvent(
	        string.Empty,
	        DTime,
	        DTime,
	        m_InfoUser.ID.Value,
	        1,
	        1,
	        (int) WorkEventType.NoData);

	    LoadDataForCurDayCurUser();
	    WorkEvent[] events = (WorkEvent[]) (Session["EventData"]);

	    for (int i = 0; i < events.Length; i++)
	    {
	        WorkEvent curEvent = events[i];
	        if (curEvent.ID == newEventID)
	        {
	            dgUserEventsDataGrid.EditItemIndex = i;
	            break;
	        }
	    }
	    RefreshDataGridState();
	}

    /// <summary>
	/// Функция для удаления выбранных событий.
	/// </summary>
    protected void bt_DeleteUptimeEvent_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < dgUserEventsDataGrid.Items.Count; i++)
        {
            DataGridItem dataGridItem = dgUserEventsDataGrid.Items[i];
            Panel panel = (Panel)dataGridItem.Cells[0].FindControl("dataPanel");
            if (panel == null)
                throw new Exception("Could not find controls on row.");

            CheckBox checkBox = (CheckBox) panel.Controls[1];
            if (checkBox == null || !checkBox.Checked)
                continue;

            DataGridCommandEventArgs args =
                new DataGridCommandEventArgs(dataGridItem,
                                             dgUserEventsDataGrid,
                                             new CommandEventArgs("IsNeedToReloadData", false));
            OnDeleteCommand(dgUserEventsDataGrid, args);
        }

        dgUserEventsDataGrid.EditItemIndex = -1;
        LoadDataForCurDayCurUser();
        RefreshDataGridState();
    }

    /// <summary>
	/// Событие изменения даты в календаре.
	/// </summary>
    protected void Calendar_SelectionChanged(object sender, EventArgs e)
	{
	    dgUserEventsDataGrid.EditItemIndex = -1;
	    // Сохранить выбранную дату.
	    ViewState["SelectedDate"] = Calendar.SelectedDate;
	    LoadDataForCurDayCurUser();
	    RefreshDataGridState();
	}


    #endregion

	#region   SERVICES FUNCTION

    private void showException(String exMessage)
    {
        lblException.Text = exMessage;
        lblException.Visible = true;
    }

	/// <summary>
	/// Преобразует ID типа события в его название.
	/// </summary>
	/// <param name="obj">ID типа события.</param>
	/// <returns>Название типа события.</returns>
	protected string ConvertWorkTypeToString( object obj )
	{
		return UptimeEventType.ConvertWorkTypeToString( obj );
	}

    protected String ConvertTimeSpanToString(TimeSpan time)
    {
        return time.ToString().Split('.')[0];
    }

	/// <summary>
	/// Возвращает список типов событий.
	/// </summary>
	/// <returns>Список типов событий.</returns>
	protected UptimeEventType[] GetEventTypeList()
	{
		return UptimeEventType.GetAllEventTypes();
	}

	/// <summary>
	/// После выбора дня загружает данные по пользователю за выбранный день 
	/// и сохраняет их в Session.
	/// </summary>
    protected void LoadDataForCurDayCurUser()
	{
	    DateTime date = new DateTime(1900, 1, 1);

	    if (ViewState["SelectedDate"] != null)
	    {
	        DateTime selDate = (DateTime) ViewState["SelectedDate"];
	        date = new DateTime(selDate.Year, selDate.Month, selDate.Day, 0, 0, 0);
	    }

	    WorkEvent[] events = WorkEvent.GetEventsOfDate(m_InfoUser.ID.Value, date);

	    Session["EventData"] = events == null
	                               ? new WorkEvent[0]
	                               : events;
	}

    /// <summary>
	/// Обновляет состояние DataGrid, заполняя ее данными.
	/// </summary>
    protected void RefreshDataGridState()
    {
        dgUserEventsDataGrid.DataSource = Session["EventData"];
        dgUserEventsDataGrid.DataBind();
        dgUserEventsDataGrid.Columns[0].Visible = true;
    }

    #endregion
}
