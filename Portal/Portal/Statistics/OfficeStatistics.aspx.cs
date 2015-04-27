using System;

using UlterSystems.PortalLib.BusinessObjects;

public partial class Statistics_OfficeStatistics : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime begin;
        DateTime end;
        if (!DateClass.TryParseRequestQueryDates(Request, out begin, out end))
            Response.Redirect(hlMain.NavigateUrl);

        officeStat.ShowStatistics(begin, end);
    }
}
