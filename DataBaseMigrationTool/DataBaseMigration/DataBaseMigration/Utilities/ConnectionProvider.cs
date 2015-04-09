using System;
using System.Configuration;
using Core.DB;

namespace DataBaseMigration.Utilities
{
    public class ConnectionProvider
    {
        private static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

        public void Connect()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = //GetConnectionString();
                @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True;User ID=portal;Password=Ubghjldbufntkm911;Packet Size=4096;";//GetConnectionString();
        }

        private string GetConnectionString()
        {
            Configuration config = null;
            string exeConfigPath = this.GetType().Assembly.Location;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            }
            catch (Exception ex)
            {
                //handle errror here.. means DLL has no sattelite configuration file.
            }

            if (config != null)
            {
                return GetAppSetting(config, "DBConnStr");
            }

            return null;
        }

        private static string GetAppSetting(Configuration config, string key)
        {
           return config.ConnectionStrings.ConnectionStrings[key].ToString();
        }
    }
}
