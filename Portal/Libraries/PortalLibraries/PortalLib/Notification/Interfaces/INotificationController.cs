using System;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface INotificationController
    {
        bool IsNotified(Person user);
        bool IsNotified(DateTime date);
    }
}
