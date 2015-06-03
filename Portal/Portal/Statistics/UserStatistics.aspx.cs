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

        // Инициализация инвариантных DateTime из всего строкового инвариантного Query
	    DateTime beginDateFromQueryStringInInvariantCulture;
        DateTime endDateFromQueryStringInInvariantCulture;
	    if (!DateClass.TryParseRequestQueryDates(Request, out beginDateFromQueryStringInInvariantCulture, out endDateFromQueryStringInInvariantCulture))
	        Response.Redirect(hlMain.NavigateUrl);

	    UserStatisticsControl.ShowStatistics(user, beginDateFromQueryStringInInvariantCulture, endDateFromQueryStringInInvariantCulture);

        //Получение  DateTime в текущей культуре из строковых инвариантных строк
        DateTime beginDateTimeInCurrentCulture;
        DateTime endDateTimeInCurrentCulture;
        var isParsedDateTimeFromQueryInvariantCulture = InitializeDateTimeByString(beginDateFromQueryStringInInvariantCulture.ToString(),
            endDateFromQueryStringInInvariantCulture.ToString(),
            out beginDateTimeInCurrentCulture, out endDateTimeInCurrentCulture, CultureInfo.CurrentCulture);
        if (!isParsedDateTimeFromQueryInvariantCulture)
            return;

	    UserStatisticsFromCurrentDateTextBox.Text = beginDateTimeInCurrentCulture.ToShortDateString();
	    UserStatisticsToCurrentDateTextBox.Text = endDateTimeInCurrentCulture.ToShortDateString();
	}

    protected void GetUserStatisticsButtonOnClick(object sender, EventArgs e)
    {
        if (CurrentUser == null || CurrentUser.ID == null)
            return;

        UserStatisticsControl.UserID = CurrentUser.ID;

        // Инициализация DateTime в текущей культуре по каждому из строковых полей date-picker'a в текущей культуре
        DateTime beginDateTimeInCurrentCulture, endDateTimeInCurrentCulture;
        var isParsedDateTimeFromDatePickerInCurrentCulture = 
            InitializeDateTimeByString(UserStatisticsFromCurrentDateTextBox.Text,
            UserStatisticsToCurrentDateTextBox.Text, out beginDateTimeInCurrentCulture, out endDateTimeInCurrentCulture, 
            CultureInfo.CurrentCulture);
        if (!isParsedDateTimeFromDatePickerInCurrentCulture)
            return;

        // Инициализация строк в инвариантной культуре
        var beginDateStringInInvariantCulture = beginDateTimeInCurrentCulture.ToString(CultureInfo.InvariantCulture);
        var endDateStringInInvariantCulture = endDateTimeInCurrentCulture.ToString(CultureInfo.InvariantCulture);

        // Инициализация DateTime по инвариантной культуре по каждой из строк
        DateTime beginDateConvertedToInvariantCulture;
        DateTime endDateConvertedToInvariantCulture;
        var isParsedDateTimeFromDatePickerInInvariantCulture = InitializeDateTimeByString(beginDateStringInInvariantCulture, endDateStringInInvariantCulture,
            out beginDateConvertedToInvariantCulture, out endDateConvertedToInvariantCulture, CultureInfo.InvariantCulture);
        if (!isParsedDateTimeFromDatePickerInInvariantCulture)
            return;

        SetUrlToRedirect(beginDateStringInInvariantCulture, endDateStringInInvariantCulture);
    }

    private bool InitializeDateTimeByString(string beginDateString, string endDateString, 
        out DateTime beginDateTime, out  DateTime endDateTime, CultureInfo cutureInfo)
    {
        if (!DateTime.TryParse(beginDateString, cutureInfo, DateTimeStyles.None, out beginDateTime))
        {
            endDateTime = new DateTime();
            return false;
        }
        if (!DateTime.TryParse(endDateString, cutureInfo, DateTimeStyles.None, out endDateTime))
            return false;
        return true;
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
