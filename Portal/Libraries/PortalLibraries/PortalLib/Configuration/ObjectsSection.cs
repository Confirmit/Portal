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
