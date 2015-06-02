using System;
using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface INotRegisterUserProvider
    {
        List<Person> GetNotRegisterUsers(DateTime datetime);
        List<Person> GetUsersWithShortMainWork(DateTime datetime, TimeSpan minDurationMainWork);
    }
}