using System;
using System.Data;
using System.Globalization;

using Core;
using Core.DB;
using Core.DB.QueryStatement;
using Core.ORM;

using UlterSystems.PortalLib.NewsTape;

namespace UlterSystems.PortalLib.DB
{
    ///<summary>
    /// ����� ��� �������������� � ����� ������.
    ///</summary>
    public static class DBManager
    {
        #region ������ ��� ������ � ���������
        /// <summary>
        /// ���������� ������� � ��������� ������������ �� �������� �������� �������.
        /// </summary>
        /// <param name="userID">ID ������������.</param>
        /// <param name="begin">������ ��������� �������.</param>
        /// <param name="end">����� ��������� �������.</param>
        /// <param name="rowsCount">���������� �����.</param>
        /// <returns>������� � ��������� ������������ �� �������� �������� �������.</returns>
        public static DataTable GetUserEvents(
            int userID,
            DateTime begin,
            DateTime end,
            out int rowsCount)
        {
            rowsCount = 0;

            Procedure proc = new Procedure("[GetUserEvents]");
            proc.Add("@UserID", userID, DbType.Int32);
            proc.Add("@IntervalBegin", begin, DbType.DateTime);
            proc.Add("@IntervalEnd", end, DbType.DateTime);
            DataTable dt = proc.ExecDataTable();

            if (dt != null)
                rowsCount = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// ���������� ������ � ������� ������ ������ �� ��������� ����.
        /// </summary>
        /// <param name="userID">ID ������������.</param>
        /// <param name="date">���� ������� �������.</param>
        /// <returns>������ � ������� ������ ������ �� ��������� ����.</returns>
        public static DataRow GetWorkEvent(int userID, DateTime date)
        {
            var proc = new Procedure("GetWorkEvent");
            proc.Add("@UserID", userID, DbType.Int32);
            proc.Add("@Date", date, DbType.DateTime);
            
            return proc.ExecDataRow();
        }

        /// <summary>
        /// ���������� ������ � ����������� ������� ������ ������.
        /// </summary>
        /// <param name="userID">ID ������������.</param>
        /// <returns>������ � ����������� ������� ������ ������.</returns>
        public static DataRow GetTodayWorkEvent(int userID)
        {
            return GetWorkEvent(userID, DateTime.Today);
        }

        /// <summary>
        /// ���������� ����� ���������� �������������.
        /// </summary>
        /// <returns>����� ���������� �������������.</returns>
        public static int GetNumberOfActiveUsers()
        {
            Procedure proc = new Procedure("GetCountUserNow");
            DataRow row = proc.ExecDataRow();
            if (row == null)
            { return 0; }
            else
            { return (int)row["count_users"]; }
        }
        #endregion

        #region ������ ��� ������ � ������
        /// <summary>
        /// Returns record about calendar date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Record about calendar date.</returns>
        public static DataRow GetCalendarDate(DateTime date)
        {
            Procedure proc = new Procedure("GetCalendarDate");
            proc.Add("@Date", date, DbType.DateTime);
            DataRow row = proc.ExecDataRow();
            if (row == null)
                return null;
            else
                return row;
        }
        #endregion

        #region ������ ��� ������ � ��������������
        /// <summary>
        /// ���������� ������ ������������� � �������� ������� ��������.
        /// </summary>
        /// <returns>������ ������������� � �������� ������� ��������.</returns>
        public static DataTable GetUserListWithOpenWorkPeriod()
        {
            Procedure proc = new Procedure("[GetUserWithOpenWorkPeriod]");
            proc.Add("@CurrentDate", DateTime.Now, DbType.DateTime);
            DataTable dt = proc.ExecDataTable();

            if ((dt == null) || (dt.Rows.Count == 0))
                return null;
            else
                return dt;
        }
        #endregion

        #region ������ ��� ������ �� �������� �����������
        /// <summary>
        /// ���������� ������� �� ������� ������� �������� ��� ����������� ��������� ����.
        /// </summary>
        /// <param name="type">��� �����������.</param>
        /// <returns>������� �� ������� ������� �������� ��� ����������� ��������� ����.</returns>
        public static DataTable GetNotificationList(int type)
        {
            Procedure proc = new Procedure("[GetNotificationList]");
            proc.Add("@Type", type, DbType.Int32);
            DataTable dt = proc.ExecDataTable();

            if ((dt == null) || (dt.Rows.Count == 0))
                return null;
            else
                return dt;
        }
        #endregion

        #region ������ �������������� ��������-�������������
        /// <summary>
        /// ���������� ���������� ��������-������������� � �������� ������ � �������.
        /// </summary>
        /// <param name="userName">��� ������������.</param>
        /// <param name="password">������.</param>
        /// <returns>���������� ��������-������������� � �������� ������ � �������.</returns>
        public static int GetInternetUsersCount(
            string userName,
            string password
            )
        {
            try
            {
                Procedure proc = new Procedure("AuthenticateUser");
                if (userName.Length > 50)
                    userName = userName.Substring(0, 50);
                proc.Add("@UserName", userName, DbType.String);
                if (password.Length > 50)
                    password = password.Substring(0, 50);
                proc.Add("@Password", password, DbType.String);

                return (int)proc.ExecScalar();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("������ ��� ��������� �� ���� ������ ���������� ��� �������������� ��������-������������.", ex);
                return 0;
            }
        }

        /// <summary>
        /// ���������� ID ��������-������������.
        /// </summary>
        /// <param name="userName">��� ��������� �������������.</param>
        /// <returns>ID ��������-������������. Null ��� �����-���� �������.</returns>
        public static int? GetInternetUserID(string userName)
        {
            try
            {
                Procedure proc = new Procedure("GetInternetUserID");
                if (userName.Length > 50)
                    userName = userName.Substring(0, 50);
                proc.Add("@UserName", userName, DbType.String);

                return (int)proc.ExecScalar();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("������ ��� ��������� ID ��������-������������.", ex);
                return null;
            }
        }
        #endregion

        #region ������ ��� ������ � ���������

        /// <summary>
        /// ���������� ������� ���������� ��������.
        /// </summary>
        public static DataSet GetActualNews(string[] offices)
        {
            var queryStatement = ObjectMapper.GetSelectQueryStatement(typeof(News));
            queryStatement.Clauses.Add(new QueryStatementClause(string.Empty, ">")
                                           {
                                               FieldName = "ExpireTime",
                                               Value = "@expTime"
                                           });

            var query = new Query(queryStatement.ToString());
            query.Add("@expTime", DateTime.Now, DbType.Time);

            query.Command.CommandText += " AND (OfficeID=" + String.Join(" OR OfficeID=", offices) + ")";
            query.Command.CommandText += " ORDER BY CreateTime DESC";
            return query.ExecDataSet();
        }

        /// <summary>
        /// ���������� ������� ������ �������� �� ������.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetArchiveNews(PagingArgs args, out int total_count, String[] offices)
        {
            String query = "SELECT n.ID,Caption,Text,AuthorID,CreateTime,ExpireTime,OfficeID, PostID,u.LastName FROM News n,Users u";
            DateTime dt = DateTime.Now.Date;
            query += " WHERE n.AuthorID=u.ID AND ExpireTime<=CONVERT(datetime, '" + dt.ToString(CultureInfo.InvariantCulture.DateTimeFormat) + "', 101)";
            
                query += " AND (OfficeID=" + String.Join(" OR OfficeID=", offices) + ")";

            Procedure procedure = new Procedure("uiGetObjectsPage");
            procedure.Add("@PageIndex", args.PageIndex);
            procedure.Add("@PageSize", args.PageSize);
            procedure.Add("@OrderField", args.SortExpression);
            procedure.Add("@IsOrderASC", args.SortOrderASC);
            procedure.Add("@Query", query);
            procedure.AddReturnValueParameter();
            DataSet ds = procedure.ExecDataSet();
            total_count = Convert.ToInt32(procedure.GetReturnValue());

            return ds;
        }

        /// <summary>
        /// ���������� ������� "�����������" �����.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllowTags()
        {
            String query = "SELECT tagName from AllowTags";
            Query q = new Query(query);
            DataTable dt = q.ExecDataTable();
            return dt;
        }

        /// <summary>
        /// ������ �������.
        /// </summary>
        public enum NewsStatus
        {
            AllNews = 0,
            ActualNews = 1,
            ArchiveNews = 2
        }

        /// <summary>
        /// ��������� ������ ��� ������ ��������.
        /// </summary>
        public enum SearchPeriod
        {
            All = 0,
            Day = 1,
            ThreeDays = 2,
            Week = 3,
            Month = 4,
            HalfYear = 5,
            Year = 6
        }

		  ///// <summary>
		  ///// ���������� ������� ���������� ���� ��������.
		  ///// </summary>
		  ///// <returns>������� ���������� ���� ��������.</returns>
		  //public static DataSet GetNewsEditors()
		  //{
		  //    String query = "select distinct u.* from ";
		  //    query += ObjectMapper.GetDBTableName(typeof(Person)) + " u, ";
		  //    query += ObjectMapper.GetDBTableName(typeof(GroupMembership)) + " pg, ";
		  //    query += ObjectMapper.GetDBTableName(typeof(Group)) + " g ";
		  //    query += " where u.ID = pg.PersonID";
		  //    query += " and pg.GroupID = g.ID";
		  //    query += " and (g.GroupID = '" + Group.GroupsEnum.OfficeNewsEditor + "' or g.GroupID='" + Group.GroupsEnum.GeneralNewsEditor + "')";
		  //    Query q = new Query(query);
		  //    DataSet ds = q.ExecDataSet();
		  //    return ds;

		  //}
        /// <summary>
        /// ���������� ������� � ������������ ������ ��������.
        /// </summary>
        /// <param name="SearchTerms">������� ����� � ������ � ����������.</param>
        /// <param name="SearchAuthor">������� �����.</param>
        public static DataSet SearchNews(PagingArgs args,
                                            out int total_count,
                                            string searchTerms,
                                            int searchAuthorID,
                                            NewsStatus newsStatus,
                                            int officeID,
                                            string[] offices,
                                            SearchPeriod period
                                           )
        {
            if (searchTerms == null)
                searchTerms = String.Empty;

            // Period

            DateTime searchDateTime = new DateTime(1754, 1, 1);
            switch (period)
            {
                case SearchPeriod.Day:
                    searchDateTime = DateTime.Now.AddDays(-1);
                    break;
                case SearchPeriod.ThreeDays:
                    searchDateTime = DateTime.Now.AddDays(-3);
                    break;
                case SearchPeriod.Week:
                    searchDateTime = DateTime.Now.AddDays(-7);
                    break;
                case SearchPeriod.Month:
                    searchDateTime = DateTime.Now.AddDays(-30);
                    break;
                case SearchPeriod.HalfYear:
                    searchDateTime = DateTime.Now.AddMonths(-6);
                    break;
                case SearchPeriod.Year:
                    searchDateTime = DateTime.Now.AddYears(-1);
                    break;
            }

            // Author

            String[] aTerms;
            // aTerms = strSearchAuthor.Split(new char[] { ' ' });
            String searchQuery = "SELECT t1.ID,Caption,Text,AuthorID,CreateTime,ExpireTime,OfficeID, PostID, t2.LastName FROM [Portal].[dbo].[News] t1 inner join (select ID,LastName from [Portal].[dbo].[Users]";
            if (searchAuthorID != 0)
            {
                searchQuery += " where (";
                /*searchQuery += "    FirstName LIKE '%" + String.Join("%' OR FirstName LIKE '%", aTerms) + "%'";
                searchQuery += " OR MiddleName LIKE '%" + String.Join("%' OR MiddleName LIKE '%", aTerms) + "%'";
                searchQuery += " OR LastName LIKE '%" + String.Join("%' OR LastName LIKE '%", aTerms) + "%'";*/
                searchQuery += "ID = " + searchAuthorID;
                searchQuery += " )";
            }
            searchQuery += ") as t2 on t1.AuthorID = t2.ID ";
            searchQuery += " WHERE (	CreateTime >= CONVERT(datetime, '" + searchDateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat) + "', 101)";

            // Terms

            aTerms = searchTerms.Split(new char[] { ' ' });
            searchQuery += " AND (( Caption LIKE '%" + String.Join("%' AND Caption LIKE '%", aTerms) + "%' )";
            searchQuery += " OR ( Text LIKE '%" + String.Join("%' AND Text LIKE '%", aTerms) + "%' ))";

            switch (newsStatus)
            {
                case NewsStatus.ActualNews:
                    searchQuery += " AND (ExpireTime > GetDate())";
                    break;
                case NewsStatus.ArchiveNews:
                    searchQuery += " AND (ExpireTime <= GetDate())";
                    break;
            }

            if (officeID == -1)
            {
                searchQuery += " AND (OfficeID=" + String.Join(" OR OfficeID=", offices) + ")";
            }
            else
                searchQuery += " AND OfficeID=" + officeID;

            searchQuery += ")";
            Procedure procedure = new Procedure("uiGetObjectsPage");
            procedure.Add("@PageIndex", args.PageIndex);
            procedure.Add("@PageSize", args.PageSize);
            procedure.Add("@OrderField", args.SortExpression);
            procedure.Add("@IsOrderASC", args.SortOrderASC);
            procedure.Add("@Query", searchQuery);
            procedure.AddReturnValueParameter();
            DataSet ds = procedure.ExecDataSet();
            total_count = Convert.ToInt32(procedure.GetReturnValue());
            return ds;
        }

        /// <summary>
        /// ���������� ������� � ������������ ��� ������ �������.
        /// </summary>
        /// <param name="NewsID">ID �������.</param>
        /// <returns></returns>
        public static DataSet GetNewsAttachments(int newsID)
        {
            Procedure procedure = new Procedure("GetNewsAttachments");
            procedure.Add("@NewsID", newsID);
            DataSet ds = procedure.ExecDataSet();
            return ds;
        }
        #endregion

        /// <summary>
        /// ���������� ����� ��������������� ������.
        /// </summary>
        public static DataSet GetUnnecessaryAttachments()
        {
            // ������� ����� ������ ��������������� ������
            Procedure procedure = new Procedure("GetUnnecessaryAttachments");
            DataSet ds = procedure.ExecDataSet();
            return ds;
        }

        /// <summary>
        /// ������� ��������������� ����� �� ��.
        /// </summary>
        public static void CleanAttachments()
        {
            // ������� �� �� ��������������� ����������
            Procedure procedure = new Procedure("CleanAttachments");
            procedure.ExecNonQuery();
            return;
        }
    }
}
