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
    public class RuleProvider<T> : IRuleProvider<T> where T : Rule, new()
    {
        private List<T> _rules = new List<T>();

        public RuleProvider()
        {
            ResolveConnection();
        }

        public IList<T> GetAllRules()
        {
            if (_rules.Count != 0) return _rules;
            var typeOfRule = new T().GetRuleType();
            var rules = (IEnumerable<T>)BasePlainObject.GetObjectsPageWithCondition(typeof (T), new PagingArgs(0, 100, "ID", true), "IdType", (int) typeOfRule).Result;
            return rules.ToList();
        }

        public void SaveRule(T rule)
        {
            throw new NotImplementedException();
        }

        public void DeleteRule(int id)
        {
            GetRuleById(id).Delete();
        }

        public T GetRuleById(int id)
        {
            T instance = new T();
            instance.Load(id);
            return instance;
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
