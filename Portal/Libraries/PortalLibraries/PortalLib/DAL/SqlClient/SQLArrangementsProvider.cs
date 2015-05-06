using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Core;
using Core.DB;
using Core.ORM;

using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.Arrangements;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
    public class SQLArrangementProvider
    {
        #region Fields

        private static string ConnectionString = Globals.Settings.Events.ConnectionString;
        private const string DBConferenceHallTableName = "ConferenceHall";
        private const string DBArrangementTableName = "Arrangement";
        private const string DBArrangementDateTableName = "ArrangementDate";

        #endregion

        /// <summary>
        /// Return all conference halls of current office.
        /// </summary>
        /// <param name="userID">Current office id.</param>
        public static IList<ConferenceHall> GetConferenceHallsList(int OfficeID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat("SELECT ch.* FROM ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ConferenceHall)) + " ch");
                query.AppendFormat(" WHERE ch.OfficeID=@pOfficeID");
                query.Append(" ORDER BY ch.Name");

                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@pOfficeID", OfficeID);
                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    return GetAllConferenceHallsDataFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Возвращает IList<ConferenceHall> комнат с мероприятиями на заданную дату.
        /// </summary>
        public static IList<ConferenceHall> GetDayConferenceHallsList(int OfficeID, DateTime Date)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DateTime searchDateBegin = Date;
                DateTime searchDateEnd = Date.AddDays(1);

                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat("SELECT distinct ch.* FROM ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(Arrangement)) + " a, ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ArrangementDate)) + " b");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ConferenceHall)) + " ch");
                
                //query.AppendFormat(" WHERE a.TimeBegin>=@pDateBegin and a.TimeBegin<@pDateEnd");

                //compare time only from Arrangement table
                query.AppendFormat(" WHERE CONVERT(char(12), a.TimeBegin, 114) >= CONVERT(char(12), @pDateBegin, 114)" +
                    "and CONVERT(char(12), a.TimeBegin, 114) < CONVERT(char(12), @pDateEnd, 114)");
                query.Append(" AND (b.ArrangementID=a.ID) AND (ch.ID=a.ConferenceHallID)");
                //compare date only from ArrangementDate table
                query.Append(" AND CONVERT(char(8), b.Date, 112)=CONVERT(char(8), @pDateBegin, 112)");

                query.Append(" AND (ch.OfficeID=" + OfficeID + ")");
                query.Append(" ORDER BY ch.Name");

                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@pDateBegin", searchDateBegin);
                command.Parameters.AddWithValue("@pDateEnd", searchDateEnd);
                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    return GetAllConferenceHallsDataFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Save new conference hall.
        /// </summary>
        /// <param name="ch">Conference hall for checking.</param>
        public static ConferenceHall TryToFindThisConferenceHall(ConferenceHall ch)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat("SELECT ch.* FROM ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ConferenceHall)) + " ch");
                query.Append(" ORDER BY ch.Name");

                command.CommandText = query.ToString();
                connection.Open();
                
                using (IDataReader reader = command.ExecuteReader())
                {
                    foreach (ConferenceHall confHall in GetAllConferenceHallsDataFromReader(reader))
                        if (confHall.Name.Contains(ch.Name) && confHall.OfficeID.Equals(ch.OfficeID)) //FIXME: бд вываливает всю строку длиной 255 символов
                            return confHall;
                    return null;
                }
            }
        }

        /// <summary>
        /// Возвращает IList мероприятий на заданную дату в данной комнате.
        /// </summary>
        public static IList<Arrangement> GetDayArrangementsList(int ConferenceHallID, DateTime Date)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DateTime searchDateBegin = Date;
                DateTime searchDateEnd = Date.AddHours(23).AddMinutes(59);

                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat("SELECT a.* FROM ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(Arrangement)) + " a,");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ArrangementDate)) + " b");

                //compare time only from Arrangement table
                query.AppendFormat(" WHERE CONVERT(char(12), a.TimeBegin, 114) >= CONVERT(char(12), @pDateBegin, 114)" +
                    "and CONVERT(char(12), a.TimeBegin, 114) < CONVERT(char(12), @pDateEnd, 114)");
                query.Append(" AND (a.ConferenceHallID=" + ConferenceHallID + ")");
                query.Append(" AND (b.ArrangementID=a.ID)");
                //compare date only from ArrangementDate table
                query.Append(" AND CONVERT(char(8), b.Date, 112)=CONVERT(char(8), @pDateBegin, 112)");
                query.Append(" ORDER BY a.Name");

                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@pDateBegin", searchDateBegin);
                command.Parameters.AddWithValue("@pDateEnd", searchDateEnd);
                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    return GetAllArrangementsDataFromReader(reader);
                }
            }
        }
        /// <summary>
        /// Проверяет возможность добавления мероприятия с данным интервалом.
        /// </summary>
        /// <param name="ConferenceHallID">ID комнаты.</param>
        /// <param name="ArrangementID">ID мероприятия (при редактировании).</param>
        /// <param name="dBegin">Дата начала.</param>
        /// <param name="dEnd">Дата конца.</param>
        /// <returns>Возможно ли добавление.</returns>
        public static bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, DateTime dBegin, DateTime dEnd)
        {
            String sql = "SELECT a.* FROM ";
            sql += DBAttributesManager.GetDBTableName(typeof(ConfirmIt.PortalLib.Arrangements.Arrangement)) + " a, ";
            sql += DBAttributesManager.GetDBTableName(typeof(ArrangementDate)) + " b";

            //compare time only from Arrangement table
            sql += " WHERE (CONVERT(char(12), a.TimeBegin, 114) <= CONVERT(char(12), @pDateBegin, 114)" +
                "and CONVERT(char(12), a.TimeEnd, 114) > CONVERT(char(12), @pDateEnd, 114)";
            sql += " AND (b.ArrangementID=a.ID)";
            //compare date only from ArrangementDate table
            sql += " AND CONVERT(char(8), b.Date, 112)=CONVERT(char(8), @pDateBegin, 112))";

            //compare time only from Arrangement table
            sql += " OR (CONVERT(char(12), a.TimeBegin, 114) >= CONVERT(char(12), @pDateBegin, 114)" +
                "and CONVERT(char(12), a.TimeEnd, 114) <= CONVERT(char(12), @pDateEnd, 114)";
            sql += " AND (b.ArrangementID=a.ID)";
            //compare date only from ArrangementDate table
            sql += " AND CONVERT(char(8), b.Date, 112)=CONVERT(char(8), @pDateBegin, 112))";

            //compare time only from Arrangement table
            sql += " OR (CONVERT(char(12), a.TimeBegin, 114) < CONVERT(char(12), @pDateEnd, 114)" +
                "and CONVERT(char(12), a.TimeEnd, 114) >= CONVERT(char(12), @pDateEnd, 114)";
            sql += " AND (b.ArrangementID=a.ID)";
            //compare date only from ArrangementDate table
            sql += " AND CONVERT(char(8), b.Date, 112)=CONVERT(char(8), @pDateBegin, 112))";

            sql += " AND (a.ID != " + ArrangementID + ")";

            //sql += " WHERE ((a.TimeBegin <= @pDateBegin and a.TimeEnd > @pDateBegin)";
            //sql += " or (a.TimeBegin >= @pDateBegin and a.TimeEnd <= @pDateEnd)";
            //sql += " or (a.TimeBegin < @pDateEnd and a.TimeEnd >= @pDateEnd))";

            Query q = new Query(sql);
            q.Add("@pDateBegin", dBegin, DbType.Time);
            q.Add("@pDateEnd", dEnd, DbType.Time);
            q.Command.CommandText += " AND (a.ConferenceHallID=" + ConferenceHallID + ")";
            if (ArrangementID != 0)
                q.Command.CommandText += " AND (a.ID<>" + ArrangementID + ")";
            DataTable dt = q.ExecDataTable();
            if (dt.Rows.Count == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Is this arrangement cyclic
        /// </summary>
        public static bool isCyclicArrangement(int ArrID)
        {
            String sql = "SELECT * FROM ";
            sql += DBAttributesManager.GetDBTableName(typeof(ArrangementDate));
            sql += " WHERE ArrangementID=" + ArrID;

            Query q = new Query(sql);
            DataTable dt = q.ExecDataTable();

            if (dt.Rows.Count > 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Delete the string in ArrangementDate
        /// </summary>
        public static void DeleteArrangementDate(ArrangementDate arrDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                StringBuilder query = new StringBuilder();

                query.AppendFormat("DELETE FROM ");
                query.AppendFormat(DBAttributesManager.GetDBTableName(typeof(ArrangementDate)));
                query.AppendFormat(" WHERE ArrangementID=" + arrDate.ArrangementID);
                query.AppendFormat(" AND Date=cast('" + arrDate.Date.ToString("MM.dd.yyyy HH:mm:ss") + "' as datetime)");

                connection.Open();
                command.CommandText = query.ToString();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all arrangements from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>All arrangements from data reader.</returns>
        private static List<Arrangement> GetAllArrangementsDataFromReader(IDataReader reader)
        {
            List<Arrangement> aData = new List<Arrangement>();
            while (reader.Read())
            {
                aData.Add(GetArrangementDataFromReader(reader));
            }
            return aData;
        }

        /// <summary>
        /// Returns arrangement from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Arrangement from data reader.</returns>
        private static Arrangement GetArrangementDataFromReader(IDataReader reader)
        {
            Arrangement details = new Arrangement();
            details.ID = (int)reader["ID"];
            details.Name = (string)reader["Name"];
            details.Description = reader["Description"] == DBNull.Value
                                      ? null
                                      : (string)reader["Description"];
            details.ConferenceHallID = (int)reader["ConferenceHallID"];
            details.TimeBegin = (DateTime)reader["TimeBegin"];
            details.TimeEnd = (DateTime)reader["TimeEnd"];
            details.ListOfGuests = reader["ListOfGuests"] == DBNull.Value
                                      ? null
                                      : (string)reader["ListOfGuests"];
            details.Equipment = reader["Equipment"] == DBNull.Value
                                      ? null
                                      : (string)reader["Equipment"];
            return details;
        }

        /// <summary>
        /// Returns all conference halls from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>All conference halls from data reader.</returns>
        private static List<ConferenceHall> GetAllConferenceHallsDataFromReader(IDataReader reader)
        {
            List<ConferenceHall> chData = new List<ConferenceHall>();
            while (reader.Read())
            {
                chData.Add(GetConferenceHallDataFromReader(reader));
            }
            return chData;
        }

        /// <summary>
        /// Returns conference hall from data reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Conference hall from data reader.</returns>
        private static ConferenceHall GetConferenceHallDataFromReader(IDataReader reader)
        {
            ConferenceHall details = new ConferenceHall();
            details.ID = (int)reader["ID"];
            details.Name = (string)reader["Name"];
            details.Description = reader["Description"] == DBNull.Value
                                      ? null
                                      : (string)reader["Description"];
            details.OfficeID = (int)reader["OfficeID"];
            return details;
        }
    }
}
