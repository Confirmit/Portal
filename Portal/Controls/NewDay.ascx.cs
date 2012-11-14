using System;
using System.Collections.Generic;

using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.BAL;
using SLService;

/// <summary>
/// Элемент управления для создания основных событий пользователей.
/// </summary>
public partial class NewDay : BaseUserControl
{
	#region Fields

	protected ControlState m_State = ControlState.WorkFinished;

	#endregion

	#region ControlState enum

	/// <summary>
	/// States of control.
	/// </summary>
	protected enum ControlState
	{
		WorkGoes,
		WorkFinished,
		Absent,
		Feeding,
		EnglishLesson
	}

	#endregion

	#region Properties

	/// <summary>
	/// State of control.
	/// </summary>
	protected ControlState State
	{
		get
		{
			return ViewState["State"] == null
					   ? ControlState.WorkFinished
					   : (ControlState) ViewState["State"];
		}
		set
		{
			ViewState["State"] = value;
			enableControls();
		}
	}

	#endregion

	#region Event handlers

	/// <summary>
	/// Обработчик загрузки страницы.
	/// </summary>
	protected void Page_Load( object sender, EventArgs e )
	{
		if (Page.CurrentUser == null || !Page.CurrentUser.ID.HasValue)
		{
			Visible = false;
			return;
		}

		// Определить текущее состояние пользователя.
		DefineCurrentState();
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		
		SLService.SLService service = new SLService.SLService();
		gridViewUserDayEvents.DataSource = service.GetTodayWorkEventsOfUser(Page.CurrentUser.ID.Value);
		gridViewUserDayEvents.DataBind();

		showTimes();
	}

	#region Обработчики нажатия на кнопки

	/// <summary>
	/// Обработчик нажатия на кнопку начала работы.
	/// </summary>
	protected void OnWork_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		if (State == ControlState.WorkFinished)
		{
			setUserWorkEvent(true, WorkEventType.MainWork);
			State = ControlState.WorkGoes;
			return;
		}

		setUserWorkEvent(false, WorkEventType.MainWork);
		State = ControlState.WorkFinished;
		
		informLastUser();
	}

	/// <summary>
	/// Обработчик нажатия на кнопку отсутствия.
	/// </summary>
	protected void OnTime_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		if (State == ControlState.WorkGoes)
		{
			setUserWorkEvent(true, WorkEventType.TimeOff);
			State = ControlState.Absent;
			return;
		}

		if (State == ControlState.Absent)
		{
			setUserWorkEvent(false, WorkEventType.TimeOff);
			State = ControlState.WorkGoes;
			return;
		}
	}

	/// <summary>
	/// Обработчик нажатия на кнопку ухода на обед.
	/// </summary>
	protected void OnDinner_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		if (State == ControlState.WorkGoes)
		{
			setUserWorkEvent(true, WorkEventType.LanchTime);
			State = ControlState.Feeding;
			return;
		}

		if (State == ControlState.Feeding)
		{
			setUserWorkEvent(false, WorkEventType.LanchTime);
			State = ControlState.WorkGoes;
			return;
		}

		//    if (!IsPostBack || State != ControlState.WorkGoes)
		//        return;

		//    m_UserWorkEvents.OpenLunchEvent();
		//    State = ControlState.Feeding;
	}

	/// <summary>
	/// Handles click on the button of lesson start.
	/// </summary>
	protected void OnLesson_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		if (State == ControlState.WorkGoes)
		{
			setUserWorkEvent(true, WorkEventType.StudyEnglish);
			State = ControlState.EnglishLesson;
			return;
		}

		if (State == ControlState.EnglishLesson)
		{
			setUserWorkEvent(false, WorkEventType.StudyEnglish);
			State = ControlState.WorkGoes;
			return;
		}
		//    if (!IsPostBack || State != ControlState.WorkGoes)
		//        return;

		//    m_UserWorkEvents.OpenWorkBreakEvent(WorkEventType.StudyEnglish);
		//    State = ControlState.EnglishLesson;
	}

	#endregion

	#endregion

	#region Methods

	private void setUserWorkEvent(bool isOpenAction, WorkEventType eventType)
	{
		if (!WebHelpers.IsRequestIPAllowed())
			return;

		SLService.SLService service = new SLService.SLService();
		service.SetUserWorkEvent(Page.CurrentUser.ID.Value, isOpenAction, eventType);
	}

	/// <summary>
	/// Shows and hides controls depending on state.
	/// </summary>
	private void enableControls()
	{
		setLocalization();

		if (!WebHelpers.IsRequestIPAllowed())
		{
			btDinner.Visible = btLesson.Visible = btTime.Visible = btWork.Visible = false;
			return;
		}

		btLesson.Enabled = Globals.Settings.GlobalSettings.IsEnableBreakButtons;

		switch (State)
		{
			case ControlState.WorkGoes:
				btWork.Visible = btTime.Visible =
								 btDinner.Visible = btLesson.Visible = true;
				break;

			case ControlState.WorkFinished:
				btWork.Visible = true;
				btTime.Visible = btDinner.Visible = btLesson.Visible = false;
				break;

			case ControlState.Absent:
				btWork.Visible = btTime.Visible = true;
				btDinner.Visible = btLesson.Visible = false;
				break;

			case ControlState.Feeding:
				btWork.Visible = true;
				btTime.Visible = false;
				btDinner.Visible = true;
				btLesson.Visible = false;
				break;

			case ControlState.EnglishLesson:
				btWork.Visible = true;
				btTime.Visible = false;
				btDinner.Visible = false;
				btLesson.Visible = true;
				break;
		}
	}

	/// <summary>
	/// Set text to controls.
	/// </summary>
	private void setLocalization()
	{
		btWork.Text = State == ControlState.WorkFinished
						  ? (string)GetLocalResourceObject("btnWorkBegin.Text")
						  : (string)GetLocalResourceObject("btnWorkEnd.Text");

		btTime.Text = State == ControlState.WorkGoes
							  ? (string)GetLocalResourceObject("btnTimeOff.Text")
							  : (string)GetLocalResourceObject("btnTimeOn.Text");

		btLesson.Text = State == ControlState.EnglishLesson
							? (string)GetLocalResourceObject("btLessonOff.Text")
							: (string)GetLocalResourceObject("btLessonOn.Text");

		btDinner.Text = State == ControlState.Feeding
							? (string)GetLocalResourceObject("btnDinnerOff.Text")
							: (string)GetLocalResourceObject("btnDinner.Text");
	}

	/// <summary>
	/// Определяет текущее состояние пользователя.
	/// </summary>
	protected virtual void DefineCurrentState()
	{
		SLService.SLService service = new SLService.SLService();
		IList<WorkEvent> mainAndLastWorkEvent = service.GetMainWorkAndLastEvent(Page.CurrentUser.ID.Value);
		WorkEvent TodayWorkEvent = mainAndLastWorkEvent[0];
		WorkEvent LastEvent = mainAndLastWorkEvent[1];

		if (TodayWorkEvent == null ||
			(TodayWorkEvent.BeginTime != TodayWorkEvent.EndTime))
		{
			State = ControlState.WorkFinished;
			return;
		}

		switch (LastEvent.EventType)
		{
			case WorkEventType.MainWork:
				State = ControlState.WorkGoes;
				break;

			case WorkEventType.TimeOff:
				State = LastEvent.BeginTime == LastEvent.EndTime
							? ControlState.Absent
							: ControlState.WorkGoes;
				break;

			case WorkEventType.LanchTime:
				State = LastEvent.BeginTime == LastEvent.EndTime
							? ControlState.Feeding
							: ControlState.WorkGoes;
				break;

			case WorkEventType.StudyEnglish:
				State = LastEvent.BeginTime == LastEvent.EndTime
							? ControlState.EnglishLesson
							: ControlState.WorkGoes;
				break;
		}
	}

	/// <summary>
	/// Показывает временные отрезки.
	/// </summary>
	private void showTimes()
	{
		SLService.SLService service = new SLService.SLService();
		IDictionary<TimeKey, TimeSpan> timeDict = service.GetFullDayTimes(Page.CurrentUser.ID.Value);

		int hours = (int)timeDict[TimeKey.TodayWork].TotalHours;
		int minutes = (int)(timeDict[TimeKey.TodayWork] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblTime.Text = String.Format(lblTime.Text, hours, minutes);

		// Время до окончания дня.
		hours = (int)timeDict[TimeKey.TodayRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.TodayRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainToday.Text = String.Format(lblRemainToday.Text, hours, minutes);

		// Время окончания работы.
		DateTime endWork = DateTime.Now.Add(timeDict[TimeKey.TodayRest]);
		lblEndDay.Text = String.Format(lblEndDay.Text, endWork.ToShortTimeString());

		// Время до окончания недели.
		hours = (int)timeDict[TimeKey.WeekRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.WeekRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainWeek.Text = String.Format(lblRemainWeek.Text, hours, minutes);

		// Время до окончания месяца.
		hours = (int)timeDict[TimeKey.MonthRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.MonthRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainMonth.Text = String.Format(lblRemainMonth.Text, hours, minutes);
	}

	/// <summary>
	/// Информировать последнего пользователя.
	/// </summary>
	private void informLastUser()
	{
		SLService.SLService service = new SLService.SLService();
		int usersCount = service.GetNumberOfActiveUsers();

		if (usersCount >= 0 && usersCount <= 2)
		{
			String scriptAllert = "<script type='text/javascript'> alert('";
			switch (usersCount)
			{
				case 0:
					scriptAllert += "Вы последний уходите из офиса!";
					break;

				case 1:
					scriptAllert += "В офисе остался только один человек!";
					break;

				case 2:
					scriptAllert += "В офисе осталось только двое!";
					break;
			}

			scriptAllert += "\\nУходя, выключите свет и кулер в столовой!'); </script>";
			Page.ClientScript.RegisterClientScriptBlock(GetType(),
														"NewDay", scriptAllert);
		}
	}

	#endregion
}