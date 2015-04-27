using System;
using System.Diagnostics;
using System.Collections.Generic;

using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.DAL
{
    /// <summary>
    /// Provider for project system.
    /// </summary>    
    public abstract class ProjectProvider : DataAccess
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ProjectProvider m_Instance;

        #endregion

        protected ProjectProvider ()
        {
            ConnectionString = Globals.Settings.Project.ConnectionString;
        }

        #region Properties
        /// <summary>
        /// Instance of projects provider.
        /// </summary>
        public static ProjectProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                if (m_Instance == null)
                    m_Instance = (ProjectProvider)Activator.CreateInstance(
                        Type.GetType(Globals.Settings.Project.ProviderType));
                return m_Instance;
            }
        }
        #endregion

        /// <summary>
        /// Returns all projects for current user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        public abstract IList<Project> GetAllUserProjects(int userID);

        /// <summary>
        /// Returns projects for current user.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="returnCompleteProjects">True – return current projects, 
        /// false – return complete projects </param>
        public abstract IList<Project> GetUserProjects(int userID, bool returnCompleteProjects);
    }
}