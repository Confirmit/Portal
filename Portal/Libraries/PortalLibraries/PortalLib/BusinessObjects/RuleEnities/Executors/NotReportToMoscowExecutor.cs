using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using UlterSystems.PortalLib.Statistics;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class NotReportToMoscowExecutor : RuleExecutor<NotReportToMoscowRule>
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        private readonly IRuleRepository _ruleRepository;
        public Stream Stream { get; set; }


        public NotReportToMoscowExecutor(IRuleRepository ruleRepository, IExecutedRuleRepository executedRuleRepository, Stream stream, DateTime beginTime, DateTime endTime)
            : base(executedRuleRepository)
        {
            Stream = stream;
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
            Stream = producer.ProduceReport(BeginTime, EndTime, GetUsersId());
        }
    }
}
