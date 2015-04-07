using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
	/// <summary>
	/// Configuration section for books provider.
	/// </summary>
	public class ObjectsSection : ConfigurationElement
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
        /// Is books cache enebled.
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
        /// Is books cache enabled.
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
        [ConfigurationProperty("providerType", DefaultValue = "ConfirmIt.PortalLib.DAL.SqlClient.SqlRequestObjectsProvider")]
		public string ProviderType
		{
			[DebuggerStepThrough]
			get { return (string) base[ "providerType" ]; }
			[DebuggerStepThrough]
			set { base[ "providerType" ] = value; }
		}

		/// <summary>
		/// Base path to books location.
		/// </summary>
		[ConfigurationProperty( "downloadBasePath", DefaultValue = @"\\DBSERVER\Library" )]
		public string DownloadBasePath
		{
			[DebuggerStepThrough]
			get { return (string) base[ "downloadBasePath" ]; }
			[DebuggerStepThrough]
			set { base[ "downloadBasePath" ] = value; }
		}

		/// <summary>
		/// Base path to books location.
		/// </summary>
		[ConfigurationProperty( "booksLanguages", DefaultValue = "Русский,English" )]
		public string BooksLanguages
		{
			[DebuggerStepThrough]
			get { return (string) base[ "booksLanguages" ]; }
			[DebuggerStepThrough]
			set { base[ "booksLanguages" ] = value; }
		}

		/// <summary>
		/// Default size of page.
		/// </summary>
		[ConfigurationProperty( "defaultPageSize", DefaultValue = "10" )]
		public int DefaultPageSize
		{
			[DebuggerStepThrough]
			get { return (int) base[ "defaultPageSize" ]; }
			[DebuggerStepThrough]
			set { base[ "defaultPageSize" ] = value; }
		}
	}
}
