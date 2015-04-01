using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification
{
    public class DataBaseProviderWorkEvent : IProviderWorkEvent
    {
        public WorkEvent GetMainWorkEvent(Person user, DateTime date)
        {
            return WorkEvent.GetMainWorkEvent(user.ID.Value, date);
        }

        public WorkEvent GetCurrentEventOfDate(Person user, DateTime date)
        {
            return WorkEvent.GetCurrentEventOfDate(user.ID.Value, date);
        }
    }
}
