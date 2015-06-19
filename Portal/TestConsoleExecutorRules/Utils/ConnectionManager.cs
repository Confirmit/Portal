using Core.DB;

namespace TestConsoleExecutorRules
{
    public static class Manager
    {
        public const string Connection = @"Data Source=CO-YAR-WS132\SQLEXPRESS;Initial Catalog=Portal;User ID=sa;Password=Stupw123!";

        public static void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        private static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
    }
}
