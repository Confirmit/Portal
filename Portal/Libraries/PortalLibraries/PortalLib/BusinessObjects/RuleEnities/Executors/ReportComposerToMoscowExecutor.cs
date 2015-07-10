using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Statistics;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class ReportComposerToMoscowExecutor : RuleExecutor<NotReportToMoscowRule>
    {
        private readonly IRuleRepository _ruleRepository;
        
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Stream Stream { get; set; }

        public ReportComposerToMoscowExecutor(IRuleInstanceRepository ruleInstanceRepository, DateTime beginTime, DateTime endTime)
            : base(ruleInstanceRepository)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            _ruleRepository = ruleInstanceRepository.RuleRepository;
        }

        private List<int> GetUsersId()
        {
            var userIds = new HashSet<int>();

            foreach (var rule in _ruleRepository.GetAllRulesByType<NotReportToMoscowRule>())
            {
                userIds.UnionWith(_ruleRepository.GetAllUsersByRule(rule.ID.Value).Select(user => user.ID.Value));
            }
            return userIds.ToList();
        }

        protected override void TryToExecuteRule(NotReportToMoscowRule rule, RuleInstance ruleInstance)
        {
            var producer = new ReportToMoscowProducer();
            var notUsersForNotReportToMoscow = GetUsersId();
            var allUsers = UserList.GetEmployeeList();
            var employees = allUsers.Where(user => !notUsersForNotReportToMoscow.Contains(user.ID.Value));
            Stream = producer.ProduceReport(BeginTime, EndTime, employees);
        }
    }
}
