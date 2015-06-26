using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("RuleInstances")]
    public class RuleInstance : BasePlainObject, ITimeInformation
    {
        public RuleInstance()
        {

        }

        public RuleInstance(Rule rule)
        {
            Rule = rule;
        }

        public RuleInstance(Rule rule, DateTime launchTime)
        {
            Rule = rule;
            _status = RuleStatus.Waiting;
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
        [DBNullable]
        public DateTime? EndTime
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private DateTime? _beginDate;
        [DBRead("BeginTime")]
        [DBNullable]
        public DateTime? BeginTime
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


        [DBRead("RuleId")]
        public int RuleId
        {
            get { return Rule.ID.Value; }
            protected set { Rule.ID = value; }
        }

        private string _exceptionMessage;
        private Rule _rule;

        [DBRead("ExceptionMessage")]
        [DBNullable]
        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set { _exceptionMessage = value; }
        }

        public Rule Rule
        {
            get { return _rule; }
            protected set
            {
                if (!value.ID.HasValue)
                {
                    throw new NullReferenceException("The ID of rule doesn't exist");
                }
                _rule = value;
            }   
        }

        public TimeEntity TimeInformation
        {
            get { return Rule.TimeInformation; }
            set { Rule.TimeInformation = value; }
        }
    }
}