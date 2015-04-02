using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestProviderWorkEventYestAndCurrent : IProviderWorkEvent
    {
        public WorkEvent GetMainWorkEvent(Person user, DateTime date)
        {
            return null;
        }

        public WorkEvent GetCurrentEventOfDate(Person user, DateTime date)
        {
            return null;
        }
    }
}
