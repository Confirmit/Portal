using System;
using System.Collections.Generic;
using System.Diagnostics;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.DAL;

using Core;
using Core.ORM.Attributes;

namespace UlterSystems.PortalLib.BusinessObjects
{
	#region Base class Event

	/// <summary>
	/// Класс с описанием события.
	/// </summary>
	[DBTable("Events")]
	public class Event : BasePlainObject
	{
		#region Fileds

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime m_DateTime;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_Description;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_Title;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] 
		private string m_DateFormat;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int m_OwnerID;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool m_IsPublic = true;

		#endregion

		#region Properties

		/// <summary>
		/// Date and time of event.
		/// </summary>
		public string DateEventString
		{
			[DebuggerStepThrough]
			get
			{
				return String.Format("{0} {1}",
									 DateTime.ToShortDateString(),
									 DateTime.ToShortTimeString());
			}
			set { } //need to view in wcf reference class.
		}

		/// <summary>
		/// Is current Event is happand.
		/// </summary>
		public bool IsHappend
		{
			get { return DateTime <= DateTime.Now; }
		}

		/// <summary>
		/// Event rise date.
		/// </summary>
		[DBRead("Date")]
		public DateTime DateTime
		{
			[DebuggerStepThrough]
			get { return m_DateTime; }
			[DebuggerStepThrough]
			set { m_DateTime = value; }
		}

		/// <summary>
		/// Title of Event.
		/// </summary>
		[DBRead("Title")]
		public string Title
		{
			[DebuggerStepThrough]
			get { return m_Title; }
			[DebuggerStepThrough]
			set { m_Title = value; }
		}

		/// <summary>
		/// Description of event.
		/// </summary>
		[DBRead("Description")]
		[DBNullable]
		public string Description
		{
			[DebuggerStepThrough]
			get { return m_Description; }
			[DebuggerStepThrough]
			set { m_Description = value; }
		}

		/// <summary>
		/// Description of event.
		/// </summary>
		[DBRead("DateFormat")]
		[DBNullable]
		public string DateFormat
		{
			[DebuggerStepThrough]
			get { return m_DateFormat; }
			[DebuggerStepThrough]
			set { m_DateFormat = value; }
		}

		[DBRead("OwnerID")]
		public int OwnerID
		{
			[DebuggerStepThrough]
			get { return m_OwnerID; }
			[DebuggerStepThrough]
			set { m_OwnerID = value; }
		}

		[DBRead("IsPublic")]
		public bool IsPublic
		{
			[DebuggerStepThrough]
			get { return m_IsPublic; }
			[DebuggerStepThrough]
			set { m_IsPublic = value; }
		}

		/// <summary>
		/// Get work days to current event.
		/// </summary>
		/// <returns>Number of work days.</returns>
		public int WorkDays
		{
			get
			{
				int count = 0;
				DateTime curTime = DateTime.Now.AddDays(1);
				while (curTime < DateTime)
				{
					CalendarItem item = CalendarItem.GetCalendarItem(curTime);
					if (!item.IsHoliday)
						count++;

					curTime = curTime.AddDays(1);
				}

				return count;
			}
			set { } //need to view in wcf reference class.
		}

		#endregion

		#region Methods

		protected virtual void CopyData(Event eventData)
		{
			ID = eventData.ID;
			Title = eventData.Title;
			Description = eventData.Description;
			DateTime = eventData.DateTime;
			DateFormat = eventData.DateFormat;
			OwnerID = eventData.OwnerID;
			IsPublic = eventData.IsPublic;
		}

		#region overrides

		public override bool Equals(object obj)
		{
			var ue = obj as Event;
			if (ue == null)
				return false;

			return ue.ID == ID;
		}

		public override int GetHashCode()
		{
			return ID.HasValue
			       	? ID.Value
			       	: -1;
		}

		public override void Save()
		{
			if (IsSaved)
				SiteProvider.Events.UpdateEvent(this);
			else
				SiteProvider.Events.CreateEvent(this);
		}

		public override bool Load(int id)
		{
			try
			{
				CopyData(SiteProvider.Events.GetEventByID(id));
				return true;
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#endregion
	}

	#endregion

	#region UserEvent class

	/// <summary>
	/// Класс с описанием события.
	/// </summary>
	[DBTable("Events")]
	public class UserEvent : Event
	{
		#region Constructors

		public UserEvent()
		{}

		public UserEvent(int eventID)
		{
			Load(eventID);
		}

		public UserEvent(Event eventData)
		{
			CopyData(eventData);
		}

		#endregion

		#region Fileds

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IList<int> m_UserOwnersID = null;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IList<Role> m_GroupOwners = null;

		#endregion

	   /* #region References Methods

		/// <summary>
		/// Return all owners of current event.
		/// </summary>
		public IList<int> GetOwnersUsersID()
		{
			if (!ID.HasValue)
				return new List<int>();

			if (m_UserOwnersID == null)
				m_UserOwnersID = SiteProvider.Events.GetUsersOfEvent(ID.Value);

			return m_UserOwnersID;
		}

		/// <summary>
		/// Return all owners of current event.
		/// </summary>
		public IList<Role> GetOwnersGroups()
		{
			if (!ID.HasValue)
				return new List<Role>();

			if (m_GroupOwners == null)
				m_GroupOwners = SiteProvider.Events.GetGroupsOfEvent(ID.Value);

			return m_GroupOwners;
		}

		#endregion*/

		#region References Properties

		/// <summary>
		/// Return all owners of current event.
		/// </summary>
		public IList<int> OwnersUsersID
		{
			get
			{
				if (!ID.HasValue)
					return new List<int>();

				if (m_UserOwnersID == null)
					m_UserOwnersID = SiteProvider.Events.GetUsersOfEvent(ID.Value);

				return m_UserOwnersID;
			}
			set { m_UserOwnersID = value; }
		}

		/// <summary>
		/// Return all owners of current event.
		/// </summary>
		public IList<Role> OwnersGroups
		{
			get
			{
				if (!ID.HasValue)
					return new List<Role>();

				if (m_GroupOwners == null)
					m_GroupOwners = SiteProvider.Events.GetGroupsOfEvent(ID.Value);

				return m_GroupOwners;
			}
			set { m_GroupOwners = value; }
		}

		#endregion
	}

	#endregion
}
