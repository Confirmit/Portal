using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestOfImplementersOfRules.Factories
{
    public class PersonFactory
    {
        private int _personCount = 0;

        public List<Person> GetPersons(int number)
        {
            var persons = new List<Person>();
            for (int i = 0; i < number; i++)
            {
                persons.Add(new Person { ID = _personCount++ });
            }
            return persons;
        }
    }
}