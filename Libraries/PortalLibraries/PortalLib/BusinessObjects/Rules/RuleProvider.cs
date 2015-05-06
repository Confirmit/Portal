using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;
using Core;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public class RuleProvider<T> : IRuleProvider<T> where T : BasePlainObject, IRule, new()
    {
        private List<T> _rules;

        public RuleProvider()
        {
            ResolveConnection();
        }

        public List<T> GetRules()
        {
            if (_rules.Count != 0) return _rules;
            var typeOfRule = new T().GetRuleType();

            foreach (var id in GetIdRules(typeOfRule))
            {
                var rule = new T();
                if (rule.Load(id))
                {
                    _rules.Add(rule);
                }
            }
            return _rules;
        }
        private List<int> GetIdRules(RuleKind typeOfRule)
        {
            var rules = new List<int>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "Select ID from Rules where IdType = @IdType";
                command.Parameters.AddWithValue("@IdType", (int)typeOfRule);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rules.Add((int)reader["ID"]);
                    }
                }
                connection.Close();
            }
            return rules;
        }


        private const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        private void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        private ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
    }
}
