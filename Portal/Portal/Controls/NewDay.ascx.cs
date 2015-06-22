using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using SLService;
using UlterSystems.PortalLib.BusinessObjects;

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
                       : (ControlState)ViewState["State"];
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
    /// ���������� �������� ��������.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.CurrentUser == null || !Page.CurrentUser.ID.HasValue)
        {
            Visible = false;
            return;
        }

        gridViewUserDayEvents.PageIndexChanging += gridViewUserDayEvents_OnPageIndexChanging;
        // ���������� ������� ��������� ������������.
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

    #region ����������� ������� �� ������

    /// <summary>
    /// ���������� ������� �� ������ ������ ������.
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

        InformLastUser();
    }

    /// <summary>
    /// ���������� ������� �� ������ ����������.
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
    /// ���������� ������� �� ������ ����� �� ����.
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
    /// Handles click on the linkButton of changing page.
    /// </summary>
    protected void gridViewUserDayEvents_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
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
            btTime.Visible = btWork.Visible = false;
            return;
        }

        switch (State)
        {
            case ControlState.WorkGoes:
                btWork.Visible = btTime.Visible = true;
                break;

            case ControlState.WorkFinished:
                btWork.Visible = true;
                break;

            case ControlState.Absent:
                btWork.Visible = btTime.Visible = true;
                break;

            case ControlState.Feeding:
                btWork.Visible = true;
                btTime.Visible = false;
                break;

            case ControlState.EnglishLesson:
                btWork.Visible = true;
                btTime.Visible = false;
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
    }

    /// <summary>
    /// ���������� ������� ��������� ������������.
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
    /// ���������� ��������� �������.
    /// </summary>
    private void showTimes()
    {
        ReloadLables();

        SLService.SLService service = new SLService.SLService();
        IDictionary<TimeKey, TimeSpan> timeDict = service.GetFullDayTimes(Page.CurrentUser.ID.Value);

        int hours = (int)timeDict[TimeKey.TodayWork].TotalHours;
        int minutes = (int)(timeDict[TimeKey.TodayWork] - new TimeSpan(hours, 0, 0)).TotalMinutes;
        lblTime.Text = String.Format(lblTime.Text, hours, minutes);

        // ����� �� ��������� ���.
        hours = (int)timeDict[TimeKey.TodayRest].TotalHours;
        minutes = (int)(timeDict[TimeKey.TodayRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
        lblRemainToday.Text = String.Format(lblRemainToday.Text, hours, minutes);

        // ����� ��������� ������.
        DateTime endWork = DateTime.Now.Add(timeDict[TimeKey.TodayRest]);
        lblEndDay.Text = String.Format(lblEndDay.Text, endWork.ToShortTimeString());

        // ����� �� ��������� ������.
        hours = (int)timeDict[TimeKey.WeekRest].TotalHours;
        minutes = (int)(timeDict[TimeKey.WeekRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
        lblRemainWeek.Text = String.Format(lblRemainWeek.Text, hours, minutes);

        // ����� �� ��������� ������.
        hours = (int)timeDict[TimeKey.MonthRest].TotalHours;
        minutes = (int)(timeDict[TimeKey.MonthRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
        lblRemainMonth.Text = String.Format(lblRemainMonth.Text, hours, minutes);
    }
    private void ReloadLables()
    {
        lblTime.Text = GetLocalResourceObject("lblTime.Text").ToString();
        lblRemainToday.Text = GetLocalResourceObject("lblRemainToday.Text").ToString();
        lblEndDay.Text = GetLocalResourceObject("lblEndDay.Text").ToString();
        lblRemainMonth.Text = GetLocalResourceObject("lblRemainMonth.Text").ToString();
    }

    /// <summary>
    /// ������������� ���������� ������������.
    /// </summary>
    private void InformLastUser()
    {
        var ruleRepository = new RuleRepository(new GroupRepository());
        MessageHelper messageHelper = new MessageHelper("Notification last user");
        if (Person.Current.ID != null)
        {
            var notifyLastUserExecutor = new NotifyLastUserExecutor(ruleRepository, new DBWorkEventTypeRecognizer(), new RuleInstanceRepository(ruleRepository), messageHelper, Person.Current.ID.Value);
            var rules = ruleRepository.GetAllRulesByType<NotifyLastUserRule>();

            foreach (var rule in rules)
            {
                notifyLastUserExecutor.ExecuteRule(rule);
            }
        }

        if (!string.IsNullOrEmpty(messageHelper.Body))
        {
            string scriptAllert = string.Format("<script type='text/javascript'> alert('{0} \\n{1}'); </script>",messageHelper.Subject, messageHelper.Body.Replace("\r\n", "\\n"));
            Page.ClientScript.RegisterClientScriptBlock(GetType(),"NewDay", scriptAllert);
        }
    }

    #endregion
}