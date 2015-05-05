using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using TestOfImplementersOfRules.NotReportingToMoscow;

namespace TestOfImplementersOfRules
{
    public class TestNotificationLastUser : TestRule, INotificationLastUser
    {
        public TestNotificationLastUser(List<IUserGroup> userGroups) : base(userGroups)
        {

        }

        public string Subject
        {
            get { return "TestSubject"; }
            set { }
        }
    }
}