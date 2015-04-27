
using System.Configuration;

namespace ConfirmIt.PortalLib.Configuration
{
	/// <summary>
	/// Configuration section of Portal library.
	/// </summary>
	public class PortalSection : ConfigurationSection
	{
		/// <summary>
		/// Name of default connection string.
		/// </summary>
		[ConfigurationProperty( "defaultConnectionStringName", DefaultValue = "DBConnStr" )]
		public string DefaultConnectionStringName
		{
			get { return (string) base[ "defaultConnectionStringName" ]; }
			set { base[ "defaultConnectionStringName" ] = value; }
		}

        /// <summary>
        /// Is caches enabled by default.
        /// </summary>
        [ConfigurationProperty("defaultCacheEnabled", DefaultValue = "false")]
        public bool DefaultCacheEnabled
        {
            get { return (bool)base["defaultCacheEnabled"]; }
            set { base["defaultCacheEnabled"] = value; }
        }

		/// <summary>
		/// Settings of work calendar.
		/// </summary>
		[ConfigurationProperty( "calendar", IsRequired = true )]
		public CalendarSection Calendar
		{
			get { return (CalendarSection) base[ "calendar" ]; }
		}

		/// <summary>
		/// Settings of roles system.
		/// </summary>
		[ConfigurationProperty( "roles", IsRequired = true )]
		public RolesSection Roles
		{
			get { return (RolesSection) base[ "roles" ]; }
		}

        // <summary>
        /// Settings of objects system.
		/// </summary>
        [ConfigurationProperty("request_objects", IsRequired = true)]
		public ObjectsSection RequestObjects
		{
            get { return (ObjectsSection)base["request_objects"]; }
		}

		/// <summary>
        /// Settings of ability system.
        /// </summary>
        [ConfigurationProperty("ability", IsRequired = true)]
        public AblitySection Ability
        {
            get { return (AblitySection)base["ability"]; }
        }

        /// <summary>
        /// Settings of project system.
        /// </summary>
        [ConfigurationProperty("project", IsRequired = true)]
        public ProjectSection Project
        {
            get { return (ProjectSection)base["project"]; }
        }

        /// <summary>
		/// Settings of offices system.
		/// </summary>
		[ConfigurationProperty( "offices", IsRequired = true )]
		public OfficesSection Offices
		{
			get { return (OfficesSection) base[ "offices" ]; }
		}

		/// <summary>
		/// Settings of work time system.
		/// </summary>
		[ConfigurationProperty( "workTime", IsRequired = true )]
		public WorkTimeSection WorkTime
		{
			get { return (WorkTimeSection) base[ "workTime" ]; }
		}

        /// <summary>
        /// Settings of event system.
        /// </summary>
        [ConfigurationProperty("events", IsRequired = true)]
        public EventSection Events
        {
            get { return (EventSection)base["events"]; }
        }

        /// <summary>
        /// Settings of user system.
        /// </summary>
        [ConfigurationProperty("user", IsRequired = true)]
        public UserSection Users
        {
            get { return (UserSection)base["user"]; }
        }

        /// <summary>
        /// Main settings of system.
        /// </summary>
        [ConfigurationProperty("settings", IsRequired = true)]
        public SettingsSection GlobalSettings
        {
            get { return (SettingsSection)base["settings"]; }
        }
	}
}
