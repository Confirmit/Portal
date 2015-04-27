using System;
using System.Collections.Generic;
using System.Diagnostics;

using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.DAL
{
    public abstract class UsersProvider : DataAccess
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static UsersProvider m_Instance;
        
        #endregion

        #region Properties

        /// <summary>
        /// Instance of users provider.
        /// </summary>
        public static UsersProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                if (m_Instance == null)
                    m_Instance = (UsersProvider)Activator.CreateInstance(
                        Type.GetType(Globals.Settings.Users.ProviderType));
                return m_Instance;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public UsersProvider()
        {
            ConnectionString = Globals.Settings.Users.ConnectionString;
        }

        #endregion

        /// <summary>
        /// Returns page of sorted users
        /// </summary>
        public abstract IList<Person> GetFilteredUsers(string SortExpression, int maximumRows, int startRowIndex, PersonsFilter filter);

        /// <summary>
        /// Get count of filtered persons.
        /// </summary>
        /// <param name="filter">Persons filter.</param>
        /// <returns>Count of filtered persons.</returns>
        public abstract int GetFilteredUsersCount(PersonsFilter filter);
    }
}
