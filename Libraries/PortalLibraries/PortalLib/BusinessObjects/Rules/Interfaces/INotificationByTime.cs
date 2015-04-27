using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    interface INotificationByTime : IRule
    {
        string Information { get; set; }
        DateTime Time { get; set; }
        string DayOfWeek { get; set; }
    }
}
