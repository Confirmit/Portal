using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class InsertTimeOffRuleExecutor : RuleExecutor<InsertTimeOffRule>
    {
        private readonly IRuleRepository _ruleRepository;

        public InsertTimeOffRuleExecutor(IRuleInstanceRepository ruleInstanceRepository)
            : base(ruleInstanceRepository)
        {
            _ruleRepository = ruleInstanceRepository.RuleRepository;
            
        }

        protected override void TryToExecuteRule(InsertTimeOffRule rule, RuleInstance ruleInstance)
        {
            var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
            foreach (var user in users)
            {
                var userEvents = new UserWorkEvents(user.ID.Value);
                userEvents.AddLatestClosedWorkEvent(rule.Interval, ruleInstance.LaunchTime, WorkEventType.TimeOff);
            }
        }
    }
}
