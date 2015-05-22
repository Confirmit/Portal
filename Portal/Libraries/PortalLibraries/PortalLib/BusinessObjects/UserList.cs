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
	/// Класс списков пользователей.
	/// </summary>
	public class UserList
	{
		#region Методы

		/// <summary>
		/// Возвращает список пользователей.
		/// </summary>
		/// <returns>Список пользователей.</returns>
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
		/// Возвращает список постоянных служащих.
		/// </summary>
		/// <returns>Список постоянных служащих.</returns>
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
		/// Возвращает список администраторов.
		/// </summary>
		/// <returns>Список администраторов.</returns>
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
		/// Возвращает список редакторов офисных новостей.
		/// </summary>
		/// <returns>Список редакторов офисных новостей.</returns>
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
		/// Возвращает список редакторов общих новостей.
		/// </summary>
		/// <returns>Список редакторов общих новостей.</returns>
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
		/// Возвращает список редакторов всех новостей.
		/// </summary>
		/// <returns>Список редакторов всех новостей.</returns>
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
		/// Возвращает список пользователей с открытыми рабочими событиями.
		/// </summary>
		/// <returns>Список пользователей с открытыми рабочими событиями.</returns>
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
		/// Возвращает список информаций о статусах пользователей за указанную дату.
		/// </summary>
		/// <param name="date">Дата для получения информации о пользователях.</param>
		/// <returns>Список информаций о статусах пользователей за указанную дату.</returns>
		public static UserStatusInfo[] GetStatusesList( DateTime date )
		{
			List<UserStatusInfo> usersList = new List<UserStatusInfo>();
			Person[] activeUsers = GetEmployeeList();

			if( ( activeUsers == null ) || ( activeUsers.Length == 0 ) )
				return usersList.ToArray();

			foreach( Person user in activeUsers )
			{
				// Отсеять московских служащих.
				if( user.EmployeesUlterSYSMoscow )
					continue;

				// Получить последнее событие пользователя.
				WorkEvent lastEvent = WorkEvent.GetCurrentEventOfDate( user.ID.Value, date, true );
				// Для получения времени рабочего периода.
				WorkEvent workEvent = null;

				// Получить статус пользователя.
				UptimeEventType status;

				if( lastEvent == null )
				{
					// Нет событий за сегодня. Человек не приходил.
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

					// Время до окончания дня.
					TimeSpan todayRest = timesCalc.GetRateWithLunch( DateTime.Today );

					// Окончание работы (расчётное или фактическое).
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
	/// Класс информации о пользователях и их статусах (and our time).
	/// </summary>
	public class UserStatusInfo
	{
		#region Поля
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

		#region Свойства
		/// <summary>
		/// ID пользователя.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
		}

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
		}

		/// <summary>
		/// Код пользователя.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
		}

		/// <summary>
		/// Статус пользователя.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_EventType.Name; }
		}

		/// <summary>
		/// Тип состояния пользователя.
		/// </summary>
		public UptimeEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
		}

		/// <summary>
		/// Начальное время для рабочего периода.
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

		#region Конструкторы
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="userID">ID пользователя.</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="uslName">Трехбуквенный код пользователя.</param>
		/// <param name="eventType">Тип состояние пользователя.</param>
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
		/// Конструктор с интервалом времени.
		/// </summary>
		/// <param name="userID">ID пользователя.</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="uslName">Трехбуквенный код пользователя.</param>
		/// <param name="eventType">Тип состояние пользователя.</param>
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
	/// Класс информации о пользователях и их статусах, пригодный для XML-сериализации.
	/// </summary>
	[Serializable]
	public class XMLSerializableUserStatusInfo
	{
		#region Поля

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

		#region Свойства
		/// <summary>
		/// ID пользователя.
		/// </summary>
		public int UserID
		{
			[DebuggerStepThrough]
			get { return m_UserID; }
			[DebuggerStepThrough]
			set { m_UserID = value; }
		}

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string UserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
			[DebuggerStepThrough]
			set { m_UserName = value; }
		}

		/// <summary>
		/// Код пользователя.
		/// </summary>
		public string USLName
		{
			[DebuggerStepThrough]
			get { return m_USLName; }
			[DebuggerStepThrough]
			set { m_USLName = value; }
		}

		/// <summary>
		/// Статус пользователя.
		/// </summary>
		public string Status
		{
			[DebuggerStepThrough]
			get { return m_Status; }
			[DebuggerStepThrough]
			set { m_Status = value; }
		}

		/// <summary>
		/// Начальное время для рабочего периода.
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
		/// Тип текущего статуса.
		/// </summary>
		public WorkEventType EventType
		{
			[DebuggerStepThrough]
			get { return m_EventType; }
			[DebuggerStepThrough]
			set { m_EventType = value; }
		}
		#endregion

		#region Конструкторы
		/// <summary>
		/// Пустой конструктор для XML-сериализации.
		/// </summary>
		public XMLSerializableUserStatusInfo()
		{ }

		/// <summary>
		/// Копирующий конструктор.
		/// </summary>
		/// <param name="usInfo">Информация о статусе пользователя.</param>
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

