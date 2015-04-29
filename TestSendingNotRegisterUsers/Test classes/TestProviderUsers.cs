using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmIt.PortalLib.Notification;
using Core;
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

        public Person GetTestPerson(string firstName, string email)
        {
            var person = new Person { FirstName = new MLText("en", firstName), PrimaryEMail = email};
            return person;

        }

        public IList<Person> GetAllEmployees()
        {
            var listUsers = new List<Person>();
            for (int i = 0; i < NumberUsers; i++)
            {
                listUsers.Add(GetTestPerson(i.ToString(), i.ToString()));
            }
            return listUsers;
        }
    }
}
