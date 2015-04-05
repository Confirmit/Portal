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
        private bool _isNotifyUser;
        private bool _isNotifyByDate;
        public TestControllerNotification(bool isNotifyUser, bool isNotifyByDate)
        {
            _isNotifyUser = isNotifyUser;
            _isNotifyByDate = isNotifyByDate;
        }
        public bool IsNotify(Person user)
        {
            return _isNotifyUser;
        }

        public bool IsNotify(DateTime date)
        {
            return _isNotifyByDate;
        }
    }
}
