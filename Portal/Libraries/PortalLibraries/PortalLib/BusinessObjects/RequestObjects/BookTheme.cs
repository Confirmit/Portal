using System.Collections.Generic;
using System.Diagnostics;

using ConfirmIt.PortalLib.DAL;

using Core;
using Core.ORM.Attributes;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects
{
    /// <summary>
    /// Theme of book class.
    /// </summary>
    [DBTable("Books_Themes")]
    public class BookTheme : BasePlainObject
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Dictionary<int, BookTheme> m_Cache = new Dictionary<int, BookTheme>();
        
        #endregion

        #region Properties

        /// <summary>
        /// Name of book theme.
        /// </summary>
        [DBRead("Name")]
        public MLText Name { get; set; }

        #endregion

        public BookTheme()
        {
            Name = new MLText();
        }

        #region Methods

        /// <summary>
        /// Creates new book theme.
        /// </summary>
        /// <param name="name">Name of book theme.</param>
        public static void CreateBookTheme(MLText name)
        {
            BookTheme theme = new BookTheme();
            theme.Name = name;
            theme.Save();
        }

        /// <summary>
        /// Updates existing book theme.
        /// </summary>
        /// <param name="id">ID of book theme to update.</param>
        /// <param name="name">Name of book theme.</param>
        public static void UpdateBookTheme(int id, MLText name)
        {
            BookTheme theme = new BookTheme();
            theme.Load(id);
            theme.Name = name;
            theme.Save();

            m_Cache[theme.ID.Value] = theme;
        }

        /// <summary>
        /// Deletes existing book theme.
        /// </summary>
        /// <param name="id">ID of book theme to delete.</param>
        public static void DeleteBookTheme(int id)
        {
            BookTheme.DeleteObjectByID(typeof(BookTheme), id);
            if (m_Cache.ContainsKey(id))
                m_Cache.Remove(id);
        }

        /// <summary>
        /// Returns all book themes in system.
        /// </summary>
        /// <returns>Array of all book themes.</returns>
        public static BookTheme[] GetAllBookThemes()
        {
            m_Cache.Clear();

            List<BookTheme> themes = (List<BookTheme>)BookTheme.GetObjects(typeof(BookTheme));
            foreach (BookTheme t in themes)
            {
                m_Cache[t.ID.Value] = t;
            }

            return themes.ToArray();
        }

        /// <summary>
        /// Returns all book themes in system.
        /// </summary>
        /// <returns>Array of all book themes.</returns>
        public static BookTheme[] GetAllBookThemesSorted(string sortExpr)
        {
            bool asc = true;
            if (sortExpr != null)
            {
                if (sortExpr.EndsWith("DESC"))
                    asc = false;
            }

            List<BookTheme> themes = new List<BookTheme>(GetAllBookThemes());

            themes.Sort(delegate(BookTheme x, BookTheme y) { if (asc) return string.Compare(x.Name.ToString(), y.Name.ToString()); else return string.Compare(y.Name.ToString(), x.Name.ToString()); });

            return themes.ToArray();
        }


        /// <summary>
        /// Returns book theme with given ID.
        /// </summary>
        /// <param name="id">ID of book theme to get.</param>
        /// <returns>Book theme with given ID. Null, otherwise.</returns>
        public static BookTheme GetThemeByID(int id)
        {
            if (m_Cache.ContainsKey(id))
            {
                return m_Cache[id];
            }
            else
            {
                BookTheme theme = new BookTheme();
                theme.Load(id);

                if (theme.ID != null)
                {
                    m_Cache[theme.ID.Value] = theme;

                    return theme;
                }
                else
                { return null; }
            }
        }

        /// <summary>
        /// Returns all themes of book.
        /// </summary>
        /// <param name="bookId">Book ID.</param>
        /// <returns>All themes of book.</returns>
        public static BookTheme[] GetBookThemes(int bookId)
        {
            BookTheme[] bookThemes = SiteProvider.RequestObjects.GetBookThemes(bookId);
            if (bookThemes == null || bookThemes.Length == 0)
                return new BookTheme[0];

            return bookThemes;
        }

        #endregion
    }
}