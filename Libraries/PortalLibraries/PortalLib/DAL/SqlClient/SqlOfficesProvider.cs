using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using UlterSystems.PortalLib;

namespace ConfirmIt.PortalLib.DAL.SqlClient
{
	/// <summary>
	/// Provider of offices data for MS SQL Server.
	/// </summary>
	public class SqlOfficesProvider : OfficesProvider
	{
		#region Constants
		private const string DBOfficesTableName = "Offices";
		#endregion

		#region Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Cache<int, OfficeDetails> m_Cache = new Cache<int, OfficeDetails>(delegate { return Globals.Settings.Offices.CacheEnabled; });
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public SqlOfficesProvider()
		{
			Initialize();
		}
		#endregion

		/// <summary>
		/// Initializes offices cache.
		/// </summary>
		private void Initialize()
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = string.Format( "SELECT * FROM {0}", DBOfficesTableName );

				connection.Open();
				using( IDataReader reader = ExecuteReader( command ) )
				{
					List<OfficeDetails> offices = GetAllOfficeDetailsFromReader( reader );
					foreach( OfficeDetails office in offices )
					{
						m_Cache[ office.ID ] = office;
					}
				}
			}
		}

		/// <summary>
		/// Creates new office record in database.
		/// </summary>
		/// <param name="details">Office details.</param>
		/// <returns>ID of new record.</returns>
		public override int CreateOffice( OfficeDetails details )
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
						@"INSERT INTO {0} (OfficeName, StatusesServiceURL, StatusesServiceUserName, StatusesServicePassword, MeteoInformer) VALUES  (@OfficeName, @StatusesServiceURL, @StatusesServiceUserName, @StatusesServicePassword, @MeteoInformer) SELECT @@IDENTITY",
						DBOfficesTableName );
				command.Parameters.Add( "@OfficeName", SqlDbType.NVarChar ).Value =
					details.OfficeName;
				command.Parameters.Add( "@StatusesServiceURL", SqlDbType.NVarChar ).Value =
					details.StatusesServiceURL;
				command.Parameters.Add( "@StatusesServiceUserName", SqlDbType.NVarChar ).Value =
					details.StatusesServiceUserName;
				command.Parameters.Add( "@StatusesServicePassword", SqlDbType.NVarChar ).Value =
					details.StatusesServicePassword;
				command.Parameters.Add( "@MeteoInformer", SqlDbType.NVarChar ).Value =
					details.MeteoInformer;

				try
				{
					id = Convert.ToInt32( ExecuteScalar( command ) );

					transaction.Commit();

					details.ID = id;
					m_Cache[ id ] = details;
				}
				catch
				{ transaction.Rollback(); }
			}
			return id;
		}

		/// <summary>
		/// Updates office information in database.
		/// </summary>
		/// <param name="details">Office details.</param>
		/// <returns>True if record was successfully updated; false, otherwise.</returns>
		public override bool UpdateOffice( OfficeDetails details )
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();
				SqlCommand command = connection.CreateCommand();
				command.Transaction = transaction;
				command.CommandText =
					string.Format(
						"UPDATE {0} SET OfficeName=@OfficeName, StatusesServiceURL=@StatusesServiceURL, StatusesServiceUserName=@StatusesServiceUserName, StatusesServicePassword=@StatusesServicePassword, MeteoInformer=@MeteoInformer WHERE ID=@ID",
						DBOfficesTableName );
				command.Parameters.Add( "@ID", SqlDbType.Int ).Value = details.ID;
				command.Parameters.Add( "@OfficeName", SqlDbType.NVarChar ).Value =
					details.OfficeName;
				command.Parameters.Add( "@StatusesServiceURL", SqlDbType.NVarChar ).Value =
					details.StatusesServiceURL;
				command.Parameters.Add( "@StatusesServiceUserName", SqlDbType.NVarChar ).Value =
					details.StatusesServiceUserName;
				command.Parameters.Add( "@StatusesServicePassword", SqlDbType.NVarChar ).Value =
					details.StatusesServicePassword;
				command.Parameters.Add( "@MeteoInformer", SqlDbType.NVarChar ).Value =
					details.MeteoInformer;

				try
				{
					bool result = ( ExecuteNonQuery( command ) == 1 );

					if( result )
					{
						transaction.Commit();

						m_Cache[ details.ID ] = details;
					}
					else
						transaction.Rollback();

					return result;
				}
				catch
				{
					transaction.Rollback();
					return false;
				}

			}
		}

		/// <summary>
		/// Deletes office from database.
		/// </summary>
		/// <param name="id">ID of office.</param>
		/// <returns>True if record was successfully deleted; false, otherwise.</returns>
		public override bool DeleteOffice( int id )
		{
			using( SqlConnection connection = new SqlConnection( this.ConnectionString ) )
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();
				SqlCommand command = connection.CreateCommand();
				command.Transaction = transaction;
				command.CommandText = string.Format( "DELETE FROM {0} WHERE ID = @ID", DBOfficesTableName );
				command.Parameters.Add( "@ID", SqlDbType.Int ).Value = id;

				try
				{
					bool result = ( ExecuteNonQuery( command ) == 1 );

					if( result )
					{
						m_Cache.Remove( id );

						transaction.Commit();
					}
					else
						transaction.Rollback();

					return result;
				}
				catch
				{
					transaction.Rollback();
					return false;
				}
			}
		}

		/// <summary>
		/// Returns all offices details.
		/// </summary>
		/// <returns>Array of all offices details.</returns>
		public override OfficeDetails[] GetAllOffices()
		{
			if( m_Cache.Count == 0 )
				return new OfficeDetails[ 0 ];

			OfficeDetails[] offices = new OfficeDetails[ m_Cache.Values.Count ];
			m_Cache.Values.CopyTo( offices, 0 );
			return offices;
		}

		/// <summary>
		/// Returns office details by given ID.
		/// </summary>
		/// <param name="id">ID of office.</param>
		/// <returns>Office details with given ID; null, otherwise.</returns>
		public override OfficeDetails GetOfficeByID( int id )
		{
            if (m_Cache.ContainsKey(id))
            { return m_Cache[id]; }
            else
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = string.Format("SELECT * FROM {0} WHERE ID=@ID", DBOfficesTableName);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    using (IDataReader reader = ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            OfficeDetails details = GetOfficeDetailsFromReader(reader);

                            m_Cache[id] = details;

                            return details;
                        }
                        else
                        { return null; }
                    }
                }
            }
		}

		/// <summary>
		/// Returns office details from data reader.
		/// </summary>
		/// <param name="reader">Data reader.</param>
		/// <returns>Office details from data reader.</returns>
		protected virtual OfficeDetails GetOfficeDetailsFromReader( IDataReader reader )
		{
			OfficeDetails details = new OfficeDetails();
			details.ID = (int) reader[ "ID" ];
			details.OfficeName = (string) reader[ "OfficeName" ];

			if( reader[ "StatusesServiceURL" ] == DBNull.Value )
				details.StatusesServiceURL = null;
			else
				details.StatusesServiceURL = (string) reader[ "StatusesServiceURL" ];

			if( reader[ "StatusesServiceUserName" ] == DBNull.Value )
				details.StatusesServiceUserName = null;
			else
				details.StatusesServiceUserName = (string) reader[ "StatusesServiceUserName" ];

			if( reader[ "StatusesServicePassword" ] == DBNull.Value )
				details.StatusesServicePassword = null;
			else
				details.StatusesServicePassword = (string) reader[ "StatusesServicePassword" ];

			if( reader[ "MeteoInformer" ] == DBNull.Value )
				details.MeteoInformer = null;
			else
				details.MeteoInformer = (string) reader[ "MeteoInformer" ];

			return details;
		}

		/// <summary>
		/// Returns all office details from data reader.
		/// </summary>
		/// <param name="reader">Data reader.</param>
		/// <returns>All office details from data reader.</returns>
		protected virtual List<OfficeDetails> GetAllOfficeDetailsFromReader( IDataReader reader )
		{
			List<OfficeDetails> offices = new List<OfficeDetails>();
			while( reader.Read() )
			{
				offices.Add( GetOfficeDetailsFromReader( reader ) );
			}
			return offices;
		}
	}
}
