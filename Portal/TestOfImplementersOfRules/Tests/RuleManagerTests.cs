using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleManagerTests
    {
        public RuleManager ruleManager;

        [TestInitialize]
        public void InitializeComponents()
        {
            var ruleRepository = new RuleRepositoryFactory(new GroupRepositoryFactory()).GetRuleRepository();
            var filterFactory = new FirstRuleFilterFactory().GetFilter();

            ruleManager = new RuleManager(new TestRuleInstanceRepository(ruleRepository),  filterFactory);
        }

        [TestMethod]
        public void SavedRuleInstancesShouldBeFive()
        {
            ruleManager.SaveValidRuleInstances();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 5);
        }

        
    }
}