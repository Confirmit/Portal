using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Principal;
using System.Reflection;

using Core;
using Core.ORM.Attributes;

using ConfirmIt.PortalLib.Properties;
using ConfirmIt.PortalLib.DAL;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BAL.Events;
using ConfirmIt.PortalLib.BAL.Settings;
using ConfirmIt.PortalLib.BAL.Settings.Interceptors;

using UlterSystems.PortalLib.NewsTape;
using ConfirmIt.PortalLib;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// Class of user.
	/// </summary>
	[DBTable( "Users" )]
	public class Person : BasePlainObject, IIdentity, IPrincipal
	{
		#region Classes
		/// <summary>
		/// Gender of user.
		/// </summary>
		public enum UserSex
		{
			Male = 1,
			Female = 2,
			Unknown = 0
		}
		#endregion

		#region Fields

		#region IIdentity fields
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_IdentityName = string.Empty;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool m_IsAuthenticated = true;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_AuthenticationType = string.Empty;
		#endregion

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private PersonalSettings m_Settings = null;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private MLText m_FirstName = new MLText();

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private MLText m_MiddleName = new MLText();

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private MLText m_LastName = new MLText();

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private short _SexID = 1;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private UserSex _Sex = UserSex.Unknown;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime? _BirthDate = null;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _PrimaryEMail = string.Empty;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _Project = string.Empty;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _Room = string.Empty;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _PrimaryIP = string.Empty;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _LongServiceEmployees = true;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _PersonnelReserve = false;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _EmployeesUlterSYSMoscow = false;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _USL_name = string.Empty;

		#endregion

		#region Constructors

		/// <summary>
		/// Simple constructor.
		/// </summary>
		[DebuggerStepThrough]
		public Person()
		{}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="identity">Identity.</param>
		[DebuggerStepThrough]
		public Person( IIdentity identity )
		{
			if( identity == null )
				throw new ArgumentNullException( "identity" );

			m_IdentityName = identity.Name;
			m_AuthenticationType = identity.AuthenticationType;
			m_IsAuthenticated = identity.IsAuthenticated;
		}

		#endregion

		#region Properties

		/// <summary>
		/// First name of person.
		/// </summary>
		[DBRead( "FirstName" )]
		[DBNullable]
		public MLText FirstName
		{
			[DebuggerStepThrough]
			get { return m_FirstName; }
			[DebuggerStepThrough]
			set { m_FirstName = value; }
		}

		/// <summary>
		/// Middle name of person.
		/// </summary>
		[DBRead( "MiddleName" )]
		[DBNullable]
		public MLText MiddleName
		{
			[DebuggerStepThrough]
			get { return m_MiddleName; }
			[DebuggerStepThrough]
			set { m_MiddleName = value; }
		}

		/// <summary>
		/// Surname of person.
		/// </summary>
		[DBRead( "LastName" )]
		public MLText LastName
		{
			[DebuggerStepThrough]
			get { return m_LastName; }
			[DebuggerStepThrough]
			set { m_LastName = value; }
		}

		/// <summary>
		/// Full name of person.
		/// </summary>
		public string FullName
		{
			get
			{
				string fullName = string.Empty;
				if (!string.IsNullOrEmpty(FirstName.ToString()))
					fullName += FirstName.ToString() + " ";

				if (!string.IsNullOrEmpty(MiddleName.ToString()))
					fullName += MiddleName.ToString() + " ";

				fullName += LastName.ToString();
				return fullName;
			}
		}

		//@idm{
		/// <summary>
		/// First and Last name of person.
		/// </summary>
		//public string FirstAndLastNames
		//{
		//    get
		//    {
		//        string name = string.Empty;
		//        if (FirstName.ToString() != string.Empty)
		//            name += FirstName.ToString() + " ";
		//        name += LastName.ToString();
		//        return name;
		//    }
		//}
		//@idm}


		/// <summary>
		/// Domain names of person.
		/// </summary>
		/// <remarks>Use only for stored persons!</remarks>
		public string[] DomainNames
		{
			get
			{
				if( ID == null )
					return new string[ 0 ];

				try
				{
					IList<PersonAttribute> attrs = PersonAttributes.GetPersonAttributesByKeyword(ID.Value, PersonAttributeTypes.DomainName.ToString());
					if ((attrs == null) || (attrs.Count == 0))
						return new string[0];
					else
					{
						List<string> dNames = new List<string>(attrs.Count);
						foreach (PersonAttribute pa in attrs)
						{
							if (!string.IsNullOrEmpty(pa.StringField))
								dNames.Add(pa.StringField);
						}
						return dNames.ToArray();
					}
				}
				catch (Exception ex)
				{
					Logger.Log.Error(ex.Message, ex);
					return new string[0];
				}
			}
		}

		/// <summary>
		/// Get Attribute USL_Name
		/// </summary>
		public string USL_name
		{
			get
			{
				if (ID == null)
					return String.Empty;

					IList<PersonAttribute> attrs = PersonAttributes.GetPersonAttributesByKeyword(ID.Value, PersonAttributeTypes.USL_name.ToString());
					if (attrs != null && attrs.Count > 0)
					{

						_USL_name = attrs[0].StringField;
						return _USL_name;
					}
					else return String.Empty;
			}
		}

		/// <summary>
		/// Идентификатор пола.
		/// </summary>
		[DBRead( "Sex" )]
		public short SexID
		{
			[DebuggerStepThrough]
			get { return _SexID; }
			set
			{
				_SexID = value;
				_Sex = (UserSex) value;
			}
		}

		/// <summary>
		/// Пол пользователя.
		/// </summary>
		public UserSex Sex
		{
			[DebuggerStepThrough]
			get { return _Sex; }
			set
			{
				_Sex = value;
				_SexID = (short) value;
			}
		}

		/// <summary>
		/// День рождения пользователя.
		/// </summary>
		[DBRead( "Birthday" )]
		[DBNullable]
		public DateTime? Birthday
		{
			[DebuggerStepThrough]
			get { return _BirthDate; }
			[DebuggerStepThrough]
			set { _BirthDate = value; }
		}

		/// <summary>
		/// E-Mail пользователя.
		/// </summary>
		[DBRead( "PrimaryEMail" )]
		[DBNullable]
		public string PrimaryEMail
		{
			[DebuggerStepThrough]
			get { return _PrimaryEMail; }
			[DebuggerStepThrough]
			set { _PrimaryEMail = value; }
		}

		/// <summary>
		/// Проект пользователя.
		/// </summary>
		[DBRead( "Project" )]
		[DBNullable]
		public string Project
		{
			[DebuggerStepThrough]
			get { return _Project; }
			[DebuggerStepThrough]
			set { _Project = value; }
		}

		/// <summary>
		/// Комната пользователя.
		/// </summary>
		[DBRead( "Room" )]
		[DBNullable]
		public string Room
		{
			[DebuggerStepThrough]
			get { return _Room; }
			[DebuggerStepThrough]
			set { _Room = value; }
		}

		/// <summary>
		/// IP пользователя.
		/// </summary>
		[DBRead( "PrimaryIP" )]
		[DBNullable]
		public string PrimaryIP
		{
			[DebuggerStepThrough]
			get { return _PrimaryIP; }
			[DebuggerStepThrough]
			set { _PrimaryIP = value; }
		}

		/// <summary>
		/// Постоянный рабочий.
		/// </summary>
		[DBRead( "LongServiceEmployees" )]
		public bool LongServiceEmployees
		{
			[DebuggerStepThrough]
			get { return _LongServiceEmployees; }
			[DebuggerStepThrough]
			set { _LongServiceEmployees = value; }
		}

		/// <summary>
		/// ?.
		/// </summary>
		[DBRead( "PersonnelReserve" )]
		public bool PersonnelReserve
		{
			[DebuggerStepThrough]
			get { return _PersonnelReserve; }
			[DebuggerStepThrough]
			set { _PersonnelReserve = value; }
		}

		/// <summary>
		/// Работает в Москве.
		/// </summary>
		[DBRead( "EmployeesUlterSYSMoscow" )]
		public bool EmployeesUlterSYSMoscow
		{
			[DebuggerStepThrough]
			get { return _EmployeesUlterSYSMoscow; }
			[DebuggerStepThrough]
			set { _EmployeesUlterSYSMoscow = value; }
		}

		/// <summary>
		/// Способности пользователя.
		/// </summary>
		public IList<Ability> Abilities
		{
			get { return SiteProvider.Abilities.GetAllUserAbilities(ID.Value); }
		}

		/// <summary>
		/// Groups for which this person is a member.
		/// </summary>
		public IList<Role> Roles
		{
			get
			{
				if (!ID.HasValue)
					return new Role[0];

				List<Role> userRoles = new List<Role>();
				foreach (Role role in Role.GetUserRoles(ID.Value))
				{
					userRoles.Add(role);
				}

				return userRoles;//.ToArray();
			}
		}

		/// <summary>
		/// Names of groups for which this person is a member.
		/// </summary>
		public string[] GroupNames
		{
			get
			{
				List<string> roles = new List<string>();
				if( ID.HasValue )
				{
					foreach( Role role in Role.GetUserRoles( ID.Value ) )
					{
						roles.Add( role.RoleID );
					}
				}
				return roles.ToArray();
			}
		}

		/// <summary>
		/// Return all events for user.
		/// </summary>
		public IList<UserEvent> Events
		{
			get
			{
				if (!ID.HasValue)
					return new List<UserEvent>();

				return SiteProvider.Events.GetAllUserEvents(ID.Value);
			}
		}

		#endregion

		#region RequestUserCallback

		/// <summary>
		/// Делегат для возвращения текущего пользователя.
		/// </summary>
		/// <returns></returns>
		public delegate Person RequestUserCallback();

		/// <summary>
		/// Должен возвращать текущего пользователя. 
		/// Приложения, заинтересованные в получении текущего пользователя, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static RequestUserCallback RequestUser;

		/// <summary>
		/// Возвращает текущего пользователя. 
		/// </summary>
		public static Person Current
		{
			get
			{
				if (RequestUser != null)
					return RequestUser();

				return null;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Check if user can edit news.
		/// </summary>
		/// <returns></returns>
		public bool IsCanEditNews(News News)
		{
			if (News.AuthorID == this.ID || IsInRole(RolesEnum.Administrator))
				return true;

			// TODO: Remake edit news.
		   /* if (IsInRole("OfficeNewsEditor") && News.OfficeID != 0)
				return true;

			if (IsInRole("GeneralNewsEditor") && News.OfficeID == 0)
				return true;*/

			return false;
		}

		/// <summary>
		/// Check if user can edit event.
		/// </summary>
		/// <returns></returns>
		public bool IsCanEditEvent(Event eventData)
		{
			return IsCanEditEvent(eventData.ID.Value);
		}

		/// <summary>
		/// Check if user can edit event.
		/// </summary>
		/// <returns></returns>
		public bool IsCanEditEvent(int userEventID)
		{
			UserEvent userEvent = new UserEvent(userEventID);
			return IsCanEditEvent(userEvent);
		}

		/// <summary>
		/// Check if user can edit event.
		/// </summary>
		/// <returns></returns>
		public bool IsCanEditEvent(UserEvent userEvent)
		{
			if (!ID.HasValue)
				return false;

			// if personal event or user is admin.
			if (userEvent.OwnerID == ID.Value
				|| !userEvent.IsSaved
				|| IsInRole(RolesEnum.Administrator))
				return true;

			return false;
		}

		//Лазарев
		/// <summary>
		/// Персональные (пользовательские) настройки
		/// </summary>
		public PersonalSettings PersonSettings
		{
			get
			{
				if (m_Settings == null)
				{
					if (IsSaved)
					{
						Castle.DynamicProxy.ProxyGenerator proxy = new Castle.DynamicProxy.ProxyGenerator();
						PersonalSettingsInterceptor interceptor = new PersonalSettingsInterceptor(this);
						m_Settings = (PersonalSettings)proxy.CreateClassProxy(typeof(PersonalSettings), interceptor);
					}
					else
					{
						throw new Exception("Incorrect usage of PersonSettings property of Person. Person object is not saved.");
					}
				}
				return m_Settings;
			}
		}


		#region Methods for work with events

		/// <summary>
		/// Add individual event to person.
		/// </summary>
		/// <param name="eventData">event data.</param>
		public void AddIndividualEvent(Event eventData)
		{
			if (!ID.HasValue)
				return;

			UserEventsManager.AddIndividualEvent(ID.Value, eventData);
		}

		/// <summary>
		/// Remove individual person event.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void DeleteIndividualEvent(int eventID)
		{
			if (!ID.HasValue)
				return;

			UserEventsManager.DeleteIndividualEvent(ID.Value, eventID);
		}

		#endregion

		#region Methods for work with attributes

		/// <summary>
		/// Adds standard string attribute.
		/// </summary>
		/// <param name="type">Type of attribute.</param>
		/// <param name="value">Value of attribute</param>
		/// <returns>Created attribute.</returns>
		public PersonAttribute AddStandardStringAttribute(PersonAttributeTypes type, string value)
		{
			return AddStandardStringAttribute(type.ToString(), value);
		}

		/// <summary>
		/// Adds standard string attribute.
		/// </summary>
		/// <param name="typeName">Type name of attribute.</param>
		/// <param name="value">Value of attribute.</param>
		/// <returns>Created attribute.</returns>
		public PersonAttribute AddStandardStringAttribute(string typeName, string value)
		{
			if (ID == null || string.IsNullOrEmpty(value))
				return null;

			try
			{
				PersonAttributeType type = PersonAttributeType.GetAttributeType(typeName);
				if (type == null)
				{
					type = new PersonAttributeType() { AttributeName = typeName, ShowToUsers = false };
					type.Save();
				}

				return AddStandardStringAttribute(type, value);

			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return null;
			}
		}

		/// <summary>
		/// Adds standard string attribute.
		/// </summary>
		/// <param name="type">Type of attribute.</param>
		/// <param name="value">Value of attribute.</param>
		/// <returns>Created attribute.</returns>
		public PersonAttribute AddStandardStringAttribute(PersonAttributeType type, string value)
		{
			if (ID == null || string.IsNullOrEmpty(value))
				return null;

			try
			{
				var pa = new PersonAttribute
				         	{
				         		PersonID = ID.Value,
				         		InsertionDate = DateTime.Now,
				         		AttributeID = type.ID.Value,
				         		ValueType = typeof (string).AssemblyQualifiedName,
				         		StringField = value
				         	};
				pa.Save();
				return pa;
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return null;
			}
		}

		/// <summary>
		/// Removes all attributes of given type.
		/// </summary>
		/// <param name="type">Type of attribute.</param>
		public void RemoveStandardAttributes(PersonAttributeType type)
		{
			if (ID == null)
				return;

			try
			{
				var attrs = PersonAttributes.GetPersonAttributesByKeyword(ID.Value, type);
				if (attrs == null)
					return;

				foreach (var pa in attrs)
				{
					pa.Delete();
				}
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
			}
		}

		#endregion

		/// <summary>
		/// Загружает информацию о пользователе по его доменному имени.
		/// </summary>
		/// <param name="domainName">Доменное имя пользователя.</param>
		public bool LoadByDomainName( string domainName )
		{
			if (string.IsNullOrEmpty(domainName))
				throw new ArgumentNullException("domainName");

			try
			{
				var dnAttributes = PersonAttributes.GetPersonAttributesByKeyword( PersonAttributeTypes.DomainName.ToString() );
				if( dnAttributes == null )
					return false;

				foreach (var dnAttr in dnAttributes)
				{
					if (dnAttr.Type == typeof (string))
					{
						if (string.Compare(domainName, dnAttr.StringField, true) == 0)
							return Load(dnAttr.PersonID);
					}
				}

				return false;
			}
			catch( Exception ex )
			{
				Logger.Log.Error( ex.Message, ex );
				return false;
			}

			//if( !LoadByReference( "DomainName", domainName ) )
			//   return false;
			//ReloadRoles();
			//_Attributes = new UserAttributes( ID.Value );
			//return true;
		}

		/// <summary>
		/// Returns person with given domain name. Uses caching.
		/// </summary>
		/// <param name="domainName">Domain name.</param>
		/// <returns>Person with given domain name.</returns>
		public static Person GetPersonByDomainName( string domainName )
		{
			if( string.IsNullOrEmpty( domainName ) )
				return null;

			try
			{
				string cacheKey = String.Format("Person with domain name '{0}'", domainName.ToLower());
				if (Cache.Contains(cacheKey))
				{
					DateTime now = DateTime.Now;
					DateTime insertDate = Cache.InsertDate(cacheKey).Value;
					if (insertDate < (now - Settings.Default.PersonExpireTime))
						Cache.Remove(cacheKey);
					else
						return (Person) Cache.GetObject(cacheKey);
				}

				Person p = new Person();
				if (p.LoadByDomainName(domainName))
				{
					Cache.Add(cacheKey, p);
					return p;
				}

				return null;
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return null;
			}
		}

		/// <summary>
		/// Returns person with given ID.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <returns>Person with given ID.</returns>
		public static Person GetPersonByID(int personID)
		{
			try
			{
				string cacheKey = String.Format("Person with ID '{0}'", personID);
				if (Cache.Contains(cacheKey))
				{
					DateTime now = DateTime.Now;
					DateTime insertDate = Cache.InsertDate(cacheKey).Value;
					if (insertDate < (now - Settings.Default.PersonExpireTime))
						Cache.Remove(cacheKey);
					else
						return (Person) Cache.GetObject(cacheKey);
				}

				Person p = new Person();
				if (p.Load(personID))
				{
					Cache.Add(cacheKey, p);
					return p;
				}

				return null;
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return null;
			}
		}

		public IList<Project> GetUserProjects(bool returnCompleteProjects)
		{
			return SiteProvider.Projects.GetUserProjects(ID.Value, returnCompleteProjects);
		}

		#region Methods to work with Objects like book, card, etc

		public void TakeObject(int ObjectID)
		{
			SiteProvider.RequestObjects.CreateRequest(ObjectID, ID, DateTime.Now, true);
		}

		public void GrantObject(int ObjectID, int? UserID)
		{
			SiteProvider.RequestObjects.CreateRequest(ObjectID, UserID, DateTime.Now, true);
		}

		#endregion

		#endregion

		#region Overrides

		/// <summary>
		/// Deletes object from database.
		/// </summary>
		public override void Delete()
		{
			if (ID == null)
				return;

			// Remove person from all groups.
			foreach (Role role in Role.GetUserRoles(ID.Value))
			{
				role.RemoveUser(ID.Value);
			}

			// Delete all attributes.
			IList<PersonAttribute> pAttribs = PersonAttributes.GetAllPersonAttributes(ID.Value);
			if (pAttribs != null)
			{
				foreach (PersonAttribute pAttrib in pAttribs)
				{
					pAttrib.Delete();
				}
			}

			// Delete all events of person.
			SiteProvider.WorkEvents.DeleteUserEvents(ID.Value);

			base.Delete();
		}

		#endregion

		#region IIdentity Members

		/// <summary>
		/// Authentication type.
		/// </summary>
		public string AuthenticationType
		{
			[DebuggerStepThrough]
			get { return m_AuthenticationType; }
		}

		/// <summary>
		/// Is user authenticated.
		/// </summary>
		public bool IsAuthenticated
		{
			[DebuggerStepThrough]
			get { return m_IsAuthenticated; }
		}

		/// <summary>
		/// Authentication name of user.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return m_IdentityName; }
		}

		#endregion

		#region IPrincipal Members

		/// <summary>
		/// Identity.
		/// </summary>
		public IIdentity Identity
		{
			[DebuggerStepThrough]
			get { return this; }
		}

		public bool IsInRole(RolesEnum roleID)
		{
			return IsInRole(roleID.ToString());
		}

		/// <summary>
		/// Is this user in role.
		/// </summary>
		/// <param name="roleID">Role string ID to check.</param>
		/// <returns>Is this user in role.</returns>
		public bool IsInRole(string roleID)
		{
			if (string.IsNullOrEmpty(roleID) || !ID.HasValue)
				return false;

			Role role = Role.GetRole(roleID);
			if (role == null)
				return false;

			return role.IsInRole(ID.Value);
		}

		/// <summary>
		/// Is this user a member of given role.
		/// </summary>
		/// <param name="role">Role to check.</param>
		/// <returns>Is this user a member of given role.</returns>
		public bool IsInRole(Role role)
		{
			if (role == null)
				throw new ArgumentNullException("role");

			if (!ID.HasValue)
				return false;

			return role.IsInRole(ID.Value);
		}

		#endregion
	}
}
