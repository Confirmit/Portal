using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ConfirmIt.PortalLib.BAL;
using Core;
using UlterSystems.PortalLib.DB;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// ����� ������� �������������.
	/// </summary>
	public class UserList
	{
        public static Person[] GetUserList(string propertyName, bool sortOrderAsc)
        {
            BaseObjectCollection<Person> coll = (BaseObjectCollection<Person>)BasePlainObject.GetObjects(typeof(Person), propertyName, sortOrderAsc);
            if (coll == null)
                return null;
            else
                return coll.ToArray();
        }

		#region ������

		/// <summary>
		/// ���������� ������ �������������.
		/// </summary>
		/// <returns>������ �������������.</returns>
		public static Person[] GetUserList()
		{
			BaseObjectCollection<Person> coll = (BaseObjectCollection<Person>) BasePlainObject.GetObjects( typeof( Person ) );
			if( coll == null )
				return null;
			else
				return coll.ToArray();
		}

        /// <summary>
        /// Return users list without moscow employeers.
        /// </summary>
        /// <returns>List of persons.</returns>
        public static IList<Person> GetYaroslavlOfficeUsersList()
        {
            IList<Person> list = new List<Person>();
            foreach (Person person in GetEmployeeList())
            {
                if (person.EmployeesUlterSYSMoscow)
                    continue;

                list.Add(person);
            }

            return list;
        }

		/// <summary>
		/// ���������� ������ ���������� ��������.
		/// </summary>
		/// <returns>������ ���������� ��������.</returns>
        public static Person[] GetEmployeeList()
		{
		    try
		    {
		        List<Person> employees = new List<Person>();
		        foreach (Person person in GetUserList())
		        {
		            if (person.IsInRole("Employee"))
		                employees.Add(person);
		        }
		        return employees.ToArray();
		    }
		    catch (Exception ex)
		    {
		        Logger.Log.Error(ex.Message, ex);
		        return new Person[0];
		    }
		}

	    /// <summary>
		/// ���������� ������ ���������������.
		/// </summary>
		/// <returns>������ ���������������.</returns>
		public static Person[] GetAdminList()
		{
			try
			{
				List<Person> admins = new List<Person>();
				foreach( Person person in GetUserList() )
				{
					if( person.IsInRole( "Administrator" ) )
						admins.Add( person );
				}
				return admins.ToArray();
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return new Person[ 0 ];
			}
		}

		/// <summary>
		/// ���������� ������ ���������� ������� ��������.
		/// </summary>
		/// <returns>������ ���������� ������� ��������.</returns>
		public static Person[] GetOfficeNewsEditorsList()
		{
			try
			{
				Role officeNewsEditorRole = Role.GetRole( "OfficeNewsEditor" );

				List<Person> newsEditors = new List<Person>();
				foreach( Person person in GetUserList() )
				{
					if( officeNewsEditorRole != null )
					{
						if( officeNewsEditorRole.IsInRole( person.ID.Value ) )
						{
							newsEditors.Add( person );
							continue;
						}
					}
				}
				return newsEditors.ToArray();
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return new Person[ 0 ];
			}

		}

		/// <summary>
		/// ���������� ������ ���������� ����� ��������.
		/// </summary>
		/// <returns>������ ���������� ����� ��������.</returns>
		public static Person[] GetGeneralNewsEditorsList()
		{
			try
			{
				Role generalNewsEditorRole = Role.GetRole( "GeneralNewsEditor" );

				List<Person> newsEditors = new List<Person>();
				foreach( Person person in GetUserList() )
				{
					if( generalNewsEditorRole != null )
					{
						if( generalNewsEditorRole.IsInRole( person.ID.Value ) )
						{
							newsEditors.Add( person );
							continue;
						}
					}
				}
				return newsEditors.ToArray();
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return new Person[ 0 ];
			}

		}
		/// <summary>
		/// ���������� ������ ���������� ���� ��������.
		/// </summary>
		/// <returns>������ ���������� ���� ��������.</returns>
		public static Person[] GetNewsEditorsList()
		{
			try
			{
				Role officeNewsEditorRole = Role.GetRole( "OfficeNewsEditor" );
				Role generalNewsEditorRole = Role.GetRole( "GeneralNewsEditor" );

				List<Person> newsEditors = new List<Person>();
				foreach( Person person in GetUserList() )
				{
					if( officeNewsEditorRole != null )
					{
						if( officeNewsEditorRole.IsInRole( person.ID.Value ) )
						{
							newsEditors.Add( person );
							continue;
						}
					}
					if( generalNewsEditorRole != null )
					{
						if( generalNewsEditorRole.IsInRole( person.ID.Value ) )
						{
							newsEditors.Add( person );
							continue;
						}
					}
				}
				return newsEditors.ToArray();
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return new Person[ 0 ];
			}

		}
		/// <summary>
		/// ���������� ������ ������������� � ��������� �������� ���������.
		/// </summary>
		/// <returns>������ ������������� � ��������� �������� ���������.</returns>
		public static Person[] GetUserListWithOpenWorkPeriod()
		{
			DataTable dt = DBManager.GetUserListWithOpenWorkPeriod();
			if( dt == null )
				return null;

			List<Person> usersList = new List<Person>();

			foreach( DataRow row in dt.Rows )
			{
				int id = (int) row[ "ID" ];
				Person user = new Person();
				if( user.Load( id ) )
					usersList.Add( user );
			}

			if( usersList.Count > 0 )
				return usersList.ToArray();
			else
				return null;
		}



		/// <summary>
		/// ���������� ������ ���������� � �������� ������������� �� ��������� ����.
		/// </summary>
		/// <param name="date">���� ��� ��������� ���������� � �������������.</param>
		/// <returns>������ ���������� � �������� ������������� �� ��������� ����.</returns>
        public static UserStatusInfo[] GetStatusesList(DateTime date)
		{
			List<UserStatusInfo> usersList = new List<UserStatusInfo>();
            Person[] activeUsers = GetEmployeeList();

			if( ( activeUsers == null ) || ( activeUsers.Length == 0 ) )
				return usersList.ToArray();

			foreach( Person user in activeUsers )
			{
				// ������� ���������� ��������.
				if( user.EmployeesUlterSYSMoscow )
					continue;

				// �������� ��������� ������� ������������.
				WorkEvent lastEvent = WorkEvent.GetCurrentEventOfDate( user.ID.Value, date, true );
				// ��� ��������� ������� �������� �������.
				WorkEvent workEvent = null;

				// �������� ������ ������������.
				UptimeEventType status;

				if( lastEvent == null )
				{
					// ��� ������� �� �������. ������� �� ��������.
					status = UptimeEventType.GetEventType( (int) WorkEventType.TimeOff );
				}
				else
				{
					switch( lastEvent.EventType )
					{
						case WorkEventType.BusinessTrip:
						case WorkEventType.Ill:
						case WorkEventType.TrustIll:
						case WorkEventType.Vacation:
							status = UptimeEventType.GetEventType( lastEvent.EventTypeID );
							break;

						case WorkEventType.MainWork:
							workEvent = lastEvent;
							if( lastEvent.IsOpen )
							{ status = UptimeEventType.GetEventType( (int) WorkEventType.MainWork ); }
							else
							{ status = UptimeEventType.GetEventType( (int) WorkEventType.TimeOff ); }
							break;

						default:
							workEvent = WorkEvent.GetMainWorkEvent( user.ID.Value, date );
							if( lastEvent.IsOpen )
							{ status = UptimeEventType.GetEventType( lastEvent.EventTypeID ); }
							else
							{
								if( workEvent == null )
								{ status = UptimeEventType.GetEventType( (int) WorkEventType.TimeOff ); }
								else
								{
									if( workEvent.IsOpen )
									{ status = UptimeEventType.GetEventType( (int) WorkEventType.MainWork ); }
									else
									{ status = UptimeEventType.GetEventType( (int) WorkEventType.TimeOff ); }
								}
							}
							break;
					}
				}

				// ������� ���������� � ������������.
				String loginName = string.Empty;
				IList<string> dnNames = user.DomainNames;
				if( dnNames != null && dnNames.Count > 0 )
				{
					loginName = dnNames[ 0 ];
					int startIndex = loginName.LastIndexOf( "\\" );
					if( !string.IsNullOrEmpty( loginName ) && ( startIndex > 0 ) )
						loginName = loginName.Substring( startIndex + 1, 3 );
				}

				UserStatusInfo info;
				if( workEvent == null )
					info = new UserStatusInfo( user.ID.Value, user.FullName, loginName, status );
				else
				{
					UserTimeCalculator timesCalc = new UserTimeCalculator( user.ID.Value );

					// ����� �� ��������� ���.
					TimeSpan todayRest = timesCalc.GetRateWithLunch( DateTime.Today );

					// ��������� ������ (��������� ��� �����������).
                   /* if (workEvent.BeginTime == workEvent.EndTime)
                        workEvent.EndTime = (todayRest.TotalMilliseconds > 0)
                                                ? workEvent.BeginTime.Add(todayRest)
                                                : DateTime.Now;*/
						/*if( todayRest.TotalMilliseconds > 0 )
							workEvent.EndTime = DateTime.Now.Add( todayRest );
						else
							workEvent.EndTime = DateTime.Now;*/

					info = new UserStatusInfo( user.ID.Value, user.FullName, loginName, status, workEvent.BeginTime, workEvent.EndTime );
				}

				usersList.Add( info );
			}

            return usersList.ToArray();
		}
		#endregion
	}

	/// <summary>
	/// ����� ���������� � ������������� � �� �������� (and our time).
	/// </summary>
    public class UserStatusInfo
	{
		#region ����
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly int m_UserID;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly string m_UserName;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly string m_USLName;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly DateTime m_BeginWork;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly DateTime m_EndWork;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private readonly UptimeEventType m_EventType;
		#endregion

		#region ��������
		/// <summary>
		/// ID ������������.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
		}

		/// <summary>
		/// ��� ������������.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
		}

		/// <summary>
		/// ��� ������������.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
		}

		/// <summary>
		/// ������ ������������.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_EventType.Name; }
		}

		/// <summary>
		/// ��� ��������� ������������.
		/// </summary>
		public UptimeEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
		}

		/// <summary>
		/// ��������� ����� ��� �������� �������.
		/// </summary>
		public DateTime BeginWork
		{
			[DebuggerStepThrough]
			get { return m_BeginWork; }
		}

		/// <summary>
		/// ��������� �������� �������.
		/// </summary>
		public DateTime EndWork
		{
			[DebuggerStepThrough]
			get { return m_EndWork; }
		}

		#endregion

		#region ������������
		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="userID">ID ������������.</param>
		/// <param name="userName">��� ������������.</param>
		/// <param name="uslName">������������� ��� ������������.</param>
		/// <param name="eventType">��� ��������� ������������.</param>
		public UserStatusInfo( int userID, string userName, string uslName, UptimeEventType eventType )
		{
			m_UserID = userID;
			if( string.IsNullOrEmpty( userName ) )
				throw new ArgumentNullException( "userName" );
			if( string.IsNullOrEmpty( uslName ) )
				throw new ArgumentNullException( "uslName" );
			if( eventType == null )
				throw new ArgumentNullException( "eventType" );

			m_UserName = userName;
			m_USLName = uslName;
			m_EventType = eventType;
		}

		/// <summary>
		/// ����������� � ���������� �������.
		/// </summary>
		/// <param name="userID">ID ������������.</param>
		/// <param name="userName">��� ������������.</param>
		/// <param name="uslName">������������� ��� ������������.</param>
		/// <param name="eventType">��� ��������� ������������.</param>
		/// <param name="beginTime">Begin Main work.</param>
		/// <param name="endTime">End Main work.</param>
		public UserStatusInfo( int userID, string userName, string uslName, UptimeEventType eventType,
										DateTime beginTime, DateTime endTime )
		{
			m_UserID = userID;
			if( string.IsNullOrEmpty( userName ) )
				throw new ArgumentNullException( "userName" );
			if( string.IsNullOrEmpty( uslName ) )
				throw new ArgumentNullException( "uslName" );
			if( eventType == null )
				throw new ArgumentNullException( "eventType" );

			m_UserName = userName;
			m_USLName = uslName;
			m_EventType = eventType;
			m_BeginWork = beginTime;
			m_EndWork = endTime;
		}
		#endregion
    }

	/// <summary>
	/// ����� ���������� � ������������� � �� ��������, ��������� ��� XML-������������.
	/// </summary>
	[Serializable]
	public class XMLSerializableUserStatusInfo
	{
		#region ����

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_UserID;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_UserName;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_USLName;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_Status;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_BeginWork;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_EndWork;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private WorkEventType m_EventType;

		#endregion

		#region ��������
		/// <summary>
		/// ID ������������.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
			[DebuggerStepThrough]
			set { m_UserID = value; }
		}

		/// <summary>
		/// ��� ������������.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
			[DebuggerStepThrough]
			set { m_UserName = value; }
		}

		/// <summary>
		/// ��� ������������.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
			[DebuggerStepThrough]
			set { m_USLName = value; }
		}

		/// <summary>
		/// ������ ������������.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_Status; }
			[DebuggerStepThrough]
			set { m_Status = value; }
		}

		/// <summary>
		/// ��������� ����� ��� �������� �������.
		/// </summary>
		public DateTime BeginWork
		{
			[DebuggerStepThrough]
			get { return m_BeginWork; }
			[DebuggerStepThrough]
			set { m_BeginWork = value; }
		}

		/// <summary>
		/// ��������� �������� �������.
		/// </summary>
		public DateTime EndWork
		{
			[DebuggerStepThrough]
			get { return m_EndWork; }
			[DebuggerStepThrough]
			set { m_EndWork = value; }
		}

		/// <summary>
		/// ��� �������� �������.
		/// </summary>
		public WorkEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
			[DebuggerStepThrough]
			set { m_EventType = value; }
		}
		#endregion

		#region ������������
		/// <summary>
		/// ������ ����������� ��� XML-������������.
		/// </summary>
		public XMLSerializableUserStatusInfo()
		{ }

		/// <summary>
		/// ���������� �����������.
		/// </summary>
		/// <param name="usInfo">���������� � ������� ������������.</param>
		public XMLSerializableUserStatusInfo( UserStatusInfo usInfo )
		{
			if( usInfo == null )
				throw new ArgumentNullException( "usInfo" );

			UserID = usInfo.UserID;
			UserName = usInfo.UserName;
			USLName = usInfo.USLName;
			Status = usInfo.Status;
			BeginWork = usInfo.BeginWork;
			EndWork = usInfo.EndWork;
			EventType = (WorkEventType) usInfo.EventType.ID;
		}
		#endregion
	}
}

