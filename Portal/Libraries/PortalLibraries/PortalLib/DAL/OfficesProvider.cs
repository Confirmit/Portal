using System;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Provider for offices system.
	/// </summary>
	public abstract class OfficesProvider : DataAccess
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static OfficesProvider m_Instance;
		#endregion

		#region Properties
		/// <summary>
		/// Instance of offices provider.
		/// </summary>
		public static OfficesProvider Instance
		{
			[DebuggerStepThrough]
			get
			{
				if( m_Instance == null )
					m_Instance = (OfficesProvider) Activator.CreateInstance(
						Type.GetType( Globals.Settings.Offices.ProviderType ) );
				return m_Instance;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public OfficesProvider()
		{
			this.ConnectionString = Globals.Settings.Offices.ConnectionString;
		}
		#endregion

		/// <summary>
		/// Creates new office record in database.
		/// </summary>
		/// <param name="details">Office details.</param>
		/// <returns>ID of new record.</returns>
		public abstract int CreateOffice( OfficeDetails details );

		/// <summary>
		/// Updates office information in database.
		/// </summary>
		/// <param name="details">Office details.</param>
		/// <returns>True if record was successfully updated; false, otherwise.</returns>
		public abstract bool UpdateOffice( OfficeDetails details );

		/// <summary>
		/// Deletes office from database.
		/// </summary>
		/// <param name="id">ID of office.</param>
		/// <returns>True if record was successfully deleted; false, otherwise.</returns>
		public abstract bool DeleteOffice( int id );

		/// <summary>
		/// Returns all offices details.
		/// </summary>
		/// <returns>Array of all offices details.</returns>
		public abstract OfficeDetails[] GetAllOffices();

		/// <summary>
		/// Returns office details by given ID.
		/// </summary>
		/// <param name="id">ID of office.</param>
		/// <returns>Office details with given ID; null, otherwise.</returns>
		public abstract OfficeDetails GetOfficeByID( int id );
	}
}
