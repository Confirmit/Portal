using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public interface IExecutedRuleRepository
    {
        IList<int> GetExecutedRuleIds(DateTime beginTime, DateTime endTime);
        void SaveExecutedRule(ExecutedRule rule);
    }
}