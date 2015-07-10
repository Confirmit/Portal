using System.Configuration;
using Core.DB;

namespace IntegrationTestRules
{
    public static class Manager
    {
        public static void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString; ;
        }

        private static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

    }
}