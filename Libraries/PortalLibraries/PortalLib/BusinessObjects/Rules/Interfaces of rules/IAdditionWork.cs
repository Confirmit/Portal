using System;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    interface IAdditionWork : IRule
    {
        string DayOfWeek { get; set; }
        TimeSpan Interval { get; set; }
    }
}
