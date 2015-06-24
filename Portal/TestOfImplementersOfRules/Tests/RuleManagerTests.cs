using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleManagerTests
    {
        public RuleManager ruleManager;
        public ITimeEntityFactory timeEntityFactor = new DefaultTimeEntityFactory();
        public IRuleInstanceRepository RuleInstanceRepository;

        public void InitializeComponents()
        {
            var ruleRepository = new RuleRepositoryFactory(new GroupRepositoryFactory(),
                timeEntityFactor).GetRuleRepository();

            var filterFactory = new DefaultRuleFilterFactory().GetFilter();
            RuleInstanceRepository = new TestRuleInstanceRepository(ruleRepository);
            ruleManager = new RuleManager(RuleInstanceRepository, filterFactory);
        }


        [TestMethod]
        public void RuleManager_GetFiltredRules_SavedRuleInstancesShouldBeFive()
        {
            InitializeComponents();

            ruleManager.GenerareSchedule();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 5);
        }

        [TestMethod]
        public void RuleManager_GetFiltredRules_NotActualRulesShouldNotBeGetted()
        {
            InitializeComponents();

            ruleManager.GenerareSchedule();
            var rules = ruleManager.GetFilteredRules(DateTime.Now.AddDays(30));
            Assert.AreEqual(rules.Count, 0);
        }

        [TestMethod]
        public void RuleManager_GetFiltredRules_RuleInstanceWithExpiredTime()
        {
            timeEntityFactor = new ExpiredTimeEntityFactory();
            InitializeComponents();

            ruleManager.GenerareSchedule();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 0);
        }

        [TestMethod]
        public void RuleManager_GetFilteredRules_RuleInstanceWithLongExpirationTime()
        {
           
            timeEntityFactor = new LongExpirationTimeEntityFactory();
            InitializeComponents();

            var timeEntity = timeEntityFactor.GetTimeEntities(1)[0];

            var ruleInstances = new RuleInstance(new NotifyByTimeRule() { ID = 0, TimeInformation = timeEntity }, DateTime.Now.AddDays(-10)) { };
            RuleInstanceRepository.SaveRuleInstance(ruleInstances);

            ruleManager.GenerareSchedule();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 15);
        }
    }
}