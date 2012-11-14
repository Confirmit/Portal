using System;
using System.Collections.Generic;
using System.Diagnostics;

using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
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

        #region Books

        /// <summary>
        /// Returns page of sorted and filtered books.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Page of sorted and filtered books.</returns>
        public abstract Book[] GetBooks(BookFilter filter, string sortExpr, int pageIndex, int pageSize);

        /// <summary>
        /// Returns number of records in database.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <returns>Number of records in database.</returns>
        public abstract int GetBooksCount(BookFilter filter);

        #endregion

        #region Books Themes

        /// <summary>
        /// Returns all themes of book.
        /// </summary>
        /// <param name="bookId">Book ID.</param>
        /// <returns>All themes of book.</returns>
        public abstract BookTheme[] GetBookThemes(int bookId);

        /// <summary>
        /// Sets all themes of book.
        /// </summary>
        /// <param name="bookId">Book ID.</param>
        /// <param name="themeIDs">IDs of themes.</param>
        /// <returns>True if themes were set for a book; false, otherwise.</returns>
        public abstract bool SetBookThemes(int bookId, int[] themeIDs);

        #endregion

        #region Disks

        /// <summary>
        /// Returns page of sorted and filtered disks.
        /// </summary>
        /// <param name="filter">Filter of disks.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Page of sorted and filtered disks.</returns>
        public abstract Disk[] GetDisks(DiskFilter filter, string sortExpr, int pageIndex, int pageSize);

        /// <summary>
        /// Returns number of records in database.
        /// </summary>
        /// <param name="filter">Filter of disks.</param>
        /// <returns>Number of records in database.</returns>
        public abstract int GetDisksCount(DiskFilter filter);

        #endregion

        #region Cards

        /// <summary>
        /// Returns page of sorted and filtered cards.
        /// </summary>
        /// <param name="filter">Filter of cards.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Page of sorted and filtered cards.</returns>
        public abstract Card[] GetCards(CardFilter filter, string sortExpr, int pageIndex, int pageSize);

        /// <summary>
        /// Returns number of records in database.
        /// </summary>
        /// <param name="filter">Filter of cards.</param>
        /// <returns>Number of records in database.</returns>
        public abstract int GetCardsCount(CardFilter filter);

        #endregion

        #region [ Request support ]

        public abstract void CreateRequest(int ObjectID, int? UserID, DateTime Date, bool IsTaken);

        public abstract void DeleteAllObjectRequests(int ObjectID);

        public abstract string GetOwnerName(int ObjectID, out int? OwnerID);

        public abstract string GetHolderName(int ObjectID, out int? HolderID);

        #endregion

        #region [ RequestObject support ]

        public abstract IList<RequestObject> GetFilteredRequestObjects(Type reqObjType, RequestObjectFilter filter, string sortExpr, int maximumRows, int startRowIndex);

        public abstract int GetFilteredRequestObjectsCount(Type reqObjType, RequestObjectFilter filter);

        #endregion

        #region [ RequestObjectHistory support ]

        public abstract System.Data.DataSet GetRequestObjectHistory(int objectID, string sortExpr, int maximumRows, int startRowIndex);

        public abstract int GetRequestObjectHistoryCount(int objectID);

        #endregion

        #region [ BookThemes support ]

        public abstract IList<BookTheme> GetAllBookThemes(string sortExpr, int maximumRows, int startRowIndex);

        public abstract int GetAllBookThemesCount();

        #endregion
    }
}