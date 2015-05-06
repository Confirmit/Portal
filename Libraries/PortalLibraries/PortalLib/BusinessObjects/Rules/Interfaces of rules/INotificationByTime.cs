using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface INotificationByTime : IRule
    {
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        string Information { get; set; }
        DateTime Time { get; set; }
        string DayOfWeek { get; set; }
    }
}
