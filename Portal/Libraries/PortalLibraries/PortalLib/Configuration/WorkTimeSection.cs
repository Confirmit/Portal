using System;
using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
    /// <summary>
    /// Configuration section for work time system.
    /// </summary>
    public class WorkTimeSection : ConfigurationElement
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
        /// Is work events cache enebled.
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
        /// Is work events cache enabled.
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
        [ConfigurationProperty("providerType", DefaultValue = "ConfirmIt.PortalLib.DAL.SqlClient.SqlWorkEventsProvider")]
        public string ProviderType
        {
            [DebuggerStepThrough]
            get { return (string)base["providerType"]; }
            [DebuggerStepThrough]
            set { base["providerType"] = value; }
        }

        /// <summary>
        /// Default work time.
        /// </summary>
        [ConfigurationProperty("defaultWorkTime", DefaultValue = "8:00")]
        public TimeSpan DefaultWorkTime
        {
            [DebuggerStepThrough]
            get { return (TimeSpan)base["defaultWorkTime"]; }
            [DebuggerStepThrough]
            set { base["defaultWorkTime"] = value; }
        }

        /// <summary>
        /// Maximum lunch time.
        /// </summary>
        [ConfigurationProperty("maxLunchTime", DefaultValue = "0:30")]
        public TimeSpan MaxLunchTime
        {
            [DebuggerStepThrough]
            get { return (TimeSpan)base["maxLunchTime"]; }
            [DebuggerStepThrough]
            set { base["maxLunchTime"] = value; }
        }
    }
}
