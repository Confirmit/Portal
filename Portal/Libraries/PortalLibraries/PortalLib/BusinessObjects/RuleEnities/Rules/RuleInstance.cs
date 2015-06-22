using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("RuleInstances")]
    public class RuleInstance : BasePlainObject
    {
        public RuleInstance()
        {
            
        }

        public RuleInstance(int ruleId, DateTime launchTime)
        {
            _status = RuleStatus.Waiting;
            _ruleId = ruleId;
            _launchTime = launchTime;
        }
        
        private RuleStatus _status;
        [DBRead("Status")]
        public RuleStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private DateTime? _endDate;
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
        
        private DateTime _launchTime;
        [DBRead("LaunchTime")]
        public DateTime LaunchTime
        {
            get { return _launchTime; }
            set { _launchTime = value; }
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