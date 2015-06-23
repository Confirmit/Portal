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

        public IList<RuleEntity> GetWaitedRules()
        {
            var rulePairs = new List<KeyValuePair<int,int>>();

            var request = new Query(string.Format("Select RuleId, ID FROM {0} WHERE Status = {1}", TableName, (int)RuleStatus.Waiting));

            using (var reader = request.ExecReader())
            {
                while (reader.Read())
                {
                    var pair = new KeyValuePair<int,int>((int)reader["RuleId"], (int)reader["ID"]);
                    rulePairs.Add(pair);
                }
            }
            request.Destroy();

            var ruleEntities = new List<RuleEntity>();

            foreach (var pair in rulePairs)
            {
                var ruleInstance = new RuleInstance();
                ruleInstance.Load(pair.Value);
                var rule = _ruleRepository.GetRuleById(pair.Key);
                ruleEntities.Add(new RuleEntity(rule, ruleInstance));
            }

            return ruleEntities;
        }

        public DateTime? GetLastLaunchDateTime(int ruleId)
        {
            var request = new Query(string.Format("SELECT TOP 1 LaunchTime FROM {0} WHERE Status = {1} and RuleId = @RuleId ORDER BY LaunchTime DESC", TableName, (int)RuleStatus.Success));
            request.Add("@RuleId", ruleId);
            
            var result = request.ExecScalar();
            return (DateTime?) result;
        }

        public void SaveRuleInstance(RuleInstance rule)
        {
            rule.Save();
        }
    }
}