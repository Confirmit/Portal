using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Work calendar data provider.
	/// </summary>
	public abstract class CalendarProvider : DataAccess
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static CalendarProvider m_Instance;
		#endregion

		#region Properties
		/// <summary>
		/// Instance of calendar provider.
		/// </summary>
		public static CalendarProvider Instance
		{
			get
			{
                if (m_Instance == null)
                {
                    Type providerType = Type.GetType(Globals.Settings.Calendar.ProviderType);
                    if (providerType == null)
                        throw new InvalidOperationException();
                    m_Instance = (CalendarProvider)Activator.CreateInstance(providerType);
                }
				return m_Instance;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public CalendarProvider()
		{
			this.ConnectionString = Globals.Settings.Calendar.ConnectionString;
		}
		#endregion

		/// <summary>
		/// Returns calendar details for given date or null if they are not found.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <returns>Calendar details for given date or null if they are not found.</returns>
		public abstract CalendarDetails GetCalendarDetails( DateTime date );

		/// <summary>
		/// Inserts details in database.
		/// </summary>
		/// <param name="details">Calendar details.</param>
		/// <returns>ID of inserted record.</returns>
		public abstract int InsertDetails( CalendarDetails details );

		/// <summary>
		/// Updates details in database.
		/// </summary>
		/// <param name="details">Calendar details.</param>
		public abstract bool UpdateDetails( CalendarDetails details );

		/// <summary>
		/// Deletes details from database.
		/// </summary>
		/// <param name="id">Record ID.</param>
		public abstract bool DeleteDetails( int id );
	}
}
