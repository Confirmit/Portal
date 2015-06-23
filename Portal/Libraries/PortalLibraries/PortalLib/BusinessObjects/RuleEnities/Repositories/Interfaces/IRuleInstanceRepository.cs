﻿using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces
{
    public interface IRuleInstanceRepository
    {
        IList<RuleEntity> GetWaitedRuleEntities();
        DateTime? GetLastLaunchDateTime(int ruleId);
        void SaveRuleInstance(RuleInstance ruleInstance);
    }
}