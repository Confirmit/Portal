using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface INotificationByTime : IRule
    {
        string Information { get; set; }
        DateTime Time { get; set; }
        string DayOfWeek { get; set; }
    }
}
