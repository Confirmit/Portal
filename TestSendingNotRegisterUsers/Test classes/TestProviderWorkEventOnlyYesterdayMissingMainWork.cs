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
    public class TestProviderWorkEventOnlyYesterdayMissingMainWork:IProviderWorkEvent
    {
        public WorkEvent GetMainWorkEvent(Person user, DateTime date)
        {
            return null;
        }

        public WorkEvent GetCurrentEventOfDate(Person user, DateTime date)
        {
            return new WorkEvent { BeginTime = DateTime.Now.AddMilliseconds(-1), EndTime = DateTime.Now };
        }
    }
}
