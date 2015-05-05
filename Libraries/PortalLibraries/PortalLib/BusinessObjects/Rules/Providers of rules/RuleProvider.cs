using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public abstract class RuleProvider
    {
        public const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        public void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

        public List<int> RulesId;

        protected RuleProvider()
        {
            FillRulesId();
        }

        protected void FillRulesId()
        {
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "Select ID from Rules where IdType = @IdType";
                command.Parameters.AddWithValue("@IdType", (int)TypeOfRule);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RulesId.Add((int)reader["ID"]);
                    }
                }
                connection.Close();
            }
        }

        public abstract RuleKind TypeOfRule { get; }
    }
}
