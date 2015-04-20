using System.Data.SqlClient;
using Core;
using Core.DB;

namespace ConfirmIt.PortalLib.Rules
{
    public abstract class ObjectDataBase : BasePlainObject
    {
        public abstract string NameTableAccord { get; }
        public const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        public void ResloveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
    }
}
