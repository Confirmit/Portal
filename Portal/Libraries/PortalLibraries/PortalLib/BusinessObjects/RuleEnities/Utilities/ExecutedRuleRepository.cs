using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class ExecutedRuleRepository : IExecutedRuleRepository
    {
        private const string TableName = "ExecutedRules";
        public IList<int> GetExecutedRuleIds(DateTime beginTime, DateTime endTime)
        {
            var executedRules = new List<int>();

            var request = new Query(string.Format("Select ID, RuleId, Date FROM {0} " +
                                                  "WHERE Date > @beginTime and date < @endTime", TableName));
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

        public void SaveAsExecuted(Rule rule)
        {
            new ExecutedRule(rule.ID.Value, DateTime.Now).Save();
        }
    }
}