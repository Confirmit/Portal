using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using PortalTest.Properties;

namespace PortalTest
{
	internal class Utils
	{
		private static bool isConnectionInitialized = false;

		/// <summary>
		/// Initializes DB connection.
		/// </summary>
		public static void InitDBConnection()
		{
			if( !isConnectionInitialized )
			{
				lock( typeof( Utils ) )
				{
					if( !isConnectionInitialized )
					{
						Core.DB.ConnectionManager.ConnectionTypeResolve += new Core.DB.ConnectionManager.ConnectionTypeResolveCallback( ConnectionTypeResolver );
						Core.DB.ConnectionManager.DefaultConnectionString = Settings.Default.PortalTestDatabase;
						isConnectionInitialized = true;
					}
				}
			}
		}

		private static Core.DB.ConnectionType ConnectionTypeResolver(Core.DB.ConnectionKind kind)
		{
			return Core.DB.ConnectionType.SQLServer;
		}
	}
}
