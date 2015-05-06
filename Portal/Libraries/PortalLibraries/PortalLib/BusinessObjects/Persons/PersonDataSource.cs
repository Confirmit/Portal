using System.Collections.Generic;

using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DataSource;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.Persons
{
    public class PersonDataSource : FilteredDataSource<Person>
    {
        #region Methods

        public override IList<Person> Select(string SortExpression, int maximumRows, int startRowIndex) 
        {
            return SiteProvider.Users.GetFilteredUsers(SortExpression, maximumRows, startRowIndex, (PersonsFilter)Filter);
        }

        public override int SelectCount()
        {
            return SiteProvider.Users.GetFilteredUsersCount((PersonsFilter)Filter);
        }

        #endregion
    }
}