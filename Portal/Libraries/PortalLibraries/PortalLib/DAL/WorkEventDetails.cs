using System;
using System.Diagnostics;
using System.Text;
using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Class of work event data from database.
	/// </summary>
	public class WorkEventDetails : BaseRecord
    {
        #region Constructors

        public WorkEventDetails()
        {}

        public WorkEventDetails(
            int ID, String name,
            DateTime begin, DateTime end,
            int userID, int projectID,
            int workCategoryID, int eventTypeID
            )
            : this(name, begin, end, userID, projectID, workCategoryID, eventTypeID)
        {
            this.ID = ID;
        }

        public WorkEventDetails(
            String name, DateTime begin,
            DateTime end, int userID,
            int projectID,int workCategoryID,
            int eventTypeID
            )
        {
            Name = name ?? string.Empty;
            BeginTime = begin;
            EndTime = end;
            Duration = (int)(EndTime - BeginTime).TotalSeconds;
            UserID = userID;
            ProjectID = projectID;
            WorkCategoryID = workCategoryID;
            UptimeEventTypeID = eventTypeID;
        }

        #endregion

        #region Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_Name;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_BeginTime;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_EndTime;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_Duration;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_UserID;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_ProjectID = 1;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_WorkCategoryID = 1;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_UptimeEventTypeID;
		#endregion

		#region Properties
		/// <summary>
		/// Name of event (not used).
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return m_Name; }
			[DebuggerStepThrough]
			set { m_Name = value; }
		}

		/// <summary>
		/// Time of event start.
		/// </summary>
		public DateTime BeginTime
		{
			[DebuggerStepThrough]
			get { return m_BeginTime; }
			[DebuggerStepThrough]
			set { m_BeginTime = value; }
		}

		/// <summary>
		/// Time of event end.
		/// </summary>
		public DateTime EndTime
		{
			[DebuggerStepThrough]
			get { return m_EndTime; }
			[DebuggerStepThrough]
			set { m_EndTime = value; }
		}

		/// <summary>
		/// Duration of event.
		/// </summary>
		public int Duration
		{
			[DebuggerStepThrough]
			get { return m_Duration; }
			[DebuggerStepThrough]
			set { m_Duration = value; }
		}

		/// <summary>
		/// User ID.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
			[DebuggerStepThrough]
			set { m_UserID = value; }
		}

		/// <summary>
		/// Project ID (not used).
		/// </summary>
		public int ProjectID
		{
			[DebuggerStepThrough]
			get { return m_ProjectID; }
			[DebuggerStepThrough]
			set { m_ProjectID = value; }
		}

		/// <summary>
		/// Work category ID (not used).
		/// </summary>
		public int WorkCategoryID
		{
			[DebuggerStepThrough]
			get { return m_WorkCategoryID; }
			[DebuggerStepThrough]
			set { m_WorkCategoryID = value; }
		}

		/// <summary>
		/// Event type.
		/// </summary>
		public int UptimeEventTypeID
		{
			[DebuggerStepThrough]
			get { return m_UptimeEventTypeID; }
			[DebuggerStepThrough]
			set { m_UptimeEventTypeID = value; }
		}
		#endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( ((WorkEventType) UptimeEventTypeID).ToString() + ": " );
            sb.Append(BeginTime.ToString("hh:mm:ss") + "-");
            sb.Append(EndTime.ToString("hh:mm:ss"));
            return sb.ToString();
        }
	}
}
