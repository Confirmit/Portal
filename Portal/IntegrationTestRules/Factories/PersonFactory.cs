using System.Collections;
using System.Collections.Generic;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

namespace IntegrationTestRules
{
    public class PersonFactory
    {
        public List<Person> GetUsers(int count)
        {
            List<Person> users = new List<Person>();

            for (int i = 0; i < count; i++)
            {
                var user = new Person()
                {
                    LastName = new MLText("en", i.ToString()),
                    Sex = Person.UserSex.Female,
                    LongServiceEmployees = true,
                    PersonnelReserve = true,
                    EmployeesUlterSYSMoscow = true
                };
                users.Add(user);
            }
            return users;
        }
    }
}