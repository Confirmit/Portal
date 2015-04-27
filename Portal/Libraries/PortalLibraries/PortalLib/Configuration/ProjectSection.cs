using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
	/// <summary>
	/// Configuration section for project provider.
	/// </summary>   
    public class ProjectSection : ConfigurationElement
    {
        /// <summary>
        /// Name of connection string for this section.
        /// </summary>
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            [DebuggerStepThrough]
            get { return (string)base["connectionStringName"]; }
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
                    (string.IsNullOrEmpty(ConnectionStringName))
                        ? Globals.Settings.DefaultConnectionStringName
                        : ConnectionStringName;

                return ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
		}

		/// <summary>
		/// Name of provider type.
		/// </summary>
        [ConfigurationProperty("providerType", DefaultValue = "ConfirmIt.PortalLib.DAL.SqlClient.SqlProjectProvider")]
		public string ProviderType
		{
			[DebuggerStepThrough]
			get { return (string) base[ "providerType" ]; }
			[DebuggerStepThrough]
			set { base[ "providerType" ] = value; }
		}

        /// <summary>
        /// Is project cache enabled.
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
        /// Is project cache enabled.
        /// </summary>
        public bool CacheEnabled
        {
            get {
                return ProtectedCacheEnabled == null
                           ? (bool) Globals.Settings.DefaultCacheEnabled
                           : ProtectedCacheEnabled.Value;
            }
            [DebuggerStepThrough]
            set { ProtectedCacheEnabled = value; }
        }
    }
}