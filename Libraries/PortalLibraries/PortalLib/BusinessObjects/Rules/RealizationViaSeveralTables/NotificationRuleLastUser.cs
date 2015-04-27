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
    [DBTable("NotificationLastUser")]
    public class NotificationRuleLastUser: Rule
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _subject;
        
        [DBRead("Subject")]
        public string Subject
        {
            [DebuggerStepThrough]
            get { return _subject; }
            [DebuggerStepThrough]
            set { _subject = value; }
        }

        public override int IdRule
        {
            get { return 2; }
        }
        public NotificationRuleLastUser(string subject)
        {
            _subject = subject;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleLastUser(string subject, List<int> groupsId)
            : this(subject)
        {
            GroupsId = new List<int>(groupsId);
        }
    }
}
