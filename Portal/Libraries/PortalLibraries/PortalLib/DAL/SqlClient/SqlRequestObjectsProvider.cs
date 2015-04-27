using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

using Core;
using Core.DB;
using Core.ORM;

using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
using ConfirmIt.PortalLib.FiltersSupport;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    /// <summary>
    /// Provider of objects data for MS SQL Server.
    /// </summary>
    public class SqlRequestObjectsProvider : RequestObjectsProvider
    {
        #region [ Constants ]

        private readonly string DBRequestObjectTableName = "RequestObject";
        private readonly string DBRequestsTableName = "Requests";

        private const string DBThemesTableName = "Books_Themes";
        private const string DBBooksTableName = "Books_Books";
        private const string DBBookThemesTableName = "Books_BookThemes";
        
        private const string DBDisksTableName = "Disks";
        private const string DBCardsTableName = "DiscountCard";
        private const string DBUsersTableName = "Users";

        #endregion

        #region Properties
        public override string BooksTable
        {
            get
            {
                return DBBooksTableName;
            }
        }
        public override string DisksTable
        {
            get
            {
                return DBDisksTableName;
            }
        }
        public override string CardsTable
        {
            get
            {
                return DBCardsTableName;
            }
        }
        #endregion

        #region Books management

        /// <summary>
        /// Returns page of sorted and filtered books.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Array of sorted and filtered books.</returns>
        public override Book[] GetBooks(BookFilter filter, string sortExpr, int pageIndex, int pageSize)
        {
            sortExpr = EnsureValidBookSortExpression(sortExpr);

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand getCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int lowerBound = pageIndex * pageSize + 1;
                    int upperBound = (pageIndex + 1) * pageSize;

                    getCommand.Transaction = transaction;
                    getCommand.CommandText =
                        string.Format(
                            "SELECT *, ROW_NUMBER() OVER (ORDER BY {3}) AS RowNum FROM {0} B LEFT JOIN {1} BT on B.ID = BT.BookID WHERE {2}",
                            DBBooksTableName, DBBookThemesTableName, whereClause, sortExpr);
                    getCommand.CommandText =
                        string.Format(
                            "SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC",
                            getCommand.CommandText, lowerBound, upperBound);

                    using (IDataReader reader = ExecuteReader(getCommand))
                    {
                        List<Book> books = GetAllBooksFromReader(reader);

                        return books.ToArray();
                    }
                }
                finally
                { transaction.Rollback(); }
            }
        }


        /// <summary>
        /// Returns number of records in database.
        /// </summary>
        /// <param name="filter">Filter of books.</param>
        /// <returns>Number of records in database.</returns>
        public override int GetBooksCount(BookFilter filter)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand countCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                countCommand.Transaction = transaction;
                countCommand.CommandText =
                    string.Format(
                        "SELECT COUNT(*) FROM {0} B LEFT JOIN {1} BT on B.ID = BT.BookID WHERE {2}",
                        DBBooksTableName, DBBookThemesTableName, whereClause);

                try
                {
                    return (int)ExecuteScalar(countCommand);
                }
                finally
                { transaction.Rollback(); }
            }
        }

        public static StringBuilder constructWhereClause(BookFilter filter)
        {
            StringBuilder whereClause = new StringBuilder("(1=1)");

            if (filter == null)
                return whereClause;

            if (!string.IsNullOrEmpty(filter.Authors))
                whereClause.AppendFormat("AND B.Authors LIKE '%' + '{0}' + '%' ", filter.Authors);

            if (!string.IsNullOrEmpty(filter.Title))
                whereClause.AppendFormat("AND B.Title LIKE '%' + '{0}' + '%' ", filter.Title);

            if (!string.IsNullOrEmpty(filter.Annotation))
                whereClause.AppendFormat("AND B.Annotation LIKE '%' + '{0}' + '%' ", filter.Annotation);

            if (filter.Themes != null)
            {
                if (filter.Themes.Count > 0)
                {
                    string themesIDs = string.Empty;
                    foreach (int theme in filter.Themes)
                    {
                        if (themesIDs != string.Empty)
                            themesIDs += ", ";
                        themesIDs += theme.ToString();
                    }

                    whereClause.AppendFormat(" AND (BT.ThemeID IN ({0}))", themesIDs);
                }
            }

            //if (filter.FromYear < 1800)
            //    filter.FromYear = 1800;
            //if (filter.FromYear > DateTime.Now.Year)
            //    filter.FromYear = DateTime.Now.Year;
            //whereClause.AppendFormat(" AND (B.PublishingYear >= {0})", filter.FromYear);

            //if (filter.ToYear < 1800)
            //    filter.ToYear = 1800;
            //if (filter.ToYear > DateTime.Now.Year)
            //    filter.ToYear = DateTime.Now.Year;
            //whereClause.AppendFormat(" AND (B.PublishingYear <= {0})", filter.ToYear);

            if (!string.IsNullOrEmpty(filter.Language))
                whereClause.AppendFormat("AND B.Language LIKE '%' + '{0}' + '%' ", filter.Language);

            if (filter.OfficeID > 0)
                whereClause.AppendFormat("AND B.OfficeID = {0}", filter.OfficeID);

            if (filter.OwnerID == null)
                whereClause.Append(" AND B.OwnerID IS NULL");
            else if (filter.OwnerID > 0)
                whereClause.AppendFormat(" AND B.OwnerID = {0}", filter.OwnerID);

            //if (filter.IsElectronic && !filter.IsPaper)
            //    whereClause.Append(" AND (B.IsElectronic = 1)");

            //if (filter.IsPaper && !filter.IsElectronic)
            //    whereClause.Append(" AND (B.IsElectronic = 0)");

            return whereClause;
        }

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string EnsureValidBookSortExpression(string sortExpr)
        {
            if (string.IsNullOrEmpty(sortExpr))
                return "Title";

            sortExpr = sortExpr.Trim().ToLower();
            if (!sortExpr.Equals("authors") && !sortExpr.Equals("authors desc") && !sortExpr.Equals("authors asc") &&
                !sortExpr.Equals("title") && !sortExpr.Equals("title desc") && !sortExpr.Equals("title asc") &&
                !sortExpr.Equals("publishingyear") && !sortExpr.Equals("publishingyear desc") && !sortExpr.Equals("publishingyear asc") &&
                !sortExpr.Equals("language") && !sortExpr.Equals("language desc") && !sortExpr.Equals("language asc") &&
                !sortExpr.Equals("iselectronic") && !sortExpr.Equals("iselectronic desc") && !sortExpr.Equals("iselectronic asc"))
            {
                return "Title";
            }
            else
            {
                if (!sortExpr.StartsWith("title"))
                {
                    sortExpr += ", Title";
                }
                return sortExpr;
            }
        }

        /// <summary>
        /// Returns book information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Book information from reader.</returns>
        protected virtual Book GetBookFromReader(IDataReader reader)
        {
            Book book = new Book();

            book.ID = (int)reader["ID"];
            book.Authors = (string)reader["Authors"];
            book.Title = (string)reader["Title"];
            book.PublishingYear = (int)reader["PublishingYear"];
            book.Annotation = (string)reader["Annotation"];
            book.Language = (string)reader["Language"];
            book.OfficeID = (int)reader["OfficeID"];

            if (reader["OwnerID"] != DBNull.Value)
                book.OwnerID = (int)reader["OwnerID"];
            else
                book.OwnerID = null;

            if (reader["DownloadLink"] != DBNull.Value)
                book.DownloadLink = (string)reader["DownloadLink"];
            else
                book.DownloadLink = null;

            book.IsElectronic = (bool)reader["IsElectronic"];

            return book;
        }

        /// <summary>
        /// Returns books information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Books information from reader.</returns>
        protected virtual List<Book> GetAllBooksFromReader(IDataReader reader)
        {
            List<Book> books = new List<Book>();

            while (reader.Read())
            {
                books.Add(GetBookFromReader(reader));
            }

            return books;
        }
        #endregion

        #region Book and themes
        /// <summary>
        /// Returns all themes of book.
        /// </summary>
        /// <param name="bookId">Book ID.</param>
        /// <returns>All themes of book.</returns>
        public override BookTheme[] GetBookThemes(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("SELECT T.ID, T.Name FROM (SELECT ThemeID FROM {0} WHERE BookID = @BookID) BT JOIN {1} T ON BT.ThemeID = T.ID",
                                        DBBookThemesTableName, DBThemesTableName);
                command.Parameters.Add("@BookID", SqlDbType.Int).Value = bookId;

                connection.Open();
                using (IDataReader reader = ExecuteReader(command))
                {
                    List<BookTheme> bookThemes = new List<BookTheme>();
                    while (reader.Read())
                    {
                        BookTheme theme = new BookTheme();
                        theme.ID = (int)reader["ID"];
                        theme.Name.LoadFromXML((string)reader["Name"]);
                        bookThemes.Add(theme);
                    }
                    return bookThemes.ToArray();
                }
            }
        }

        /// <summary>
        /// Sets all themes of book.
        /// </summary>
        /// <param name="bookId">Book ID.</param>
        /// <param name="themeIDs">IDs of themes.</param>
        /// <returns>True if themes were set for a book; false, otherwise.</returns>
        public override bool SetBookThemes(int bookId, int[] themeIDs)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand deleteCommand = connection.CreateCommand();
                    deleteCommand.Transaction = transaction;
                    deleteCommand.CommandText =
                        string.Format("DELETE FROM {0} WHERE BookID = @BookID",
                                            DBBookThemesTableName);
                    deleteCommand.Parameters.Add("@BookID", SqlDbType.Int).Value = bookId;

                    ExecuteNonQuery(deleteCommand);

                    if (themeIDs != null)
                    {
                        foreach (int themeID in themeIDs)
                        {
                            SqlCommand insertCommand = connection.CreateCommand();
                            insertCommand.Transaction = transaction;
                            insertCommand.CommandText =
                                string.Format(
                                    "INSERT INTO {0} (BookID, ThemeID) VALUES  (@BookID, @ThemeID)",
                                    DBBookThemesTableName);
                            insertCommand.Parameters.Add("@BookID", SqlDbType.Int).Value = bookId;
                            insertCommand.Parameters.Add("@ThemeID", SqlDbType.Int).Value = themeID;

                            ExecuteNonQuery(insertCommand);
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }
        #endregion

        #region Disks management

        public override Disk[] GetDisks(DiskFilter filter, string sortExpr, int pageIndex, int pageSize)
        {
            sortExpr = EnsureValidDiskSortExpression(sortExpr);

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand getCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int lowerBound = pageIndex * pageSize + 1;
                    int upperBound = (pageIndex + 1) * pageSize;

                    getCommand.Transaction = transaction;
                    getCommand.CommandText =
                        string.Format(
                            "SELECT *, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum FROM {0} D WHERE {1}",
                            DBDisksTableName, whereClause, sortExpr);
                    getCommand.CommandText =
                        string.Format(
                            "SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC",
                            getCommand.CommandText, lowerBound, upperBound);

                    using (IDataReader reader = ExecuteReader(getCommand))
                    {
                        List<Disk> Disks = GetAllDisksFromReader(reader);

                        return Disks.ToArray();
                    }
                }
                finally
                { transaction.Rollback(); }
            }
        }


        public override int GetDisksCount(DiskFilter filter)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand countCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();


                countCommand.Transaction = transaction;
                countCommand.CommandText =
                    string.Format(
                        "SELECT COUNT(*) FROM {0} D WHERE {1}",
                        DBDisksTableName, whereClause);

                try
                {
                    return (int)ExecuteScalar(countCommand);
                }
                finally
                { transaction.Rollback(); }
            }
        }

        public static StringBuilder constructWhereClause(DiskFilter filter)
        {
            StringBuilder whereClause = new StringBuilder("(1=1)");

            if (!string.IsNullOrEmpty(filter.Manufacturers))
                whereClause.AppendFormat("AND D.Manufacturers LIKE '%' + '{0}' + '%' ", filter.Manufacturers);

            if (!string.IsNullOrEmpty(filter.Title))
                whereClause.AppendFormat("AND D.Title LIKE '%' + '{0}' + '%' ", filter.Title);

            if (!string.IsNullOrEmpty(filter.Annotation))
                whereClause.AppendFormat("AND D.Annotation LIKE '%' + '{0}' + '%' ", filter.Annotation);

            if (filter.FromYear < 1800)
                filter.FromYear = 1800;
            if (filter.FromYear > DateTime.Now.Year)
                filter.FromYear = DateTime.Now.Year;
            whereClause.AppendFormat(" AND (D.PublishingYear >= {0})", filter.FromYear);

            if (filter.ToYear < 1800)
                filter.ToYear = 1800;
            if (filter.ToYear > DateTime.Now.Year)
                filter.ToYear = DateTime.Now.Year;
            whereClause.AppendFormat(" AND (D.PublishingYear <= {0})", filter.ToYear);

            if (filter.OfficeID > 0)
                whereClause.AppendFormat("AND D.OfficeID = {0}", filter.OfficeID);

            if (filter.OwnerID == null)
                whereClause.Append(" AND D.OwnerID IS NULL");
            else if (filter.OwnerID > 0)
                whereClause.AppendFormat(" AND D.OwnerID = {0}", filter.OwnerID);

            return whereClause;
        }

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string EnsureValidDiskSortExpression(string sortExpr)
        {
            if (string.IsNullOrEmpty(sortExpr))
                return "Title";

            sortExpr = sortExpr.Trim().ToLower();
            if (!sortExpr.Equals("manufacturers") && !sortExpr.Equals("manufacturers desc") && !sortExpr.Equals("manufacturers asc") &&
                !sortExpr.Equals("title") && !sortExpr.Equals("title desc") && !sortExpr.Equals("title asc") &&
                !sortExpr.Equals("publishingyear") && !sortExpr.Equals("publishingyear desc") && !sortExpr.Equals("publishingyear asc"))
            {
                return "Title";
            }
            else
            {
                if (!sortExpr.StartsWith("title"))
                {
                    sortExpr += ", Title";
                }
                return sortExpr;
            }
        }

        /// <summary>
        /// Returns Disk information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Disk information from reader.</returns>
        protected virtual Disk GetDiskFromReader(IDataReader reader)
        {
            Disk disk = new Disk();

            disk.ID = (int)reader["ID"];
            disk.Manufacturers = (string)reader["Manufacturers"];
            disk.Title = (string)reader["Title"];
            disk.PublishingYear = (int)reader["PublishingYear"];
            disk.Annotation = (string)reader["Annotation"];
            disk.OfficeID = (int)reader["OfficeID"];

            if (reader["OwnerID"] != DBNull.Value)
                disk.OwnerID = (int)reader["OwnerID"];
            else
                disk.OwnerID = null;

            return disk;
        }

        /// <summary>
        /// Returns Disks information from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Disks information from reader.</returns>
        protected virtual List<Disk> GetAllDisksFromReader(IDataReader reader)
        {
            List<Disk> Disks = new List<Disk>();

            while (reader.Read())
            {
                Disks.Add(GetDiskFromReader(reader));
            }

            return Disks;
        }
        #endregion

        #region Cards management

        public override Card[] GetCards(
            CardFilter filter,
            string sortExpr,
            int pageIndex,
            int pageSize)
        {
            sortExpr = EnsureValidCardSortExpression(sortExpr);

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand getCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int lowerBound = pageIndex * pageSize + 1;
                    int upperBound = (pageIndex + 1) * pageSize;

                    getCommand.Transaction = transaction;
                    getCommand.CommandText =
                        string.Format(
                            "SELECT *, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum FROM {0} C WHERE {1}",
                            DBCardsTableName, whereClause, sortExpr);
                    getCommand.CommandText =
                        string.Format(
                            "SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC",
                            getCommand.CommandText, lowerBound, upperBound);

                    using (IDataReader reader = ExecuteReader(getCommand))
                    {
                        List<Card> Cards = GetAllCardsFromReader(reader);

                        return Cards.ToArray();
                    }
                }
                finally
                { transaction.Rollback(); }
            }
        }


        public override int GetCardsCount(CardFilter filter)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand countCommand = connection.CreateCommand();

                // Construct WHERE clause.
                StringBuilder whereClause = constructWhereClause(filter);

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();


                countCommand.Transaction = transaction;
                countCommand.CommandText =
                    string.Format(
                        "SELECT COUNT(*) FROM {0} C WHERE {1}",
                        DBCardsTableName, whereClause);

                try
                {
                    return (int)ExecuteScalar(countCommand);
                }
                finally
                { transaction.Rollback(); }
            }
        }

        public static StringBuilder constructWhereClause(CardFilter filter)
        {
            StringBuilder whereClause = new StringBuilder("(1=1)");

            if (!string.IsNullOrEmpty(filter.Title))
                whereClause.AppendFormat("AND C.Title LIKE '%' + '{0}' + '%' ", filter.Title);

            if (filter.ValuePercent > 0)
                whereClause.AppendFormat("AND C.ValuePercent = {0}", filter.ValuePercent);

            if (!string.IsNullOrEmpty(filter.ShopName))
                whereClause.AppendFormat("AND C.ShopName LIKE '%' + '{0}' + '%' ", filter.ShopName);

            if (!string.IsNullOrEmpty(filter.ShopSite))
                whereClause.AppendFormat("AND C.ShopSite LIKE '%' + '{0}' + '%' ", filter.ShopSite);

            if (filter.OfficeID > 0)
                whereClause.AppendFormat("AND C.OfficeID = {0}", filter.OfficeID);

            if (filter.OwnerID == null)
                whereClause.Append(" AND C.OwnerID IS NULL");
            else if (filter.OwnerID > 0)
                whereClause.AppendFormat(" AND C.OwnerID = {0}", filter.OwnerID);

            return whereClause;
        }

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string EnsureValidCardSortExpression(string sortExpr)
        {
            if (string.IsNullOrEmpty(sortExpr))
                return "Title";

            sortExpr = sortExpr.Trim().ToLower();
            if (!sortExpr.Equals("valuePercent") && !sortExpr.Equals("valuePercent desc") && !sortExpr.Equals("valuePercent asc") &&
                !sortExpr.Equals("title") && !sortExpr.Equals("title desc") && !sortExpr.Equals("title asc") &&
                !sortExpr.Equals("shopname") && !sortExpr.Equals("shopname desc") && !sortExpr.Equals("shopname asc") &&
                !sortExpr.Equals("shopsite") && !sortExpr.Equals("shopsite desc") && !sortExpr.Equals("shopsite asc"))
            {
                return "Title";
            }
            else
            {
                if (!sortExpr.StartsWith("title"))
                {
                    sortExpr += ", Title";
                }
                return sortExpr;
            }
        }

        /// <summary>
        /// Returns card from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Card from reader.</returns>
        protected virtual Card GetCardFromReader(IDataReader reader)
        {
            Card card = new Card();

            card.ID = (int)reader["ID"];
            card.Title = (string)reader["Title"];
            card.ValuePercent = (byte)reader["ValuePercent"];

            if (reader["ShopName"] != DBNull.Value)
                card.ShopName = (string)reader["ShopName"];
            else
                card.ShopName = null;

            if (reader["ShopSite"] != DBNull.Value)
                card.ShopSite = (string)reader["ShopSite"];
            else
                card.ShopSite = null;

            card.OfficeID = (int)reader["OfficeID"];

            if (reader["OwnerID"] != DBNull.Value)
                card.OwnerID = (int)reader["OwnerID"];
            else
                card.OwnerID = null;

            return card;
        }

        /// <summary>
        /// Returns cards from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Cards from reader.</returns>
        protected virtual List<Card> GetAllCardsFromReader(IDataReader reader)
        {
            List<Card> cards = new List<Card>();

            while (reader.Read())
            {
                cards.Add(GetCardFromReader(reader));
            }

            return cards;
        }
        #endregion

        #region [ Request management ]

        public override void CreateRequest(int ObjectID, int? UserID, DateTime Date, bool IsTaken)
        {
            Procedure procedure = new Procedure("CreateRequest");
            if (UserID == null)
                procedure.Add("@UserID", DBNull.Value);
            else
                procedure.Add("@UserID", UserID);

            procedure.Add("@ObjectID", ObjectID);
            procedure.Add("@Date", Date);
            procedure.Add("@IsTaken", IsTaken);
            procedure.ExecNonQuery();
        }

        public override void DeleteAllObjectRequests(int ObjectID)
        {
            Query command = new Query(string.Format("DELETE FROM [{0}] WHERE ObjectID = @ObjectID", DBRequestsTableName));
            command.Add("@ObjectID", SqlDbType.Int).Value = ObjectID;
            command.ExecNonQuery();
        }

        public override string GetOwnerName(int ObjectID, out int? OwnerID)
        {
            string OwnerName = string.Empty;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                StringBuilder query = new StringBuilder();
                query.Append("SELECT FirstName, ");
                query.Append("CASE WHEN LastName IS NULL ");
                query.Append("THEN '<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
                query.Append("ELSE LastName END as LastName, ");
                query.AppendFormat("OwnerID FROM [{0}] ", DBRequestObjectTableName);
                query.AppendFormat("LEFT JOIN [{0}] ON [{1}].OwnerID = [{0}].ID ", DBUsersTableName, DBRequestObjectTableName);
                query.AppendFormat("WHERE [{0}].ID = @ObjectID ", DBRequestObjectTableName);

                command.CommandText = query.ToString();
                command.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID;

                using (IDataReader reader = ExecuteReader(command))
                {
                    reader.Read();
                    MLText firstname = new MLText();
                    MLText lastname = new MLText();

                    if (reader["FirstName"] != DBNull.Value)
                        firstname.LoadFromXML(reader["FirstName"].ToString());

                    lastname.LoadFromXML(reader["LastName"].ToString());

                    if (!string.IsNullOrEmpty(firstname.ToString()))
                        OwnerName += firstname.ToString() + " ";
                    OwnerName += lastname.ToString();

                    OwnerID = (reader["OwnerID"] != DBNull.Value)
                                                    ? (int?)reader["OwnerID"] : null;
                }
            }
            return OwnerName;
        }

        public override string GetHolderName(int ObjectID, out int? HolderID)
        {
            string HolderName = string.Empty;

            StringBuilder queryText = new StringBuilder();
            queryText.Append("DECLARE @HolderID int ");
            queryText.Append("SET @HolderID = [dbo].GetRequestObjectHolderID(@ObjectID) ");
            queryText.Append("IF (@HolderID IS NULL) ");
            queryText.Append("BEGIN ");
            queryText.Append("SELECT NULL as FirstName, ");
            queryText.Append("'<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
            queryText.Append("as LastName, null as holderID ");
            queryText.Append("END ");
            queryText.Append("ELSE ");
            queryText.Append("BEGIN ");
            queryText.Append("SELECT FirstName, LastName, ID as holderID ");
            queryText.AppendFormat("FROM [{0}] ", DBUsersTableName);
            queryText.AppendFormat("WHERE [{0}].ID = @HolderID ", DBUsersTableName);
            queryText.Append("END ");

            Query query = new Query(queryText.ToString());
            query.Add("@ObjectID", ObjectID);

            using (IDataReader reader = query.ExecReader())
            {
                reader.Read();
                MLText firstname = new MLText();
                MLText lastname = new MLText();

                if (reader["FirstName"] != DBNull.Value)
                    firstname.LoadFromXML(reader["FirstName"].ToString());
                lastname.LoadFromXML(reader["LastName"].ToString());

                if (!string.IsNullOrEmpty(firstname.ToString()))
                    HolderName += firstname.ToString() + " ";
                HolderName += lastname.ToString();

                if (reader["holderID"] != DBNull.Value)
                    HolderID = (int)reader["holderID"];
                else
                    HolderID = null;
            }
            return HolderName;
        }

        #endregion

        #region [ RequestObjects support ]

        /// <summary>
        /// Ensures that sorting expression is valid.
        /// </summary>
        /// <param name="sortExpr">Sorting expression.</param>
        /// <returns>Valid sorting expression.</returns>
        private static string ensureValidSortExpression(string sortExpr, Type filterType)
        {
            if (string.IsNullOrEmpty(sortExpr))
                return "Title";

            var sortingExpr = sortExpr.Split(' ')[0].Trim().ToLower();
            if (!sortingExpr.StartsWith("title"))
                sortExpr += ", Title";

            return sortExpr;
        }

        private static List<string> getPropertiesNames(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => prop.Name).ToList<string>();
        }

        #region [ GetFilteredRequestObjects ]

        public override IList<RequestObject> GetFilteredRequestObjects(Type reqObjType, RequestObjectFilter filter, string sortExpr, int maximumRows, int startRowIndex)
        {
            int lowerBound = startRowIndex + 1;
            int upperBound = startRowIndex + maximumRows;

            var queryStatement = DBFilterBuilder.GetQueryStatement(filter);
            sortExpr = ensureValidSortExpression(sortExpr, filter.GetType());
            var mappingData = ObjectPropertiesMapper.GetObjectMappingData(reqObjType, true);
            foreach (var pair in mappingData)
            {
                foreach (var fieldName in pair.Value)
                {
                    string fullFieldName = string.Format("[{0}].{1}", pair.Key, fieldName);
                    queryStatement.Fields.Add(fullFieldName);
                }
            }

            queryStatement.Fields.Add(string.Format("ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum", sortExpr));

            //var commandText = string.Format("SELECT *, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum FROM {0} {1}"
            //                                        , tableName, whereClause, sortExpr);
            var commandText = string.Format("SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC"
                                                    , queryStatement.ToString(), lowerBound, upperBound);
            
            var command = new Query(commandText);
            return GetAllObjectsFromDataTable<RequestObject>(reqObjType, command.ExecDataTable());
        }

        public override int GetFilteredRequestObjectsCount(Type reqObjType, RequestObjectFilter filter)
        {
            string tableName = DBAttributesManager.GetDBTableName(reqObjType);
            var queryStatement = DBFilterBuilder.GetQueryStatement(filter);
            queryStatement.Fields.Add("COUNT(*)");

            //string whereClause = DBFilterBuilder.BuildFilterExpression(filter, tableName);

            var command = new Query(queryStatement.ToString());//string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", tableName, whereClause));
            return (int)command.ExecScalar();
        }

        #endregion

        #endregion

        #region [ RequestObject History support ]

        public override DataSet GetRequestObjectHistory(int objectID, string sortExpr, int maximumRows, int startRowIndex)
        {
            int lowerBound = startRowIndex + 1;
            int upperBound = startRowIndex + maximumRows;

            if (string.IsNullOrEmpty(sortExpr))
                sortExpr = "Date";

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT [{0}].ID, [{1}].FirstName, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum, "
                , DBRequestsTableName, DBUsersTableName, sortExpr);
            sbQuery.AppendFormat("CASE WHEN [{0}].LastName is null ", DBUsersTableName);
            sbQuery.Append("THEN '<MLText><Text lang=\"en\">Office</Text><Text lang=\"ru\">ќфис</Text></MLText>' ");
            sbQuery.AppendFormat("ELSE [{0}].LastName ", DBUsersTableName);
            sbQuery.AppendFormat("END as LastName, [{0}].Date, [{0}].IsTaken ", DBRequestsTableName);
            sbQuery.AppendFormat("FROM [{0}] ", DBRequestsTableName);
            sbQuery.AppendFormat("LEFT JOIN [{0}] ON [{1}].UserId = [{0}].ID ", DBUsersTableName, DBRequestsTableName);
            sbQuery.AppendFormat("WHERE [{0}].ObjectID = {1} ", DBRequestsTableName, objectID);

            string commandText = string.Format("SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC"
                                                    , sbQuery.ToString(), lowerBound, upperBound);
            var command = new Query(commandText);
            return command.ExecDataSet();
        }

        public override int GetRequestObjectHistoryCount(int objectID)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT COUNT(*) FROM {0} ", DBRequestsTableName);
            sbQuery.AppendFormat("WHERE ObjectID = {0} ", objectID);

            var query = new Query(sbQuery.ToString());
            return (int)query.ExecScalar();
        }

        #endregion

        #region [ BookThemes ]

        public override IList<BookTheme> GetAllBookThemes(string sortExpr, int maximumRows, int startRowIndex)
        {
            int lowerBound = startRowIndex + 1;
            int upperBound = startRowIndex + maximumRows;

            if (string.IsNullOrEmpty(sortExpr))
                sortExpr = "Name";

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum ", sortExpr);
            sbQuery.AppendFormat("FROM [{0}] ", DBThemesTableName);

            string commandText = string.Format("SELECT * FROM ({0}) FilteredData WHERE RowNum BETWEEN {1} AND {2} ORDER BY RowNum ASC"
                                                    , sbQuery.ToString(), lowerBound, upperBound);
            var command = new Query(commandText);
            return GetAllObjectsFromDataTable<BookTheme>(command.ExecDataTable());
        }

        public override int GetAllBookThemesCount()
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("SELECT COUNT(*) FROM [{0}] ", DBThemesTableName);

            var query = new Query(sbQuery.ToString());
            return (int)query.ExecScalar();
        }

        #endregion
    }
}