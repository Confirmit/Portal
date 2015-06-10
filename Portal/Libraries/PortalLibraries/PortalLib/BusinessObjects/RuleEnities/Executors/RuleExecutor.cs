using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public abstract class RuleExecutor<T> where T : Rule
    {
        private readonly IExecutedRuleRepository _executedRuleRepository;

        protected RuleExecutor(IExecutedRuleRepository executedRuleRepository)
        {
            _executedRuleRepository = executedRuleRepository;
        }

        public bool ExecuteRule(T rule)
        {
            var executingRule = new ExecutedRule(rule.ID.Value, DateTime.Now, RuleStatus.Processing);
            _executedRuleRepository.SaveExecutedRule(executingRule);

            try
            {
                TryToExecuteRule(rule);
            }
            catch (Exception ex)
            {
                executingRule.ExceptionMessage = ex.Message;
                executingRule.Status = RuleStatus.Error;
                executingRule.EndTime = DateTime.Now;
                _executedRuleRepository.SaveExecutedRule(executingRule);

                return false;
            }
            executingRule.EndTime = DateTime.Now;
            executingRule.Status = RuleStatus.Success;
            _executedRuleRepository.SaveExecutedRule(executingRule);
            return true;
        }
       protected abstract void TryToExecuteRule(T rule);
    }
}