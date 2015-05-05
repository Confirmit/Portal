using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules.NotReportingToMoscow
{
    public class TestRule : IRule
    {
        protected List<IUserGroup> _userGroups;

        public TestRule(List<IUserGroup> userGroups)
        {
            _userGroups = userGroups;
        }
        public List<IUserGroup> GetUserGroups()
        {
            return _userGroups;
        }

        public bool Contains(int userId)
        {
            foreach (var group in _userGroups)
            {
                if (group.GetUsersId().Contains(userId)) return true;
            }
            return false;
        }



        public void AddGroupId(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveGroupId(int id)
        {
            throw new NotImplementedException();
        }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int IdType { get; private set; }

        public RuleKind GetRuleType()
        {
            throw new NotImplementedException();
        }

        public string XmlInformation { get; set; }
    }
}