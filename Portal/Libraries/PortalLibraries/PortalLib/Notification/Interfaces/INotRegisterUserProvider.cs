using System;
using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification.Interfaces
{
    public interface INotRegisterUserProvider
    {
        IList<Person> GetNotRegisterUsers(DateTime datetime);
        IList<Person> GetUsersWithShortMainWork(DateTime datetime, TimeSpan minDurationMainWork);
    }
}