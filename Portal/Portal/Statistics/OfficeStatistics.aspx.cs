using System;
using System.Globalization;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_OfficeStatistics : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime begin;
        DateTime end;
        if (!DateClass.TryParseRequestQueryDates(Request, out begin, out end))
            Response.Redirect(hlMain.NavigateUrl);

        var dateTimeFormatInfo = CultureInfo.CurrentCulture;
        if (!DateTime.TryParse(begin.ToString(), dateTimeFormatInfo, DateTimeStyles.None, out begin))
            return;
        if (!DateTime.TryParse(end.ToString(), dateTimeFormatInfo, DateTimeStyles.None, out end))
            return;

        var beginDateStringInInvariantCulture = begin.ToString(CultureInfo.InvariantCulture);
        var endDateStringInInvariantCulture = end.ToString(CultureInfo.InvariantCulture);
        DateTime beginDateConvertedToInvariantCulture;
        if (!DateTime.TryParse(beginDateStringInInvariantCulture, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out beginDateConvertedToInvariantCulture))
            return;
        DateTime endDateConvertedToInvariantCulture;
        if (!DateTime.TryParse(endDateStringInInvariantCulture, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out endDateConvertedToInvariantCulture))
            return;

        officeStat.ShowStatistics(beginDateConvertedToInvariantCulture, endDateConvertedToInvariantCulture);
    }
}
