using System;
using System.Globalization;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_UserStatistics : BaseWebPage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

	    // Получить пользователя, который запрашивает страницу.
	    if (string.IsNullOrEmpty(Request.QueryString["UserID"]))
	        Response.Redirect(hlMain.NavigateUrl);

	    int userID;
	    if (!Int32.TryParse(Request.QueryString["UserID"], out userID))
	        Response.Redirect(hlMain.NavigateUrl);

        if (!CurrentUser.IsInRole("Administrator") && userID != CurrentUser.ID)
            Response.Redirect(hlMain.NavigateUrl);

	    var user = new Person();
	    if (!user.Load(userID))
	        Response.Redirect(hlMain.NavigateUrl);

	    DateTime beginDateFromQueryStringInInvariantCulture;
        DateTime endDateFromQueryStringInInvariantCulture;
	    if (!DateClass.TryParseRequestQueryDates(Request, out beginDateFromQueryStringInInvariantCulture, out endDateFromQueryStringInInvariantCulture))
	        Response.Redirect(hlMain.NavigateUrl);

	    UserStatisticsControl.ShowStatistics(user, beginDateFromQueryStringInInvariantCulture, endDateFromQueryStringInInvariantCulture);

        var dateTimeFormatInfo = CultureInfo.CurrentCulture;
	    DateTime begin;
        if (!DateTime.TryParse(beginDateFromQueryStringInInvariantCulture.ToString(), dateTimeFormatInfo, DateTimeStyles.None, out begin))
            return;
	    DateTime end;
        if (!DateTime.TryParse(endDateFromQueryStringInInvariantCulture.ToString(), dateTimeFormatInfo, DateTimeStyles.None, out end))
            return;
	    UserStatisticsFromCurrentDateTextBox.Text = begin.ToShortDateString();
	    UserStatisticsToCurrentDateTextBox.Text = end.ToShortDateString();
	}

    protected void GetUserStatisticsButtonOnClick(object sender, EventArgs e)
    {
        if (CurrentUser == null || CurrentUser.ID == null)
            return;

        UserStatisticsControl.UserID = CurrentUser.ID;
        DateTime beginDateTimeInCurrentCulture, endDateTimeInCurrentCulture;
        var dateTimeFormatInfo = CultureInfo.CurrentCulture;
        if (!DateTime.TryParse(UserStatisticsFromCurrentDateTextBox.Text, dateTimeFormatInfo, DateTimeStyles.None, out beginDateTimeInCurrentCulture))
            return;
        if (!DateTime.TryParse(UserStatisticsToCurrentDateTextBox.Text, dateTimeFormatInfo, DateTimeStyles.None, out endDateTimeInCurrentCulture))
            return;

        var beginDateStringInInvariantCulture = beginDateTimeInCurrentCulture.ToString(CultureInfo.InvariantCulture);
        var endDateStringInInvariantCulture = endDateTimeInCurrentCulture.ToString(CultureInfo.InvariantCulture);
        DateTime beginDateConvertedToInvariantCulture;
        if (!DateTime.TryParse(beginDateStringInInvariantCulture, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out beginDateConvertedToInvariantCulture))
            return;
        DateTime endDateConvertedToInvariantCulture;
        if (!DateTime.TryParse(endDateStringInInvariantCulture, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out endDateConvertedToInvariantCulture))
            return;

        SetUrlToRedirect(beginDateStringInInvariantCulture, endDateStringInInvariantCulture);
    }

    private void SetUrlToRedirect(string beginDate, string endDate)
    {
        int userId;
	    if (!int.TryParse(Request.QueryString["UserID"], out userId))
            Response.Redirect(hlMain.NavigateUrl);

        var url = string.Format("UserStatistics.aspx?UserID={0}&BeginDate={1}&EndDate={2}", userId, beginDate, endDate);
        Response.Redirect(url);
    }
}
