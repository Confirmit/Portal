using System;
using System.Collections.Generic;
using System.Diagnostics;

using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL;
using System.Data;

namespace ConfirmIt.PortalLib.DAL
{
    /// <summary>
    /// Provider for objects system.
    /// </summary>
    public abstract class RequestObjectsProvider : DataAccess
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static RequestObjectsProvider m_Instance;
        
        #endregion

        #region Properties

        /// <summary>
        /// Instance of books provider.
        /// </summary>
        public static RequestObjectsProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                if (m_Instance == null)
                    m_Instance = (RequestObjectsProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.RequestObjects.ProviderType));
                
                return m_Instance;
            }
        }

        public abstract string BooksTable { get; }
        public abstract string DisksTable { get; }
        public abstract string CardsTable { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestObjectsProvider()
        {
            this.ConnectionString = Globals.Settings.RequestObjects.ConnectionString;
        }

        #endregion

       

       

       

        #region [ Request support ]

        public abstract void CreateRequest(int ObjectID, int? UserID, DateTime Date, bool IsTaken);

        public abstract void DeleteAllObjectRequests(int ObjectID);

        public abstract string GetOwnerName(int ObjectID, out int? OwnerID);

        public abstract string GetHolderName(int ObjectID, out int? HolderID);

        #endregion

       

        #region [ RequestObjectHistory support ]

        public abstract System.Data.DataSet GetRequestObjectHistory(int objectID, string sortExpr, int maximumRows, int startRowIndex);

        public abstract int GetRequestObjectHistoryCount(int objectID);

        #endregion

        
    }
}