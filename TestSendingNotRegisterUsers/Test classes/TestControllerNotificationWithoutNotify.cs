using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestControllerNotificationWithoutNotify : IControllerNotification
    {
        public bool IsNotify(Person user)
        {
            return false;
        }

        public bool IsNotify(DateTime date)
        {
            return false;
        }
    }
}
