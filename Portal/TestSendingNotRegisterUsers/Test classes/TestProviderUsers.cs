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

        public Person GetTestPerson(int identificator)
        {
            var person = new Person
            {
                FirstName = new MLText("en", identificator.ToString()),
                LastName = new MLText("en", identificator.ToString()),
                MiddleName = new MLText("en", identificator.ToString()),
                ID = identificator
            };
            person.PrimaryEMail = person.FullName;
            return person;

        }

        public IList<Person> GetAllEmployees()
        {
            var listUsers = new List<Person>();
            for (int i = 0; i < NumberUsers; i++)
            {
                listUsers.Add(GetTestPerson(i));
            }
            return listUsers;
        }
    }
}
