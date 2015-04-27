using System;
using System.Text;
using System.Collections.Generic;

using Core;
using Core.ORM.Attributes;

using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.DAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects
{
    /// <summary>
    /// Book class.
    /// </summary>
    [DBTable("Books_Books", true)]
    public class Book : RequestObject
    {
        #region Properties

        /// <summary>
        /// Authors of book.
        /// </summary>
        [DBRead("Authors")]
        public string Authors { get; set; }

        /// <summary>
        /// Publishing year of book.
        /// </summary>
        [DBRead("PublishingYear")]
        public int PublishingYear { get; set; }

        /// <summary>
        /// Annotation of book.
        /// </summary>
        [DBRead("Annotation")]
        public string Annotation { get; set; }

        /// <summary>
        /// Language of book.
        /// </summary>
        [DBRead("Language")]
        public string Language { get; set; }

        /// <summary>
        /// Download link for book.
        /// </summary>
        [DBRead("DownloadLink")]
        [DBNullable]
        public string DownloadLink { get; set; }

        /// <summary>
        /// Is this book electronic.
        /// </summary>
        [DBRead("IsElectronic")]
        public bool IsElectronic { get; set; }

        /// <summary>
        /// Themes of book.
        /// </summary>
        public BookTheme[] Themes
        {
            get
            {
                if (ID <= 0 || ID == null)
                    return new BookTheme[0];

                return BookTheme.GetBookThemes(ID.Value);
            }
        }

        /// <summary>
        /// Text representation of book themes.
        /// </summary>
        public string ThemesText
        {
            get
            {
                BookTheme[] themes = Themes;
                if (themes == null || themes.Length == 0)
                    return string.Empty;

                var themesText = new StringBuilder();
                foreach (BookTheme theme in themes)
                {
                    if (themesText.Length > 0)
                        themesText.Append(", ");

                    themesText.Append(theme.Name.ToString());
                }

                return themesText.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns languages of books.
        /// </summary>
        /// <returns>Array of languages of books.</returns>
        public static string[] GetLanguages()
        {
            return Globals.Settings.RequestObjects.BooksLanguages.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Creates new book.
        /// </summary>
        /// <param name="authors">Authors.</param>
        /// <param name="title">Title.</param>
        /// <param name="publishingYear">Publishing year.</param>
        /// <param name="annotation">Annotation.</param>
        /// <param name="language">Language.</param>
        /// <param name="officeID">ID of office where book is.</param>
        /// <param name="downloadLink">Download link.</param>
        /// <param name="isElectronic">Is this book electronic.</param>
        /// <param name="themes">IDs of themes of book.</param>
        /// <returns>ID of new book.</returns>
        public static int CreateBook(string authors, string title, int publishingYear, string annotation, string language,
                                        int officeID, int? ownerID, string downloadLink, bool isElectronic, int[] themes)
        {
            var book = new Book
            {
                Authors = authors,
                Title = title,
                PublishingYear = publishingYear,
                Annotation = annotation,
                Language = language,
                OfficeID = officeID,
                OwnerID = ownerID,
                DownloadLink = downloadLink,
                IsElectronic = isElectronic
            };
            book.Save();

            if (book.ID > 0 && themes != null && themes.Length > 0)
                SetThemes(book.ID.Value, themes);

            return book.ID.Value;
        }

        /// <summary>
        /// Updates book.
        /// </summary>
        /// <param name="id">ID of book.</param>
        /// <param name="authors">Authors.</param>
        /// <param name="title">Title.</param>
        /// <param name="publishingYear">Publishing year.</param>
        /// <param name="annotation">Annotation.</param>
        /// <param name="language">Language.</param>
        /// <param name="officeID">ID of office where book is.</param>
        /// <param name="downloadLink">Download link.</param>
        /// <param name="isElectronic">Is this book electronic.</param>
        public static void UpdateBook(int id, string authors, string title, int publishingYear, string annotation,
                                        string language, int officeID, int? ownerID, string downloadLink, bool isElectronic)
        {
            var book = new Book();
            book.Load(id);

            book.Authors = authors;
            book.Title = title;
            book.PublishingYear = publishingYear;
            book.Annotation = annotation;
            book.Language = language;
            book.OfficeID = officeID;
            book.OwnerID = ownerID;
            book.DownloadLink = downloadLink;
            book.IsElectronic = isElectronic;

            book.Save();
        }

        /// <summary>
        /// Deletes book.
        /// </summary>
        /// <param name="id">ID of book.</param>
        public static void DeleteBook(int id)
        {
            DeleteObjectByID(typeof(Book), id);
            SiteProvider.RequestObjects.DeleteAllObjectRequests(id);
        }

        /// <summary>
        /// Returns page of sorted and filtered books.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="rowIndex">Index of starting row.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Array of sorted and filtered books.</returns>
        public static Book[] GetBooks(object filter, string sortExpr, int rowIndex, int pageSize)
        {
            if (pageSize == 0)
                pageSize = Globals.Settings.RequestObjects.DefaultPageSize;

            int pageIndex = (rowIndex / pageSize);

            return SiteProvider.RequestObjects.GetBooks(filter as BookFilter, sortExpr, pageIndex, pageSize);
        }

        /// <summary>
        /// Returns number of filtered books.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="rowIndex">Index of starting row.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Number of filtered books.</returns>
        public static int GetBooksCount(object filter, string sortExpr, int rowIndex, int pageSize)
        {
            return SiteProvider.RequestObjects.GetBooksCount(filter as BookFilter);
        }

        /// <summary>
        /// Returns book by ID.
        /// </summary>
        /// <param name="id">ID of book.</param>
        /// <returns>Book with given ID; null, otherwise.</returns>
        public static Book GetBookByID(int id)
        {
            var book = new Book();
            book.Load(id);

            return book;
        }

        /// <summary>
        /// Sets themes of book.
        /// </summary>
        /// <param name="bookID">Book ID.</param>
        /// <param name="themeIDs">Themes IDs.</param>
        /// <returns>True if themes was successfully set; false, otherwise.</returns>
        public static bool SetThemes(int bookID, int[] themeIDs)
        {
            return SiteProvider.RequestObjects.SetBookThemes(bookID, themeIDs);
        }

        public override IList<RequestObject> GetAllRequestObjects()
        {
            return ((BaseObjectCollection<Book>) GetObjects(typeof (Book), "Title", true, "IsElectronic", "false")).ToArray();
        }

        #endregion
    }
}