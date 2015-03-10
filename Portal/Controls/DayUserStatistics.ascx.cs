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
	#region ��������
	/// <summary>
	/// ���������� ������������ �� ����.
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

	#region ����������� �������
	/// <summary>
	/// ���������� �������� �������� ����������.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		FillControls(Statistics);
	}
	#endregion

	#region ������
	/// <summary>
	/// ��������� �������� ����������.
	/// </summary>
	/// <param name="stat">���������� ������������ �� ����.</param>
	private void FillControls(DayUserStatistics stat)
	{
		Visible = (stat != null);

		if (stat == null)
		{ return; }

		// ������� ���������� �� ������.
		phAbsence.Visible = ( stat.AbsenceReason != null );
		if (stat.AbsenceReason != null)
		{
			UptimeEventType uet = UptimeEventType.GetEventType((int)stat.AbsenceReason.Value);
			lblAbsenceReason.Text = uet.Name;
			lblAbsenceReason.ForeColor = uet.Color;
		}

		// ������� �������.
		phWorkTimes.Visible = (stat.IsWorked);
		if (stat.IsWorked)
		{
			locBeginTime.Text = stat.BeginTime.ToString("HH:mm");
			locEndTime.Text = stat.EndTime.ToString("HH:mm");
			locTotalTime.Text = DateTimePresenter.GetTime( stat.TotalTime );
			locWorkTime.Text = DateTimePresenter.GetTime( stat.WorkTime );

			// ����� ����������.
			phTimeOff.Visible = (stat.TimeOffTime > TimeSpan.Zero);
			locTimeOffTime.Text = DateTimePresenter.GetTime( stat.TimeOffTime );
		}
	}
	#endregion
}
