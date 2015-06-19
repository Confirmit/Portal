using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository
{
    public class InstanceRuleRepository : IInstanceRuleRepository
    {
        private const string TableName = "ExecutedRules";
        public IList<int> GetExecutedRuleIds(DateTime beginTime, DateTime endTime)
        {
            var executedRules = new List<int>();

            var request = new Query(string.Format("Select ID FROM {0} " +
                                                  "WHERE BeginTime > @beginTime and EndTime < @endTime", TableName));
            request.Add("@beginTime", beginTime);
            request.Add("@endTime", endTime);

            using (var reader = request.ExecReader())
            {
                while (reader.Read())
                {
                  executedRules.Add((int) reader["ID"]);
                }
            }
            request.Destroy();
            return executedRules;
        }

        public void SaveExecutedRule(ExecutedRule rule)
        {
            rule.Save();
        }
    }
}