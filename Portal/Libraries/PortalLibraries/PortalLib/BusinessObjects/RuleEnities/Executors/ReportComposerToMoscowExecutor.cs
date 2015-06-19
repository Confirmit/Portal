using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
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


        public ReportComposerToMoscowExecutor(IRuleRepository ruleRepository, IInstanceRuleRepository instanceRuleRepository, DateTime beginTime, DateTime endTime)
            : base(instanceRuleRepository)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            _ruleRepository = ruleRepository;
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

        protected override void TryToExecuteRule(NotReportToMoscowRule rule)
        {
            var producer = new ReportToMoscowProducer();
            var employees = UserList.GetEmployeeList().Where(user => ! GetUsersId().Contains(user.ID.Value));
            Stream = producer.ProduceReport(BeginTime, EndTime, employees);
        }
    }
}
