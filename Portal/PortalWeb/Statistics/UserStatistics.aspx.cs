using System;

using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_UserStatistics : BaseWebPage
{
	/// <summary>
	/// ���������� ������� �������� ��������.
	/// </summary>
    protected override void OnLoad(EventArgs e)
	{
	    base.OnLoad(e);

	    // �������� ������������, ������� ����������� ��������.
	    if (string.IsNullOrEmpty(Request.QueryString["UserID"]))
	        Response.Redirect(hlMain.NavigateUrl);

	    int userID;
	    if (!Int32.TryParse(Request.QueryString["UserID"], out userID))
	        Response.Redirect(hlMain.NavigateUrl);

	    Person user = new Person();
	    if (!user.Load(userID))
	        Response.Redirect(hlMain.NavigateUrl);

	    DateTime begin;
	    DateTime end;
	    if (!DateClass.TryParseRequestQueryDates(Request, out begin, out end))
	        Response.Redirect(hlMain.NavigateUrl);

	    userStat.ShowStatistics(user, begin, end);
	}
}
