using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestControllerNotification : IControllerNotification
    {
        public bool IsNotify(Person user)
        {
            return true;
        }

        public bool IsNotify(DateTime date)
        {
            return true;
        }
    }
}
