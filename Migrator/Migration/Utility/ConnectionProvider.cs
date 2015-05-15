using Core.DB;

namespace Migration.Utility
{
    public class ConnectionProvider
    {
        private ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

        public void Connect(string connectionString)
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = connectionString;
        }
    }
}
