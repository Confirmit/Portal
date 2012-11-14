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

using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_Statistics : BaseWebPage
{
	#region Fields
	private string sBegDate, sEndDate;
	#endregion

	#region Event handlers
	protected void Page_Load(object sender, EventArgs e)
	{}
	#endregion

	#region REPORTING SERVICES REFERENCES
	protected void lbtnRSLastWeek_Click(object sender, EventArgs e)
	{
		//���������� ������������ �� �������� � ������� �� ��������� ������
		DateClass.GetPeriodLastWeek(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
	protected void lbtnRSCurrentWeek_Click(object sender, EventArgs e)
	{   
		//���������� ������������ �� �������� � ������� �� ������� ������            
		DateClass.GetPeriodCurrentWeek(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
	protected void lbtnRSCurrentMonth_Click(object sender, EventArgs e)
	{
		//���������� ������������ �� �������� � ������� �� ������� �����
		DateClass.GetPeriodCurrentMonth(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
	}
    protected void lbtnRSCurrentMonthToNow_Click(object sender, EventArgs e)
    {
        //���������� ������������ �� �������� � ������� �� ������� ����� �� �������� �������
        DateClass.GetPeriodCurrentMonthToNow(out sBegDate, out sEndDate);
        SetUrlToRedirect(sBegDate, sEndDate);
    }
	protected void lbtnRSLastMonth_Click(object sender, EventArgs e)
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
