using System;
using System.Drawing;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

public partial class Controls_UserStatistics : BaseUserControl
{
	#region Свойства

	/// <summary>
	/// ID пользователя, для которого рассчитывается статистика.
	/// </summary>
	public int? UserID
	{
		get
		{
			if (ViewState["StatisticsUserID"] == null)
				return null;

			return (int)ViewState["StatisticsUserID"];
		}
		set { ViewState["StatisticsUserID"] = value; }
	}

	/// <summary>
	/// Дата начала интервала расчета статистики.
	/// </summary>
	public DateTime BeginDate
	{
		get
		{
			if (ViewState["BeginDate"] == null)
				return DateTime.MinValue;

			return (DateTime)ViewState["BeginDate"];
		}
		set { ViewState["BeginDate"] = value; }
	}

	/// <summary>
	/// Дата окончания интервала расчета статистики.
	/// </summary>
	public DateTime EndDate
	{
		get
		{
			if (ViewState["EndDate"] == null)
				return DateTime.MinValue;

			return (DateTime)ViewState["EndDate"];
		}
		set { ViewState["EndDate"] = value; }
	}

	#endregion

	#region Обработчики событий

	/// <summary>
	/// Привязка данных пользователей к элементам управления.
	/// </summary>
	protected void OnStatisticsBound(object sender, DataGridItemEventArgs e)
	{
        if (e.Item.DataItem == null || !(e.Item.DataItem is DayUserStatistics))
			return;

		DayUserStatistics dStat = (DayUserStatistics) e.Item.DataItem;

		// Найти контрол для времени.
		Label lbl = (Label)e.Item.FindControl("locDate");
		if (lbl != null)
		{
		    lbl.Text = dStat.Date.ToString("ddd dd/MM/yyyy");
		    lbl.ForeColor = CalendarItem.GetHoliday(dStat.Date)
		                        ? Color.Red
		                        : Color.Black;
		}

	    // Найти контрол для отображения времен.
		Controls_DayUserStatistics statControl = (Controls_DayUserStatistics)e.Item.FindControl("dayStat");
		if (statControl != null)
			statControl.Statistics = dStat;
	}

	#endregion

	#region Методы
	/// <summary>
	/// Заставляет элемент управления показать статистику.
	/// </summary>
	/// <param name="user">Пользователь, чья статистика показывается.</param>
	/// <param name="begin">Начало интервала статистики.</param>
	/// <param name="end">Конец интервала статистики.</param>
	public void ShowStatistics(Person user, DateTime begin, DateTime end)
	{
		if (user == null || user.ID == null)
			return;

        UserID = user.ID.Value;
		BeginDate = begin;
		EndDate = end;
		fillStatistics();
	}

	/// <summary>
	/// Заполняет элементы управления информацией о статистике.
	/// </summary>
    private void fillStatistics()
	{
		Visible = false;
        if (UserID == null
            || BeginDate == DateTime.MinValue
            || EndDate == DateTime.MinValue)
            return;

		// Создать пользователя.
		Person user = new Person();
        if (!user.Load(UserID.Value))
            return;

		// Получить статистику за данный период.
	    PeriodUserStatistics stat = PeriodUserStatistics.GetUserStatistics(user,
	                                                                       BeginDate,
	                                                                       EndDate);
		if (stat == null)
			return;

		// Связать данные с таблицей.
		grdDaysStats.DataSource = stat.DaysStatistics;
		grdDaysStats.DataBind();

		// Показать общее время и рабочее время.
		lblTotalTime.Text = "  " + DateTimePresenter.GetTime(stat.TotalTime);
		lblWorkTime.Text = "  " + DateTimePresenter.GetTime(stat.WorkTime);
		lblTimeRate.Text = " " + DateTimePresenter.GetTime(stat.TimeRate);
		lblRestTime.Text = "  " + DateTimePresenter.GetTime(stat.RestTime);
        
        Visible = true;
	}
	#endregion
}
