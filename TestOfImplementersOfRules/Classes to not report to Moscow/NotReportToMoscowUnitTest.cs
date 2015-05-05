using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.Helpers;

namespace TestOfImplementersOfRules.NotReportingToMoscow
{
    [TestClass]
    public class NotReportToMoscowUnitTest
    {
        UserGroupFactory _factoryGroup = new UserGroupFactory();

        [TestMethod]
        public void AfterInvocationGetUsersId_UsersIdMustMatch()
        {
            var groups1 = new List<List<int>> {new List<int> {1, 2}, new List<int> {3, 4}};
            var groups2 = new List<List<int>> {new List<int> {1, 2}, new List<int> {5, 6}};

            var allUsers = new List<int>(); 
            foreach (var group in groups1)
            {
                allUsers = allUsers.Union(group).ToList();
            }
            foreach (var group in groups2)
            {
                allUsers = allUsers.Union(group).ToList();
            }

            var rules = new List<IRule> { new TestRule(_factoryGroup.GetUserGroups(groups1)), new TestRule(_factoryGroup.GetUserGroups(groups2))};

            var provider = new TestNotReportingToMoscowProvider(rules);
            var implementer = new NotReportedToMoscowImplementer(provider,new TestActivityRuleChecking());
            var expectedUsers = implementer.GetUsersId();
            Assert.IsTrue(expectedUsers.SequenceEqual(allUsers));
        }
    }
}