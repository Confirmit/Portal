using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

public partial class Controls_UserStatistics : BaseUserControl
{
	#region ��������

	/// <summary>
	/// ID ������������, ��� �������� �������������� ����������.
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
	/// ���� ������ ��������� ������� ����������.
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
	/// ���� ��������� ��������� ������� ����������.
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

	#region ����������� �������

	/// <summary>
	/// �������� ������ ������������� � ��������� ����������.
	/// </summary>
	protected void OnStatisticsBound(object sender, DataGridItemEventArgs e)
	{
        if (e.Item.DataItem == null || !(e.Item.DataItem is DayUserStatistics))
			return;

		DayUserStatistics dStat = (DayUserStatistics) e.Item.DataItem;

		// ����� ������� ��� �������.
		Label lbl = (Label)e.Item.FindControl("locDate");
		if (lbl != null)
		{
		    lbl.Text = dStat.Date.ToString("ddd dd/MM/yyyy");
		    lbl.ForeColor = CalendarItem.GetHoliday(dStat.Date)
		                        ? Color.Red
		                        : Color.Black;
		}

	    // ����� ������� ��� ����������� ������.
		Controls_DayUserStatistics statControl = (Controls_DayUserStatistics)e.Item.FindControl("dayStat");
		if (statControl != null)
			statControl.Statistics = dStat;
	}

	#endregion

	#region ������
	/// <summary>
	/// ���������� ������� ���������� �������� ����������.
	/// </summary>
	/// <param name="user">������������, ��� ���������� ������������.</param>
	/// <param name="begin">������ ��������� ����������.</param>
	/// <param name="end">����� ��������� ����������.</param>
	public void ShowStatistics(Person user, DateTime begin, DateTime end)
	{
		if (user == null || user.ID == null)
			return;

        UserID = user.ID.Value;
		BeginDate = begin;
		EndDate = end;
		FillStatistics();
	}

	/// <summary>
	/// ��������� �������� ���������� ����������� � ����������.
	/// </summary>
    public void FillStatistics()
	{
		Visible = false;
        if (UserID == null
            || BeginDate == DateTime.MinValue
            || EndDate == DateTime.MinValue)
            return;

		// ������� ������������.
		Person user = new Person();
        if (!user.Load(UserID.Value))
            return;

		// �������� ���������� �� ������ ������.
	    PeriodUserStatistics stat = PeriodUserStatistics.GetUserStatistics(user,
	                                                                       BeginDate,
	                                                                       EndDate);
		if (stat == null)
			return;

		// ������� ������ � ��������.
		grdDaysStats.DataSource = stat.DaysStatistics;
		grdDaysStats.DataBind();

		// �������� ����� ����� � ������� �����.
		lblTotalTime.Text = "  " + DateTimePresenter.GetTime(stat.TotalTime);
		lblWorkTime.Text = "  " + DateTimePresenter.GetTime(stat.WorkTime);
		lblTimeRate.Text = " " + DateTimePresenter.GetTime(stat.TimeRate);
		lblRestTime.Text = "  " + DateTimePresenter.GetTime(stat.RestTime);
        
        Visible = true;
	}
	#endregion
}
