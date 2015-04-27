using System;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Provider for work events system.
	/// </summary>
	public abstract class WorkEventsProvider : DataAccess
	{
		#region Fields
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private static WorkEventsProvider m_Instance;
		#endregion

		#region Properties
		/// <summary>
		/// Instance of work events provider.
		/// </summary>
		public static WorkEventsProvider Instance
		{
			[DebuggerStepThrough]
			get
			{
				if( m_Instance == null )
					m_Instance = (WorkEventsProvider) Activator.CreateInstance(
						Type.GetType( Globals.Settings.WorkTime.ProviderType ) );
				return m_Instance;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public WorkEventsProvider()
		{
			this.ConnectionString = Globals.Settings.WorkTime.ConnectionString;
		}
		#endregion

		/// <summary>
		/// Creates new work event in database.
		/// </summary>
		/// <param name="eventDetails">Work event details.</param>
		/// <returns>ID of new database record.</returns>
		public abstract int CreateEvent( WorkEventDetails eventDetails );

		/// <summary>
		/// Updates work event information in database.
		/// </summary>
		/// <param name="eventDetails">Work event details.</param>
		/// <returns>True if information was updated; false, otherwise.</returns>
		public abstract bool UpdateEvent( WorkEventDetails eventDetails );

		/// <summary>
		/// Deletes work event from database.
		/// </summary>
		/// <param name="id">ID of work event.</param>
		/// <returns>True if work event was deleted; false, otherwise.</returns>
		public abstract bool DeleteEvent( int id );

		/// <summary>
		/// Deletes all events of user.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <returns>True if events were deleted; false, otherwise.</returns>
		public abstract bool DeleteUserEvents( int userId );

		/// <summary>
		/// Returns all work events for given date.
		/// <param name="userID">User ID.</param>
		/// <param name="date">Date.</param>
		/// </summary>
		/// <returns>Array of all work events for given date.</returns>
		public abstract WorkEventDetails[] GetEventsOfDate( int userID, DateTime date );

		/// <summary>
		/// Returns work event with given ID.
		/// </summary>
		/// <param name="id">ID of work event.</param>
		/// <returns>Work event with given ID. Null, otherwise.</returns>
		public abstract WorkEventDetails GetEventByID( int id );
	}
}
