using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using UlterSystems.PortalLib.Statistics;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_DayUserStatistics : BaseUserControl
{
	#region Свойства
	/// <summary>
	/// Статистика пользователя за день.
	/// </summary>
	public DayUserStatistics Statistics
	{
		get
		{
			if (ViewState["Statistics"] == null)
				return null;
			return (DayUserStatistics)ViewState["Statistics"];
		}
		set
		{
			ViewState["Statistics"] = value;
			FillControls(value);
		}
	}
	#endregion

	#region Обработчики событий
	/// <summary>
	/// Обработчик загрузки элемента управления.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		FillControls(Statistics);
	}
	#endregion

	#region Методы
	/// <summary>
	/// Заполняет элементы управления.
	/// </summary>
	/// <param name="stat">Статистика пользователя за день.</param>
	private void FillControls(DayUserStatistics stat)
	{
		Visible = (stat != null);

		if (stat == null)
		{ return; }

		// Причина отсутствия на работе.
		phAbsence.Visible = ( stat.AbsenceReason != null );
		if (stat.AbsenceReason != null)
		{
			UptimeEventType uet = UptimeEventType.GetEventType((int)stat.AbsenceReason.Value);
			lblAbsenceReason.Text = uet.Name;
			lblAbsenceReason.ForeColor = uet.Color;
		}

		// Рабочие времена.
		phWorkTimes.Visible = (stat.IsWorked);
		if (stat.IsWorked)
		{
			locBeginTime.Text = stat.BeginTime.ToString("HH:mm");
			locEndTime.Text = stat.EndTime.ToString("HH:mm");
			locTotalTime.Text = DateTimePresenter.GetTime( stat.TotalTime );
			locWorkTime.Text = DateTimePresenter.GetTime( stat.WorkTime );

			// Время отсутствия.
			phTimeOff.Visible = (stat.TimeOffTime > TimeSpan.Zero);
			locTimeOffTime.Text = DateTimePresenter.GetTime( stat.TimeOffTime );
		}
	}
	#endregion
}
