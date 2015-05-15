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
	/// Кла�?�? �?пи�?ков пользователей.
	/// </summary>
	public class UserList
	{
		#region Методы

		/// <summary>
		/// Возвращает �?пи�?ок пользователей.
		/// </summary>
		/// <returns>������ �������������.</returns>
        public static Person[] GetUserList(bool sortOrderAsc = false, string propertyName = "")
		{
            BaseObjectCollection<Person> baseObjectCollection;
            if (propertyName != "")
                baseObjectCollection = (BaseObjectCollection<Person>)BasePlainObject.GetObjects(typeof(Person), propertyName, sortOrderAsc);
            else
                baseObjectCollection = (BaseObjectCollection<Person>)BasePlainObject.GetObjects(typeof(Person));

            if (baseObjectCollection == null)
                return null;

            return baseObjectCollection.ToArray();
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
		/// Возвращает �?пи�?ок по�?то�?нных �?лужащих.
		/// </summary>
		/// <returns>������ ���������� ��������.</returns>
        public static Person[] GetEmployeeList(bool isDescendingSortDirection = false, String propertyName = "")
		{
		    try
		    {
		        List<Person> employees = new List<Person>();
		        foreach (Person person in GetUserList(isDescendingSortDirection, propertyName))
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
		/// Возвращает �?пи�?ок админи�?траторов.
		/// </summary>
		/// <returns>Спи�?ок админи�?траторов.</returns>
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
        /// ���������� ������ ���������� �� ������.
        /// </summary>
        /// <returns>������ ���������� �� ������.</returns>
        public static Person[] GetHrManagerList()
        {
            try
            {
                List<Person> hrManagers = new List<Person>();
                foreach (Person person in GetUserList())
                {
                    if (person.IsInRole("HRManager"))
                        hrManagers.Add(person);
                }
                return hrManagers.ToArray();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return new Person[0];
            }
        }

		/// <summary>
		/// Возвращает �?пи�?ок редакторов офи�?ных ново�?тей.
		/// </summary>
		/// <returns>Спи�?ок редакторов офи�?ных ново�?тей.</returns>
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
		/// Возвращает �?пи�?ок редакторов общих ново�?тей.
		/// </summary>
		/// <returns>Спи�?ок редакторов общих ново�?тей.</returns>
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
		/// Возвращает �?пи�?ок редакторов в�?ех ново�?тей.
		/// </summary>
		/// <returns>Спи�?ок редакторов в�?ех ново�?тей.</returns>
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
		/// Возвращает �?пи�?ок пользователей �? открытыми рабочими �?обыти�?ми.
		/// </summary>
		/// <returns>Спи�?ок пользователей �? открытыми рабочими �?обыти�?ми.</returns>
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
		/// Возвращает �?пи�?ок информаций о �?тату�?ах пользователей за указанную дату.
		/// </summary>
		/// <param name="date">���� ��� ��������� ���������� � �������������.</param>
		/// <returns>������ ���������� � �������� ������������� �� ��������� ����.</returns>
        public static UserStatusInfo[] GetStatusesList(DateTime date, bool isDescendingSortDirection = false, String propertyName = "")
		{
			List<UserStatusInfo> usersList = new List<UserStatusInfo>();
            Person[] activeUsers = GetEmployeeList(isDescendingSortDirection, propertyName);

			if( ( activeUsers == null ) || ( activeUsers.Length == 0 ) )
				return usersList.ToArray();

			foreach( Person user in activeUsers )
			{
				// От�?е�?ть мо�?ков�?ких �?лужащих.
				if( user.EmployeesUlterSYSMoscow )
					continue;

				// Получить по�?леднее �?обытие пользовател�?.
				WorkEvent lastEvent = WorkEvent.GetCurrentEventOfDate( user.ID.Value, date, true );
				// Дл�? получени�? времени рабочего периода.
				WorkEvent workEvent = null;

				// Получить �?тату�? пользовател�?.
				UptimeEventType status;

				if( lastEvent == null )
				{
					// �?ет �?обытий за �?егодн�?. Человек не приходил.
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

				// Создать информацию о пользователе.
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

					// Врем�? до окончани�? дн�?.
					TimeSpan todayRest = timesCalc.GetRateWithLunch( DateTime.Today );

					// Окончание работы (ра�?чётное или фактиче�?кое).
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
	/// Кла�?�? информации о пользовател�?х и их �?тату�?ах (and our time).
	/// </summary>
    public class UserStatusInfo
	{
		#region Пол�?
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

		#region Свой�?тва
		/// <summary>
		/// ID пользовател�?.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
		}

		/// <summary>
		/// Им�? пользовател�?.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
		}

		/// <summary>
		/// Код пользовател�?.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
		}

		/// <summary>
		/// Стату�? пользовател�?.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_EventType.Name; }
		}

		/// <summary>
		/// Тип �?о�?то�?ни�? пользовател�?.
		/// </summary>
		public UptimeEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
		}

		/// <summary>
		/// �?ачальное врем�? дл�? рабочего периода.
		/// </summary>
		public DateTime BeginWork
		{
			[DebuggerStepThrough]
			get { return m_BeginWork; }
		}

		/// <summary>
		/// Окончание рабочего периода.
		/// </summary>
		public DateTime EndWork
		{
			[DebuggerStepThrough]
			get { return m_EndWork; }
		}

		#endregion

		#region Кон�?трукторы
		/// <summary>
		/// Кон�?труктор.
		/// </summary>
		/// <param name="userID">ID пользовател�?.</param>
		/// <param name="userName">Им�? пользовател�?.</param>
		/// <param name="uslName">Трехбуквенный код пользовател�?.</param>
		/// <param name="eventType">Тип �?о�?то�?ние пользовател�?.</param>
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
		/// Кон�?труктор �? интервалом времени.
		/// </summary>
		/// <param name="userID">ID пользовател�?.</param>
		/// <param name="userName">Им�? пользовател�?.</param>
		/// <param name="uslName">Трехбуквенный код пользовател�?.</param>
		/// <param name="eventType">Тип �?о�?то�?ние пользовател�?.</param>
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
	/// Кла�?�? информации о пользовател�?х и их �?тату�?ах, пригодный дл�? XML-�?ериализации.
	/// </summary>
	[Serializable]
	public class XMLSerializableUserStatusInfo
	{
		#region Пол�?

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

		#region Свой�?тва
		/// <summary>
		/// ID пользовател�?.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
			[DebuggerStepThrough]
			set { m_UserID = value; }
		}

		/// <summary>
		/// Им�? пользовател�?.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
			[DebuggerStepThrough]
			set { m_UserName = value; }
		}

		/// <summary>
		/// Код пользовател�?.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
			[DebuggerStepThrough]
			set { m_USLName = value; }
		}

		/// <summary>
		/// Стату�? пользовател�?.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_Status; }
			[DebuggerStepThrough]
			set { m_Status = value; }
		}

		/// <summary>
		/// �?ачальное врем�? дл�? рабочего периода.
		/// </summary>
		public DateTime BeginWork
		{
			[DebuggerStepThrough]
			get { return m_BeginWork; }
			[DebuggerStepThrough]
			set { m_BeginWork = value; }
		}

		/// <summary>
		/// Окончание рабочего периода.
		/// </summary>
		public DateTime EndWork
		{
			[DebuggerStepThrough]
			get { return m_EndWork; }
			[DebuggerStepThrough]
			set { m_EndWork = value; }
		}

		/// <summary>
		/// Тип текущего �?тату�?а.
		/// </summary>
		public WorkEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
			[DebuggerStepThrough]
			set { m_EventType = value; }
		}
		#endregion

		#region Кон�?трукторы
		/// <summary>
		/// Пу�?той кон�?труктор дл�? XML-�?ериализации.
		/// </summary>
		public XMLSerializableUserStatusInfo()
		{ }

		/// <summary>
		/// Копирующий кон�?труктор.
		/// </summary>
		/// <param name="usInfo">Информаци�? о �?тату�?е пользовател�?.</param>
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

