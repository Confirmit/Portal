using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestOfImplementersOfRules.Factories
{
    public class PersonFactory
    {
        private const int defaultNumber = 5;

        public List<Person> GetPersons(int number = defaultNumber)
        {
            var persons = new List<Person>();
            for (int i = 0; i < number; i++)
            {
                persons.Add(new Person{ID = i});
            }
            return persons;
        }
    }
}