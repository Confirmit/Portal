using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules.NotReportingToMoscow
{
    public class TestRule : IRule
    {
        public void AddGroupId(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveGroupId(int id)
        {
            throw new NotImplementedException();
        }

        public List<IUserGroup> GetUserGroups()
        {
            throw new NotImplementedException();
        }

        public bool Contains(int userId)
        {
            throw new NotImplementedException();
        }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int IdType { get; private set; }
        public RuleKind GetRuleType()
        {
            return RuleKind.NotReportingToMoscow;
        }

        public string XmlInformation { get; set; }
    }
}