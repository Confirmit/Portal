using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
    /// <summary>
    /// Configuration section for event system.
    /// </summary>
    public class EventSection : ConfigurationElement
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
            set { base["connectionStringName"] = value; }
        }

        /// <summary>
        /// Is events cache enebled.
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
        /// Is events cache enabled.
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

        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string connStringName =
                    (string.IsNullOrEmpty(this.ConnectionStringName)) ?
                    Globals.Settings.DefaultConnectionStringName :
                    this.ConnectionStringName;

                return ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        /// <summary>
        /// Name of provider type.
        /// </summary>
        [ConfigurationProperty("providerType", DefaultValue = "ConfirmIt.PortalLib.DAL.SqlClient.SqlEventsProvider")]
        public string ProviderType
        {
            [DebuggerStepThrough]
            get { return (string)base["providerType"]; }
            [DebuggerStepThrough]
            set { base["providerType"] = value; }
        }
    }
}
