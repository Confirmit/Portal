using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public abstract class RuleExecutor<T> where T : Rule
    {
        private readonly IExecutedRuleRepository _executedRuleRepository;

        public RuleExecutor(IExecutedRuleRepository executedRuleRepository)
        {
            _executedRuleRepository = executedRuleRepository;
        }

        public bool ExecuteRule(T rule)
        {
            try
            {
                TryToExecuteRule(rule);
            }
            catch (Exception ex)
            {
                return false;
            }
            _executedRuleRepository.SaveAsExecuted(rule);
            return true;
        }
       protected abstract void TryToExecuteRule(T rule);
    }
}