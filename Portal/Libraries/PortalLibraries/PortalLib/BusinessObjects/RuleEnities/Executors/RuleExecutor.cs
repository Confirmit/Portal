using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors
{
    public abstract class RuleExecutor<T> where T : Rule
    {
        private readonly IRuleInstanceRepository _ruleInstanceRepository;

        protected RuleExecutor(IRuleInstanceRepository ruleInstanceRepository)
        {
            _ruleInstanceRepository = ruleInstanceRepository;
        }

        public bool ExecuteRule(T rule, RuleInstance ruleInstance)
        {
           
            ruleInstance.BeginTime = DateTime.Now;

            _ruleInstanceRepository.SaveRuleInstance(ruleInstance);
            try
            {
                TryToExecuteRule(rule);
            }
            catch (Exception ex)
            {
                ruleInstance.ExceptionMessage = ex.Message;
                ruleInstance.Status = RuleStatus.Error;
                ruleInstance.EndTime = DateTime.Now;
                _ruleInstanceRepository.SaveRuleInstance(ruleInstance);

                return false;
            }
            ruleInstance.EndTime = DateTime.Now;
            ruleInstance.Status = RuleStatus.Success;
            _ruleInstanceRepository.SaveRuleInstance(ruleInstance);
            return true;
        }
       protected abstract void TryToExecuteRule(T rule);
    }
}