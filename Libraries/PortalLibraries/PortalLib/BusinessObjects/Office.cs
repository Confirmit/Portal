using System;
using System.Collections.Generic;
using System.Diagnostics;

using Core;
using Core.ORM.Attributes;

using ConfirmIt.PortalLib.BAL.Settings;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// Класс с описанием офиса.
	/// </summary>
	[DBTable("Offices")]
	public class Office : BasePlainObject, ISetting
	{
		#region Поля
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private SettingAttribute m_SettingAttribute = null;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_OfficeName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_StatusesServiceURL;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_StatusesServiceUserName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_StatusesServicePassword;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_MeteoInformer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_ClockInformer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_DigitalClockInformer;
		#endregion

        #region ISetting Members

        public string KeyColumnName
        {
            get { return "OfficeName"; }
        }

        public SettingAttribute SettingAttribute
        {
            get 
            {
                return new SettingAttribute(SettingType.Office, OfficeName);
                /*return (m_SettingAttribute == null) ? (new SettingAttribute(SettingType.Office, OfficeName)) : m_SettingAttribute;*/
            }
        }

        public object Value
        {
            get { return StatusesServiceURL; }
            set { StatusesServiceURL = (string)value; }
        }

        #endregion

		#region Свойства

		/// <summary>
		/// Название офиса.
		/// </summary>
		[DBRead("OfficeName")]
		public string OfficeName
		{
			[DebuggerStepThrough]
			get { return m_OfficeName; }
			[DebuggerStepThrough]
			set { m_OfficeName = value; }
		}

		/// <summary>
		/// URL сервиса статусов пользователей офиса.
		/// </summary>
		[DBRead("StatusesServiceURL")]
		[DBNullable]
		public string StatusesServiceURL
		{
			[DebuggerStepThrough]
			get { return m_StatusesServiceURL; }
			[DebuggerStepThrough]
			set { m_StatusesServiceURL = value; }
		}

		/// <summary>
		/// Name of user to access to Web service.
		/// </summary>
		[DBRead("StatusesServiceUserName")]
		[DBNullable]
		public string StatusesServiceUserName
		{
			[DebuggerStepThrough]
			get { return m_StatusesServiceUserName; }
			[DebuggerStepThrough]
			set { m_StatusesServiceUserName = value; }
		}

		/// <summary>
		/// Password of user to access to Web service.
		/// </summary>
		[DBRead("StatusesServicePassword")]
		[DBNullable]
		public string StatusesServicePassword
		{
			[DebuggerStepThrough]
			get { return m_StatusesServicePassword; }
			[DebuggerStepThrough]
			set { m_StatusesServicePassword = value; }
		}

		/// <summary>
		/// Meteo Informer's string.
		/// </summary>
		[DBRead("MeteoInformer")]
		[DBNullable]
		public string MeteoInformer
		{
			[DebuggerStepThrough]
			get { return m_MeteoInformer; }
			[DebuggerStepThrough]
			set { m_MeteoInformer = value; }
		}

		/// <summary>
		/// Clock Informer's string.
		/// </summary>
		[DBRead("ClockInformer")]
		[DBNullable]
		public string ClockInformer
		{
			[DebuggerStepThrough]
			get { return m_ClockInformer; }
			[DebuggerStepThrough]
			set { m_ClockInformer = value; }
		}

		/// <summary>
		/// Clock Informer's string.
		/// </summary>
		[DBRead("DigitalClockInformer")]
		[DBNullable]
		public string DigitalClockInformer
		{
			[DebuggerStepThrough]
			get { return m_DigitalClockInformer; }
			[DebuggerStepThrough]
			set { m_DigitalClockInformer = value; }
		}
		#endregion

		#region Методы
		/// <summary>
		/// Возвращает все офисы.
		/// </summary>
		/// <returns>Массив офисов.</returns>
		public static Office[] GetOffices()
		{
			BaseObjectCollection<Office> coll = (BaseObjectCollection<Office>)BasePlainObject.GetObjects(typeof(Office));
			if (coll == null)
				return null;

		    return coll.ToArray();
		}

		/// <summary>
		/// Возвращает имена всех офисов для данного пользователя.
		/// </summary>
		/// <param name="personID">ID пользователя.</param>
		/// <returns>Имена всех офисов для данного пользователя.</returns>
		public static string[] GetUserOfficesNames(int personID)
		{
			Office[] offices = GetUserOffices(personID);
			int i = 0;
			string[] officeNames = new string[offices.Length];
			foreach (Office office in offices)
			{
				officeNames[i++] = office.OfficeName;
			}
			return officeNames;
		}



		/// <summary>
		/// Возвращает все офисы для данного пользователя.
		/// </summary>
		/// <param name="personID">ID пользователя.</param>
		/// <returns>Все офисы для данного пользователя.</returns>
		public static Office[] GetUserOffices(int personID)
		{
			IList<PersonAttribute> officeAttributes = PersonAttributes.GetPersonAttributesByKeyword(personID, "Office");
			object[] param = new object[officeAttributes.Count];
			int i = 0;
			foreach (PersonAttribute officeAttr in officeAttributes)
			{
				if (officeAttr.ValueType == typeof(int).AssemblyQualifiedName)
				{
					param[i++] = officeAttr.IntegerField;
				}
			}

			BaseObjectCollection<Office> coll = (BaseObjectCollection<Office>)BasePlainObject.GetObjects(typeof(Office), "ID", param);
		    return coll == null
		               ? null
		               : coll.ToArray();
		}

		#endregion
    }

    /// <summary>
    /// Класс информации о офисе, пригодный для XML-сериализации.
    /// </summary>
    [Serializable]
    public class XMLSerializableOffice
    {
        #region Поля
		private string m_OfficeName;
		private string m_StatusesServiceURL;
		private string m_StatusesServiceUserName;
		private string m_StatusesServicePassword;

		private string m_MeteoInformer;
		private string m_ClockInformer;
		private string m_DigitalClockInformer;
        #endregion

        #region Свойства

        /// <summary>
        /// Название офиса.
        /// </summary>
        public string Name
        {
            get
            { return m_OfficeName; }
            set
            { m_OfficeName = value; }
        }

        /// <summary>
        /// URL сервиса статусов пользователей офиса.
        /// </summary>
        public string StatusesServiceURL
        {
            get { return m_StatusesServiceURL; }
            set { m_StatusesServiceURL = value; }
        }

        /// <summary>
        /// Name of user to access to Web service.
        /// </summary>
        public string StatusesServiceUserName
        {
            get { return m_StatusesServiceUserName; }
            set { m_StatusesServiceUserName = value; }
        }

        /// <summary>
        /// Password of user to access to Web service.
        /// </summary>
        public string StatusesServicePassword
        {
            get { return m_StatusesServicePassword; }
            set { m_StatusesServicePassword = value; }
        }

        /// <summary>
        /// Meteo Informer's string.
        /// </summary>
        public string MeteoInformer
        {
            get { return m_MeteoInformer; }
            set { m_MeteoInformer = value; }
        }

        /// <summary>
        /// Clock Informer's string.
        /// </summary>
        public string ClockInformer
        {
            get { return m_ClockInformer; }
            set { m_ClockInformer = value; }
        }

        /// <summary>
        /// Clock Informer's string.
        /// </summary>
        public string DigitalClockInformer
        {
            get { return m_DigitalClockInformer; }
            set { m_DigitalClockInformer = value; }
        }

        #endregion

        #region Конструкторы

        public XMLSerializableOffice()
        {
        }

        public XMLSerializableOffice(Office of)
        {
            if ((of == null) || (!of.ID.HasValue))
                throw new ArgumentNullException("of");

            this.Name = of.OfficeName;
            this.MeteoInformer = of.MeteoInformer;
            this.StatusesServicePassword = of.StatusesServicePassword;
            this.StatusesServiceURL = of.StatusesServiceURL;
            this.StatusesServiceUserName = of.StatusesServiceUserName;
            this.ClockInformer = of.ClockInformer;
            this.DigitalClockInformer = of.DigitalClockInformer;
        }
        #endregion
    }
}