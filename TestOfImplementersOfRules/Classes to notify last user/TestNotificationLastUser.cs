using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules
{
    public class TestNotificationLastUser : INotificationLastUser
    {

        public string Subject
        {
            get { return "TestSubject"; }
            set { }
        }

        public void AddGroupId(int id)
        {
            
        }

        public void RemoveGroupId(int id)
        {

        }

        public List<IUserGroup> GetUserGroups()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(int userId)
        {
            throw new System.NotImplementedException();
        }

        public DateTime BeginTime
        {
            get
            {
                return DateTime.Now;
            }
            set
            {

            }
        }

        public DateTime EndTime
        {
            get
            {
                return DateTime.Now;
            }
            set
            {

            }
        }

        public int IdType
        {
            get { return (int)GetRuleType(); }
        }

        public RuleKind GetRuleType()
        {
            return RuleKind.NotificationLastUser;
        }

        public string XmlInformation
        {
            get
            {
                return string.Empty;
            }
            set
            {
                
            }
        }
    }
}