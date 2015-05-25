using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public interface IExecutedRuleProvider
    {
        IList<Rule> GetExecutedRules(DateTime beginTime, DateTime endTime);
        void SaveAsExecuted(Rule rule);
    }
}