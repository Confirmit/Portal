using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IAdditionWork : IRule
    {
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        string DayOfWeek { get; set; }
        TimeSpan Interval { get; set; }
    }
}
