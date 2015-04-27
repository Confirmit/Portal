using System;
using System.Web.UI;

public partial class Calendar : UserControl
{
    #region Properties

    public String TargetControlID
    {
        set { calendar.TargetControlID = value; }
    }

    public DateTime? SelectedDate
    {
        set { calendar.SelectedDate = value; }
        get { return calendar.SelectedDate; }
    }

    public String Format
    {
        set { calendar.Format = value; }
        get { return calendar.Format; }
    }

    public bool Enabled
    {
        set
        {
            calendar.Enabled = value;
            imgCalendar.Enabled = value;
        }
        get { return calendar.Enabled; }
    }

    #endregion
}
