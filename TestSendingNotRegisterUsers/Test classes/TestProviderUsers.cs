using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestSendingNotRegisterUsers.Test_classes
{
    public class TestProviderUsers : IUsersProvider
    {
        public int NumberUsers { get; private set; }

        public TestProviderUsers(int numberUsers)
        {
            NumberUsers = numberUsers;
        }
        public IList<Person> GetAllEmployees()
        {
            var listUsers = new List<Person>();
            for (int i = 0; i < NumberUsers; i++)
            {
                listUsers.Add(new Person() { PrimaryEMail = i.ToString() });
            }
            return listUsers;
        }
    }
}
