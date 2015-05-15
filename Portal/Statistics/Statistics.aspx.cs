using System;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_Statistics : BaseWebPage
{
	#region Fields
	private string sBegDate, sEndDate;
	#endregion

	#region REPORTING SERVICES REFERENCES
	protected void GetReportByLastWeekLinkButton_Click(object sender, EventArgs e)
	{
		//���������� ������������ �� �������� � ������� �� ��������� ������
		DateClass.GetPeriodLastWeek(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
	protected void GetReportByCurrentWeekLinkButton_Click(object sender, EventArgs e)
	{   
		//���������� ������������ �� �������� � ������� �� ������� ������            
		DateClass.GetPeriodCurrentWeek(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
	protected void GetReportByCurrentMonthLinkButton_Click(object sender, EventArgs e)
	{
		//���������� ������������ �� �������� � ������� �� ������� �����
		DateClass.GetPeriodCurrentMonth(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
    protected void GetReportByCurrentMonthToNowLinkButton_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ������� ����� �� �������� �������
        DateClass.GetPeriodCurrentMonthToNow(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
    }
	protected void GetReportByLastMonthLinkButton_Click(object sender, EventArgs e)
	{
		//���������� ������������ �� �������� � ������� �� ��������� ������
		DateClass.GetPeriodLastMonth(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
    private void SetUrlToRedirect(string sBegDate, string sEndDate)
    {
        string URL = hlStatPage.NavigateUrl + "?";
        URL += "UserID=" + CurrentUser.ID.Value + "&";
        URL += "BeginDate=" + sBegDate + "&";
        URL += "EndDate=" + sEndDate;
        Response.Redirect(URL);
    }

	#endregion
}
