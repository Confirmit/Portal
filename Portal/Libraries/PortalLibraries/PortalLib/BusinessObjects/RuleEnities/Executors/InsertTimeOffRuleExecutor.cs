using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;


namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public class InsertTimeOffRuleExecutor : RuleExecutor<InsertTimeOffRule>
    {
        private readonly IRuleRepository _ruleRepository;
        

        public InsertTimeOffRuleExecutor(IRuleRepository ruleRepository,
             IInstanceRuleRepository instanceRuleRepository)
            : base(instanceRuleRepository)
        {
            _ruleRepository = ruleRepository;
            
        }

        public void InsertTimeOff(DateTime beginTime, DateTime endTime)
        {
            //var rules = _ruleRepository.GetAllRulesByType<InsertTimeOffRule>().Where(rule => 
            //    _rulesInspector.NeedToExecute(rule, beginTime, endTime)).ToList();

            //foreach (var rule in rules)
            //{
            //    TryToExecuteRule(rule);
            //}
        }

        protected override void TryToExecuteRule(InsertTimeOffRule rule)
        {
            var users = _ruleRepository.GetAllUsersByRule(rule.ID.Value);
            foreach (var user in users)
            {
                var userEvents = new UserWorkEvents(user.ID.Value);
                userEvents.AddLatestClosedWorkEvent(rule.Interval, DateTime.Now, WorkEventType.TimeOff);
            }
        }
    }
}
