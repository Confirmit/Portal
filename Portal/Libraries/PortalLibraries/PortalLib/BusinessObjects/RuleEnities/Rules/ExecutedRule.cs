using System;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("ExecutedRules")]
    public class ExecutedRule : BasePlainObject
    {
        private DateTime _date;
        private int _ruleId;

        public ExecutedRule()
        {
            
        }

        public ExecutedRule(int ruleId, DateTime date)
        {
            _date = date;
            _ruleId = ruleId;
        }

        [DBRead("Date")]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        [DBRead("RuleId")]
        public int RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }
    }
}