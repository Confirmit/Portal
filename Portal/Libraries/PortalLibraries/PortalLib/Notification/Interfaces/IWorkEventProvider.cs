using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification
{
    public interface IWorkEventProvider
    {
        WorkEvent GetMainWorkEvent(Person user, DateTime date);

        WorkEvent GetCurrentEventOfDate(Person user, DateTime date);
    }
}
