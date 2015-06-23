using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleManagerTests
    {
        [TestMethod]
        public void SavedRuleInstancesShouldBeOne()
        {
            var repository = new TestRuleRepository(new TestGroupRepository());
            repository.SaveRule(new NotReportToMoscowRule(){ID = 1});
            repository.SaveRule(new NotifyByTimeRule("f", "fs", new TimeEntity(new TimeSpan(0), DateTime.Now, new HashSet<DayOfWeek>() )){ID = 2});

            var rules = repository.GetAllRulesByType<NotifyByTimeRule>();

            var filter = new CompositeRuleFilter(new DayOfWeekFilter(), new ActiveTimeFilter(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(123)));
            //var ruleManager = new RuleManager(new TestRuleInstanceRepository(), new TestRuleRepository(new TestGroupRepository()), filter);
            //ruleManager.SaveValidRuleInstances();
            //var ruleEntities = ruleManager.GetFilteredRules(DateTime.Now);
            //Assert.AreEqual(ruleEntities.Count, 1);
        }
    }
}