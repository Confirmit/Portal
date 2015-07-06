using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;
using TestOfImplementersOfRules.Helpers;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleExectuterTests
    {
        public IRuleInstanceRepository RuleInstanceRepository;

        public void InitializeComponents(int countGroups, int countUsers, IEnumerable<Rule> rules)
        {
            var ruleRepository = new TestRuleRepository(new TestGroupRepository());

            new UserGroupFiller().FillGroupRepository(ruleRepository.GroupRepository, countGroups, countUsers);
            new RuleFiller().FillRuleRepository(ruleRepository, rules);
            
            RuleInstanceRepository = new TestRuleInstanceRepository(ruleRepository);
        }

        [TestMethod]
        public void NotifyLastUserExecutor_ExecuteRule_ResultShouldBeTrueWithNotifyLastUserRule()
        {
            const int countRules = 5;
            const int countGroups = 5;
            const int countUsers = 5;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetDefaultTimeEntity);
            var createdRules = ruleFactory.GetNotifyLastUserRules(timeEntities);

            InitializeComponents(countGroups, countUsers, createdRules);

            var lastUserId = new PersonFactory().GetPersons(1).First().ID.Value;
            var dictionaryUserIdWorkEventType = new Dictionary<int, WorkEventType> { {lastUserId, WorkEventType.TimeOff} };

            var workEventTypeRecognizer = new TestWorkEventTypeRecognizer(dictionaryUserIdWorkEventType);

            var messageHelper = new MessageHelper();

            var notifyLastUserExecutor = new NotifyLastUserExecutor(workEventTypeRecognizer, RuleInstanceRepository,
                messageHelper, lastUserId);

            var rule = RuleInstanceRepository.RuleRepository.GetAllRulesByType<NotifyLastUserRule>().First();
            var result = notifyLastUserExecutor.ExecuteRule(rule, new RuleInstance(rule));
            var subject = rule.Subject;

            Assert.IsTrue(result);
            Assert.AreEqual(string.Format("{0}) {1}{2}", 1, subject, Environment.NewLine), messageHelper.Body);
        }

        [TestMethod]
        public void NotifyLastUserExecutor_ExecuteRule_ResultShouldBeFalseWithNotifyLastUserRule()
        {
            const int countRules = 5;
            const int countGroups = 5;
            const int countUsers = 5;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetDefaultTimeEntity);
            var createdRules = ruleFactory.GetNotifyLastUserRules(timeEntities);

            InitializeComponents(countGroups, countUsers, createdRules);

            var lastUserId = new PersonFactory().GetPersons(1).First().ID.Value;
            var dictionaryUserIdWorkEventType = new Dictionary<int, WorkEventType>();

            var workEventTypeRecognizer = new TestWorkEventTypeRecognizer(dictionaryUserIdWorkEventType);

            var messageHelper = new MessageHelper();

            var notifyLastUserExecutor = new NotifyLastUserExecutor(workEventTypeRecognizer, RuleInstanceRepository,
                messageHelper, lastUserId);

            var rule = RuleInstanceRepository.RuleRepository.GetAllRulesByType<NotifyLastUserRule>().First();
            var result = notifyLastUserExecutor.ExecuteRule(rule, new RuleInstance(rule));

            Assert.IsTrue(result);
            Assert.AreEqual("", messageHelper.Body);
        }
    }
}