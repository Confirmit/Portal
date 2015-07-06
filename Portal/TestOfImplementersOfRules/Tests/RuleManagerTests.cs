using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;
using TestOfImplementersOfRules.Helpers;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleManagerTests
    {
        public RuleManager ruleManager;
        public IRuleInstanceRepository RuleInstanceRepository;

        public void InitializeComponents(int countGroups, int countUsers, IEnumerable<Rule> rules)
        {
            var ruleRepository = new TestRuleRepository(new TestGroupRepository());
            
            new UserGroupFiller().FillGroupRepository(ruleRepository.GroupRepository, countGroups, countUsers);
            new RuleFiller().FillRuleRepository(ruleRepository, rules);

            var filterFactory = new RuleFilterFactory().GetFullFilter();
            RuleInstanceRepository = new TestRuleInstanceRepository(ruleRepository);
            ruleManager = new RuleManager(RuleInstanceRepository, filterFactory);
        }


        [TestMethod]
        public void RuleManager_GetFiltredRules_SavedRuleInstancesOfNotifyByTimeShouldBeFive()
        {
            const int countRules = 5;
            const int countGroups = 5;
            const int countUsers = 5;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetDefaultTimeEntity);
            var createdRules = ruleFactory.GetNotifyByTimeRules(timeEntities);

            InitializeComponents(countGroups, countUsers, createdRules);

            ruleManager.GenerareSchedule();

            var filteredRules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(countRules,filteredRules.Count);
            Assert.AreEqual(countRules, RuleInstanceRepository.GetWaitedRuleInstances().Count);
        }

        [TestMethod]
        public void RuleManager_GetFiltredRules_SavedRuleInstancesOfNotifyLastUserShouldBeZero()
        {
            const int countRules = 5;
            const int countGroups = 5;
            const int countUsers = 5;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetExpiredTimeEntity);
            var createdRules = ruleFactory.GetNotifyLastUserRules(timeEntities);

            InitializeComponents(countGroups, countUsers, createdRules);

            ruleManager.GenerareSchedule();

            var filteredRules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(0, filteredRules.Count);
            Assert.AreEqual(0,RuleInstanceRepository.GetWaitedRuleInstances().Count);
        }

        [TestMethod]
        public void RuleManager_GetFilteredRules_SavedRuleInstancesWithLongExpirationTimeShouldBeFifty()
        {
            const int countRules = 5;
            const int countGroups = 5;
            const int countUsers = 5;
            const int countDays = 10;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetLongExpirationTimeEntity);
            var createdRules = ruleFactory.GetNotifyByTimeRules(timeEntities);
            InitializeComponents(countGroups, countUsers, createdRules);

            new RuleInstanceRepositoryFiller().FillRuleInstanceRepository(RuleInstanceRepository, createdRules, DateTime.Now.AddDays((-1)*countDays));
            ruleManager.GenerareSchedule();

            var filteredRules = ruleManager.GetFilteredRules(DateTime.Now.AddDays(1).AddMilliseconds(-1));
            Assert.AreEqual(countRules*countDays, filteredRules.Count);
            Assert.AreEqual(countRules * countDays, RuleInstanceRepository.GetWaitedRuleInstances().Count);
        }
    }
}