using System.Collections.Generic;

using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DataSource;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.Persons
{
    public class PersonDataSource : FilteredDataSource<Person>
    {
        #region Methods

        public override IList<Person> Select(string SortExpression, int maximumRows, int startRowIndex) 
        {
            var isAscendingOrder = true;
            if (SortExpression.Contains(" DESC"))
            {
                SortExpression = SortExpression.Substring(0, SortExpression.LastIndexOf(" DESC"));
                isAscendingOrder = false;
            }
            if (SortExpression == "")
                SortExpression = "LastName";
            var pagingResult = BasePlainObject.GetObjectsPage(typeof(Person), new PagingArgs(startRowIndex / maximumRows, maximumRows, SortExpression, isAscendingOrder));
            return (IList<Person>)pagingResult.Result;
        }

        public override int SelectCount()
        {
            return SiteProvider.Users.GetFilteredUsersCount((PersonsFilter)Filter);
        }

        #endregion
    }
}