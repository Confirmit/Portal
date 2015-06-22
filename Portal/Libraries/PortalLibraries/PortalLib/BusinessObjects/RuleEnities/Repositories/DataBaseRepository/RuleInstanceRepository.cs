using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository
{
    public class RuleInstanceRepository : IRuleInstanceRepository
    {
        private readonly IRuleRepository _ruleRepository;
        private const string TableName = "RuleInstances";

        public RuleInstanceRepository(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public IList<Rule> GetWaitedRules()
        {
            var ruleInstances = new List<int>();

            var request = new Query(string.Format("Select RuleId FROM {0} WHERE Status = {1}", TableName, (int)RuleStatus.Waiting));

            using (var reader = request.ExecReader())
            {
                while (reader.Read())
                {
                    ruleInstances.Add((int)reader["RuleId"]);
                }
            }
            request.Destroy();

            return ruleInstances.Select(ruleId => _ruleRepository.GetRuleById(ruleId)).ToList();
        }

        public DateTime GetLastLaunchDateTime(int ruleId)
        {
            var request = new Query(string.Format("SELECT TOP 1 LaunchTime FROM {0} WHERE Status = {1} and RuleId = @RuleId ORDER BY LaunchTime DESC", TableName, (int)RuleStatus.Waiting));
            request.Add("@RuleId", ruleId);
            
            var result = request.ExecScalar();
            return result == null ? DateTime.Today : (DateTime) result;
        }

        public void SaveRuleInstance(RuleInstance rule)
        {
            rule.Save();
        }
    }
}