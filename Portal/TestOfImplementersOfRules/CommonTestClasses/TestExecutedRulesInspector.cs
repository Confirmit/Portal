using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;

namespace TestOfImplementersOfRules.CommonTestClasses
{
    public class TestExecutedRulesInspector<T> : IExecutedRulesInspector<T> where T : Rule, IDateRule
    {
        public bool Result { get; set; }

        public TestExecutedRulesInspector(bool result)
        {
            Result = result;
        }
        public bool IsExecute(T rule, DateTime beginTime, DateTime endTime)
        {
            return Result;
        }
    }
}