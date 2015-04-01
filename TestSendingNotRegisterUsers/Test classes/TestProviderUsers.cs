using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestProviderUsers : IProviderUsers
    {
        private const int NumberUsers = 5;
        public IList<Person> GetAllEmployees()
        {
            var listUsers = new List<Person>();
            for (int i = 0; i < NumberUsers; i++)
            {
                listUsers.Add(new Person());
            }
            return listUsers;
        }
    }
}
