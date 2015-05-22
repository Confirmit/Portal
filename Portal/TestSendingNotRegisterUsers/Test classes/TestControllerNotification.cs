using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestControllerNotification : INotificationController
    {
        private bool _isNotifiedUser;
        private bool _isNotifyByDate;
        public TestControllerNotification(bool isNotifiedUser, bool isNotifyByDate)
        {
            _isNotifiedUser = isNotifiedUser;
            _isNotifyByDate = isNotifyByDate;
        }
        public bool IsNotified(Person user)
        {
            return _isNotifiedUser;
        }

        public bool IsNotified(DateTime date)
        {
            return _isNotifyByDate;
        }
    }
}
