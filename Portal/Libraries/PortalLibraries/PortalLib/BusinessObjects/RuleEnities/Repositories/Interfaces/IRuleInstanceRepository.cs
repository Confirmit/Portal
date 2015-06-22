using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces
{
    public interface IRuleInstanceRepository
    {
        IList<Rule> GetWaitedRules();
        DateTime GetLastLaunchDateTime(int ruleId);
        void SaveRuleInstance(RuleInstance rule);
    }
}