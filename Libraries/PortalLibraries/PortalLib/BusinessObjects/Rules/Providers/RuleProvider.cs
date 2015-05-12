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
        private List<T> _rules;

        public RuleProvider()
        {
            ResolveConnection();
        }

        public IList<T> GetAllRules()
        {
            if (_rules != null) return _rules;
            var typeOfRule = new T().GetRuleType();
            var result = BasePlainObject.GetObjectsPageWithCondition(typeof (T), new PagingArgs(0, 100, "ID", true), "IdType", (int) typeOfRule);
            if (result.TotalCount != 0)
            {
                _rules = ((IEnumerable<T>)result.Result).ToList();
            }
            else
            {
                _rules = new List<T>();
            }
            return _rules.ToList();
        }

        public void SaveRule(T rule)
        {
            rule.Save();
        }

        public void DeleteRule(int ruleId)
        {
            GetRuleById(ruleId).Delete();
        }

        public T GetRuleById(int ruleId)
        {
            T instance = new T();
            instance.Load(ruleId);
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
