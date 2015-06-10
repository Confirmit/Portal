using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("ExecutedRules")]
    public class ExecutedRule : BasePlainObject
    {

        public ExecutedRule()
        {
            
        }

        public ExecutedRule(int ruleId, DateTime beginTime, RuleStatus ruleStatus)
        {
            _status = ruleStatus;
            _beginDate = beginTime;
            _ruleId = ruleId;
        }

        private DateTime? _endDate;

        private RuleStatus _status;

        [DBRead("Status")]
        public RuleStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [DBRead("EndTime")]
        public DateTime? EndTime
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private DateTime _beginDate;

        [DBRead("BeginTime")]
        public DateTime BeginTime
        {
            get { return _beginDate; }
            set { _beginDate = value; }
        }

        private int _ruleId;

        [DBRead("RuleId")]
        public int RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        private string _exceptionMessage;

        [DBRead("ExceptionMessage")]
        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set { _exceptionMessage = value; }
        }

    }
}