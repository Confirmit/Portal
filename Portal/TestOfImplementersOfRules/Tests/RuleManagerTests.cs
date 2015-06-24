using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
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

            ruleManager = new RuleManager(new TestRuleInstanceRepository(ruleRepository), ruleRepository, filterFactory);
        }

        [TestMethod]
        public void SavedRuleInstancesShouldBeOne()
        {
            ruleManager.SaveValidRuleInstances();
            var rules = ruleManager.GetFilteredRules(DateTime.Now);
            Assert.AreEqual(rules.Count, 5);
        }
    }
}