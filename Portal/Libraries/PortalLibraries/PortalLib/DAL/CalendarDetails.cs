using System;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Class of calendar data from database.
	/// </summary>
	public class CalendarDetails : BaseRecord
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime m_Date;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_WorkTime;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_Comment;
		#endregion

		#region Properties
		/// <summary>
		/// Date.
		/// </summary>
		public DateTime Date
		{
			[DebuggerStepThrough]
			get { return m_Date; }
			[DebuggerStepThrough]
			set { m_Date = value.Date; }
		}

		/// <summary>
		/// Work time for date of object (for database support).
		/// </summary>
		public DateTime WorkTime
		{
			[DebuggerStepThrough]
			get { return m_WorkTime; }
			[DebuggerStepThrough]
			set { m_WorkTime = value; }
		}

		/// <summary>
		/// Comment for date.
		/// </summary>
		public string Comment
		{
			[DebuggerStepThrough]
			get { return m_Comment; }
			[DebuggerStepThrough]
			set { m_Comment = value; }
		}

		#endregion
	}
}
