using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    [DBTable("AdditionWorkTime")]
    public class AdditionRuleWorkTime : Rule
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _dayOfWeek;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan _interval;
        
        [DBRead("DayOfWeek")]
        public string DayOfWeek
        {
            [DebuggerStepThrough]
            get { return _dayOfWeek; }
            [DebuggerStepThrough]
            set { _dayOfWeek = value; }
        }

        [DBRead("Interval")]
        public TimeSpan Interval
        {
            [DebuggerStepThrough]
            get { return _interval; }
            [DebuggerStepThrough]
            set { _interval = value; }
        }

        public override int IdRule
        {
            get { return 3; }
        }

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek)
        {
            _interval = interval;
            _dayOfWeek = dayOfWeek;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek, List<int> groupsId)
            : this(interval, dayOfWeek)
        {
            GroupsId = new List<int>(groupsId);
        }
    }
}
