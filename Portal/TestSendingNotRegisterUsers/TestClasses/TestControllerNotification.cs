using System;
using ConfirmIt.PortalLib.Notification;
using ConfirmIt.PortalLib.Notification.Interfaces;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.TestClasses
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
