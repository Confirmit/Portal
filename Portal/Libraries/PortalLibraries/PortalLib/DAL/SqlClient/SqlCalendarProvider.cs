using System;
using System.Data;
using System.Data.SqlClient;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
	/// <summary>
	/// Provider of calendar data for MS SQL Server.
	/// </summary>
	public class SqlCalendarProvider : CalendarProvider
	{
		#region Constants

		/// <summary>
		/// Name of calendar table in database.
		/// </summary>
		private const string DBTableName = "Calendar";

		#endregion

		#region Methods
		/// <summary>
		/// Returns calendar details for given date or null if they are not found.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <returns>Calendar details for given date or null if they are not found.</returns>
		public override CalendarDetails GetCalendarDetails( DateTime date )
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText =
					string.Format( "SELECT * FROM {0} WHERE Date = @Date", DBTableName );
				command.Parameters.Add( "@Date", SqlDbType.DateTime ).Value = date.Date;
				connection.Open();
				IDataReader reader = ExecuteReader( command );
				if( reader == null )
					return null;
				if( !reader.Read() )
					return null;
				CalendarDetails details = GetDetailsFromReader( reader );

				// Only one record must be in database.
				while( reader.Read() )
					DeleteDetails( (int) reader[ "ID" ] );

				return details;
			}
		}

		/// <summary>
		/// Inserts details in database.
		/// </summary>
		/// <param name="details">Calendar details.</param>
		/// <returns>ID of inserted record.</returns>
		public override int InsertDetails( CalendarDetails details )
		{
			int id = -1;
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();
				SqlCommand command = connection.CreateCommand();
				command.Transaction = transaction;
				command.CommandText =
					string.Format(
						"INSERT INTO {0} (Date, WorkTime, Comment) VALUES  (@Date, @WorkTime, @Comment) SELECT @@IDENTITY",
						DBTableName );
				command.Parameters.Add( "@Date", SqlDbType.DateTime ).Value =
					details.Date.Date;
				command.Parameters.Add( "@WorkTime", SqlDbType.DateTime ).Value =
					details.WorkTime;
				command.Parameters.Add( "@Comment", SqlDbType.NVarChar ).Value =
					details.Comment;

				try
				{
					id = Convert.ToInt32( ExecuteScalar( command ) );
					transaction.Commit();
				}
				catch
				{ transaction.Rollback();}
			}
			return id;
		}

		/// <summary>
		/// Updates details in database.
		/// </summary>
		/// <param name="details">Calendar details.</param>
		public override bool UpdateDetails( CalendarDetails details )
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = string.Format( "UPDATE {0} SET Date=@Date, WorkTime=@WorkTime, Comment=@Comment WHERE ID=@ID", DBTableName );
				command.Parameters.Add( "@ID", SqlDbType.Int ).Value = details.ID;
				command.Parameters.Add( "@Date", SqlDbType.DateTime ).Value = details.Date.Date;
				command.Parameters.Add( "@WorkTime", SqlDbType.DateTime ).Value = details.WorkTime;
				command.Parameters.Add( "@Comment", SqlDbType.NVarChar ).Value = details.Comment;
				connection.Open();
				ExecuteNonQuery( command );
				return true;
			}
		}

		/// <summary>
		/// Deletes details from database.
		/// </summary>
		/// <param name="id">Record ID.</param>
		public override bool DeleteDetails( int id )
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = string.Format( "DELETE FROM {0} WHERE ID = @ID", DBTableName );
				command.Parameters.Add( "@ID", SqlDbType.Int ).Value = id;
				connection.Open();
				ExecuteNonQuery( command );
				return true;
			}
		}

		/// <summary>
		/// Returns calendar details from data reader.
		/// </summary>
		/// <param name="reader">Data reader.</param>
		/// <returns>Calendar details from data reader.</returns>
		protected virtual CalendarDetails GetDetailsFromReader( IDataReader reader )
		{
			CalendarDetails details = new CalendarDetails();

			details.ID = (int) reader[ "ID" ];
			details.Date = (DateTime) reader[ "Date" ];
			details.WorkTime = (DateTime) reader[ "WorkTime" ];
			details.Comment = (string) reader[ "Comment" ];

			return details;
		}
		#endregion
	}
}
