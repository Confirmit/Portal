using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core;
using Core.DB;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    [DBTable("NotificationByTime")]
    public class NotificationRuleByTime : Rule
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _informaton = String.Empty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _time;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _dayOfWeek = String.Empty;
        
        [DBRead("Information")]
        public string Information
        {
            [DebuggerStepThrough]
            get { return _informaton; }
            [DebuggerStepThrough]
            set { _informaton = value; }
        }

        [DBRead("Time")]
        public DateTime Time
        {
            [DebuggerStepThrough]
            get { return _time; }
            [DebuggerStepThrough]
            set { _time = value; }
        }

        [DBRead("DayOfWeek")]
        public string DayOfWeek
        {
            [DebuggerStepThrough]
            get { return _dayOfWeek; }
            [DebuggerStepThrough]
            set { _dayOfWeek = value; }
        }

        public override int IdRule
        {
            get { return 1; }
        }
        
        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek, List<int> groupsId) : this(information, time,  dayOfWeek)
        {
            GroupsId = new List<int>(groupsId);
        }

        
    }
}
