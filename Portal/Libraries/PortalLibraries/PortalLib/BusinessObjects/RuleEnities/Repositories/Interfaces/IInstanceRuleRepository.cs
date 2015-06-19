using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces
{
    public interface IInstanceRuleRepository
    {
        IList<int> GetExecutedRuleIds(DateTime beginTime, DateTime endTime);
        void SaveExecutedRule(ExecutedRule rule);
    }
}