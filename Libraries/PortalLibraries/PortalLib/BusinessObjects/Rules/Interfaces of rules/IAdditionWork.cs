using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IAdditionWork : IRule
    {
        string DayOfWeek { get; set; }
        TimeSpan Interval { get; set; }
    }
}
