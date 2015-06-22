using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public abstract class RuleExecutor<T> where T : Rule
    {
        private readonly IRuleInstanceRepository _ruleInstanceRepository;

        protected RuleExecutor(IRuleInstanceRepository ruleInstanceRepository)
        {
            _ruleInstanceRepository = ruleInstanceRepository;
        }

        public bool ExecuteRule(T rule)
        {
            var executingRule = new RuleInstance(rule.ID.Value, DateTime.Now);
            _ruleInstanceRepository.SaveRuleInstance(executingRule);

            try
            {
                TryToExecuteRule(rule);
            }
            catch (Exception ex)
            {
                executingRule.ExceptionMessage = ex.Message;
                executingRule.Status = RuleStatus.Error;
                executingRule.EndTime = DateTime.Now;
                _ruleInstanceRepository.SaveRuleInstance(executingRule);

                return false;
            }
            executingRule.EndTime = DateTime.Now;
            executingRule.Status = RuleStatus.Success;
            _ruleInstanceRepository.SaveRuleInstance(executingRule);
            return true;
        }
       protected abstract void TryToExecuteRule(T rule);
    }
}