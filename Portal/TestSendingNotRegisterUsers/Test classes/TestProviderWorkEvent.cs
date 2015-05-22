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
    public class TestProviderWorkEvent : IWorkEventProvider
    {
        private WorkEvent _mainEvent;
        private WorkEvent _currenEvent;

        public TestProviderWorkEvent(WorkEvent mainEvent, WorkEvent currentEvent)
        {
            _mainEvent = mainEvent;
            _currenEvent = currentEvent;
        }
        public WorkEvent GetMainWorkEvent(Person user, DateTime date)
        {
            return _mainEvent;
        }

        public WorkEvent GetCurrentEventOfDate(Person user, DateTime date)
        {
            return _currenEvent;
        }
    }
}
