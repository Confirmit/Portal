using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public interface IExecutedRulesInspector<T> where T : Rule, IDateRule
    {
        bool IsExecute(T rule, DateTime beginTime, DateTime endTime);
    }
}