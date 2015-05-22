using System;
using System.Diagnostics;
using System.Collections.Generic;

using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.DAL
{
    /// <summary>
    /// Provider for ability system.
    /// </summary>
    public abstract class AbilityProvider : DataAccess
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static AbilityProvider m_Instance;
        
        #endregion

        protected AbilityProvider()
        {
            ConnectionString = Globals.Settings.Ability.ConnectionString;
        }

        #region Properties

        /// <summary>
        /// Instance of ability provider.
        /// </summary>
        public static AbilityProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                if (m_Instance == null)
                    m_Instance = (AbilityProvider)Activator.CreateInstance(
                        Type.GetType(Globals.Settings.Ability.ProviderType));
                return m_Instance;
            }
        }

        #endregion

        /// <summary>
        /// Creates new ability in database.
        /// </summary>
        /// <param name="abilityname">Name of ability.</param>
        /// <returns>ID of new database record.</returns>
        public abstract int CreateAbility(string abilityname);

        /// <summary>
        /// Returns all abilities for current user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        public abstract IList<Ability> GetAllUserAbilities(int userID);
    }
}