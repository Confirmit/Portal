using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
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
        public ITimeEntityFactory timeEntityFactor= new DefaultTimeEntityFactory();

        
        public void InitializeComponents()
        {
            var ruleRepository = new RuleRepositoryFactory(new GroupRepositoryFactory(), timeEntityFactor).GetRuleRepository();
            var filterFactory = new DefaultRuleFilterFactory().GetFilter();

            ruleManager = new RuleManager(new TestRuleInstanceRepository(ruleRepository),  filterFactory);
        }


        [TestMethod]
        public void RuleManager_GetFiltredRules_SavedRuleInstancesShouldBeFive()
        {
            InitializeComponents();

            ruleManager.SaveValidRuleInstances();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 5);
        }

        [TestMethod]
        public void RuleManager_GetFiltredRules_NotActualRulesShouldNotBeGetted()
        {
            InitializeComponents();

            ruleManager.SaveValidRuleInstances();
            var rules = ruleManager.GetFilteredRules(DateTime.Now.AddDays(30));
            Assert.AreEqual(rules.Count, 0);
        }

        [TestMethod]
        public void RuleManager_GetFiltredRules_RuleInstanceWithExpiredTime()
        {
            InitializeComponents();
            timeEntityFactor = new ExpiredTimeEntityFactory();

            ruleManager.SaveValidRuleInstances();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 0);
        }
    }
}