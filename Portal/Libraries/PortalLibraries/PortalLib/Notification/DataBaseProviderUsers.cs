using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Notification
{
    public class DataBaseProviderUsers : IUsersProvider
    {
        public IList<Person> GetAllEmployees()
        {
            return UserList.GetEmployeeList();
        }
    }
}
