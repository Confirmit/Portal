using System;
using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
	/// <summary>
	/// Configuration element of work calendar.
	/// </summary>
	public class CalendarSection : ConfigurationElement
	{
		/// <summary>
		/// Name of connection string for this section.
		/// </summary>
		[ConfigurationProperty( "connectionStringName" )]
		public string ConnectionStringName
		{
			[DebuggerStepThrough]
			get { return (string) base[ "connectionStringName" ]; }
			[DebuggerStepThrough]
			set { base[ "connectionStringName" ] = value; }
		}

		/// <summary>
		/// Connection string.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				string connStringName =
					( string.IsNullOrEmpty( this.ConnectionStringName ) ) ?
					Globals.Settings.DefaultConnectionStringName :
					this.ConnectionStringName;

				return ConfigurationManager.ConnectionStrings[ connStringName ].ConnectionString;
			}
		}

        /// <summary>
        /// Is calendar cache enebled.
        /// </summary>
        [ConfigurationProperty("cacheEnabled", IsRequired = false)]
        protected virtual bool? ProtectedCacheEnabled
        {
            [DebuggerStepThrough]
            get { return (bool?)base["cacheEnabled"]; }
            [DebuggerStepThrough]
            set { base["cacheEnabled"] = value; }
        }

        /// <summary>
        /// Is calendar cache enabled.
        /// </summary>
        public bool CacheEnabled
        {
            get
            {
                if (ProtectedCacheEnabled == null)
                    return Globals.Settings.DefaultCacheEnabled;
                else
                    return ProtectedCacheEnabled.Value;
            }
            [DebuggerStepThrough]
            set { ProtectedCacheEnabled = value; }
        }

		/// <summary>
		/// Name of provider type.
		/// </summary>
		[ConfigurationProperty( "providerType", DefaultValue = "ConfirmIt.PortalLib.DAL.SqlClient.SqlCalendarProvider" )]
		public string ProviderType
		{
			[DebuggerStepThrough]
			get { return (string) base[ "providerType" ]; }
			[DebuggerStepThrough]
			set { base[ "providerType" ] = value; }
		}
	}
}
