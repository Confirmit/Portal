using System.Collections.Generic;
using Core;

namespace Migration.Utility.PersonNameType
{
    public static class PersonConverter
    {
        const string En = "en";
        const string Ru = "ru";

        public static IList<Person<MLString>> ConvertPerson(params Person<MLText>[] persons)
        {
            var result = new List<Person<MLString>>();
            foreach (var person in persons)
            {
                result.Add(ConvertPerson(person));
            }

            return result;
        }

        public static IList<Person<MLText>> ConvertPerson(params Person<MLString>[] persons)
        {
            var result = new List<Person<MLText>>();
            foreach (var person in persons)
            {
                result.Add(ConvertPerson(person));
            }

            return result;
        }

        private static Person<MLString> ConvertPerson(Person<MLText> person)
        {
            var result = new Person<MLString>
            {
                FirstName = new MLString(person.FirstName[Ru], person.FirstName[En]),
                MiddleName = new MLString(person.MiddleName[Ru], person.MiddleName[En]),
                LastName = new MLString(person.LastName[Ru], person.LastName[En])
            };
            CopyParameters(person, result);

            return result;
        }

        private static Person<MLText> ConvertPerson(Person<MLString> person)
        {
            var result = new Person<MLText>
            {
                FirstName = new MLText(Ru, person.FirstName.RussianValue, En, person.FirstName.EnglishValue),
                MiddleName = new MLText(Ru, person.MiddleName.RussianValue, En, person.MiddleName.EnglishValue),
                LastName = new MLText(Ru, person.LastName.RussianValue, En, person.LastName.EnglishValue)
            };
            CopyParameters(person, result);

            return result;
        }

        private static void CopyParameters<TS, TD>(Person<TS> sourcePerson, Person<TD> distPerson)
        {
            distPerson.SexID = sourcePerson.SexID;
            distPerson.Birthday = sourcePerson.Birthday;
            distPerson.PrimaryEMail = sourcePerson.PrimaryEMail;
            distPerson.Project = sourcePerson.Project;
            distPerson.Room = sourcePerson.Room;
            distPerson.PrimaryIP = sourcePerson.PrimaryIP;
            distPerson.LongServiceEmployees = sourcePerson.LongServiceEmployees;
            distPerson.PersonnelReserve = sourcePerson.PersonnelReserve;
            distPerson.EmployeesUlterSYSMoscow = sourcePerson.EmployeesUlterSYSMoscow;
            distPerson.ID = sourcePerson.ID;
        }
    }
}
