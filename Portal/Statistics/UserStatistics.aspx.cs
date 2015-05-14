using System;
using System.Globalization;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_UserStatistics : BaseWebPage
{
	/// <summary>
	/// Обработчик события загрузки страницы.
	/// </summary>
    protected override void OnLoad(EventArgs e)
	{
	    base.OnLoad(e);

	    // Получить пользователя, который запрашивает страницу.
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

	    UserStatisticsControl.ShowStatistics(user, begin, end);
	}

    protected void GenerateReport(object sender, EventArgs e)
    {
        if (CurrentUser == null || CurrentUser.ID == null)
            return;

        UserStatisticsControl.UserID = CurrentUser.ID;
        DateTime begin, end;
        var dateTimeFormatInfo = CultureInfo.InvariantCulture.DateTimeFormat;
        if (!DateTime.TryParse(tbReportFromDate.Text, dateTimeFormatInfo, DateTimeStyles.None, out begin))
            return;
        if (!DateTime.TryParse(tbReportToDate.Text, dateTimeFormatInfo, DateTimeStyles.None, out end))
            return;
        UserStatisticsControl.BeginDate = begin;
        UserStatisticsControl.EndDate = end;
        UserStatisticsControl.FillStatistics();
    }
}
