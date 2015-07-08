using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.CommonTestClasses;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;
using TestOfImplementersOfRules.Helpers;
using TestSendingNotRegisterUsers.Test_classes;
using UlterSystems.PortalLib.Notification;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class RuleExecutorTests
    {
        public IRuleInstanceRepository RuleInstanceRepository;

        public void InitializeComponents(int countGroups, int countUsers, IEnumerable<Rule> rules)
        {
            var ruleRepository = new TestRuleRepository(new TestGroupRepository());

            new UserGroupFiller().FillGroupRepository(ruleRepository.GroupRepository, countGroups, countUsers);
            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, rules);
            
            RuleInstanceRepository = new TestRuleInstanceRepository(ruleRepository);
        }

        [TestMethod]
        public void NotifyLastUserExecutor_ExecuteRule_ResultShouldBeTrue()
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
        public void NotifyLastUserExecutor_ExecuteRule_ResultShouldBeFalse()
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
        
        [TestMethod]
        public void NotifyByTimeExecuter_ExecuteRule_ResultSholdBeTrue()
        {
            const int countRules = 1;
            const int countGroups = 1;
            const int countUsers = 1;

            var ruleFactory = new RuleFactory();
            var timeEntityFactory = new TimeEntityFactory();

            var timeEntities = timeEntityFactory.GetTimeEntities(countRules, timeEntityFactory.GetDefaultTimeEntity);
            var createdRules = ruleFactory.GetNotifyByTimeRules(timeEntities);

            InitializeComponents(countGroups, countUsers, createdRules);

            var testMailStorage = new TestMailStorage();
            var mailProvider = new MailProvider("FromAddress", MailTypes.NRNotification, testMailStorage);
            var notifyByTimeExecutor = new NotifyByTimeRuleExecutor(mailProvider, RuleInstanceRepository);

            var rule = RuleInstanceRepository.RuleRepository.GetAllRulesByType<NotifyByTimeRule>().First();
            var result = notifyByTimeExecutor.ExecuteRule(rule, new RuleInstance(rule));

            var user = RuleInstanceRepository.RuleRepository.GetAllUsersByRule(rule.ID.Value).Single();

            var mail = new MailItem
            {
                Body = rule.Information,
                Subject = rule.Subject,
                FromAddress = "FromAddress",
                ToAddress = user.PrimaryEMail
            };
            Assert.IsTrue(result);
            Assert.IsTrue(testMailStorage.IsSave);
            Assert.AreEqual(1, testMailStorage.CountSavingLetters);
            Assert.IsTrue(AreEqual(mail, testMailStorage.GetMails(false).Single()));
        }

        private bool AreEqual(MailItem mail1, MailItem mail2)
        {
            return mail1.Body == mail2.Body &&
                   mail1.Subject == mail2.Subject &&
                   mail1.FromAddress == mail2.FromAddress &&
                   mail1.ToAddress == mail2.ToAddress;
        }
    }
}