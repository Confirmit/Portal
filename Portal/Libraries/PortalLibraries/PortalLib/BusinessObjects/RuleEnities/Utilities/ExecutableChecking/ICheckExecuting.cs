using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking
{
    public interface ICheckExecuting<T> where T : Rule, ITimeRule
    {
        bool IsExecute(T rule, DateTime beginTime, DateTime endTime);
    }
}