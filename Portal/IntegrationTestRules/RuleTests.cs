using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Filters;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DAL.SqlClient;
using ConfirmIt.PortalLib.Notification;
using Core.DB;
using IntegrationTestRules.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLService;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;
using TestOfImplementersOfRules.Helpers;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.Notification;
using UlterSystems.PortalLib.Statistics;
using UlterSystems.PortalService;
using Rule = ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.Rule;

namespace IntegrationTestRules
{
    [TestClass]
    public class RuleTests
    {
        private RuleInstanceRepository ruleInstanceRepository;
        [TestInitialize]
        public void Init()
        {
            Manager.ResolveConnection();
            new DataBaseHelper().RestoreDatabaseFromOriginal();
        }

        public TimeNotification GetTimeNotification(IRuleRepository ruleRepository)
        {
            var mailStorage = new DataBaseMailStorage();
            
            ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);
            var compositeRuleFilter = new CompositeRuleFilter(new ActiveTimeFilter(), new ExperationTimeFilter(), new DayOfWeekFilter());

            var ruleManager = new RuleManager(ruleInstanceRepository, compositeRuleFilter);

            var insertTimeOffExecutor = new InsertTimeOffRuleExecutor(ruleInstanceRepository);
            var notifyByTimeExecutor = new NotifyByTimeRuleExecutor(new MailProvider("TestAddress", MailTypes.NRNotification, mailStorage), ruleInstanceRepository);

            var ruleVisitor = new RuleVisitor(insertTimeOffExecutor, notifyByTimeExecutor, null, null);
            var ruleProcessor = new RuleProcessor(ruleVisitor);

            return new TimeNotification(null, mailStorage, ruleManager, ruleProcessor);
            
        }

        [TestMethod]
        public void InsertTimeOff_OneUserWithMainWorkEventFourHours_RestTimeOnWeekShouldBeDecreasedOnHour()
        {
            var users = new PersonFactory().GetUsers(1);
            users.ForEach(user => user.Save());
            var groups = new UserGroupFactory().GetUserGroups(1);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(new GroupRepository());

            new UserGroupFiller().FillGroupRepository(groupRepository, groups, users);

           
            var interval = TimeSpan.FromHours(1);
            var timeEntity = new TimeEntityFactory().GetDefaultTimeEntity();

            var rule = new RuleFactory().GetInsertTimeOffRules(new List<TimeEntity> {timeEntity}).First();
            rule.Interval = interval;

            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, new List<Rule>{rule});

            var timeNotification = GetTimeNotification(ruleRepository);
            timeNotification.GenerateShedule(null);
            
            var userId = users.First().ID.Value;
            WorkEvent.CreateEvent(DateTime.Now.AddHours(-4), DateTime.Now, userId, (int)WorkEventType.MainWork);
            
            var timeDic = new SLService.SLService().GetFullDayTimes(userId);
            var oldRestTimeOnWeek = timeDic[TimeKey.WeekRest];

            timeNotification.ExecuteRules(null);

            timeDic = new SLService.SLService().GetFullDayTimes(userId);
            var newRestTimeOnWeek = timeDic[TimeKey.WeekRest];

            Assert.AreEqual(oldRestTimeOnWeek + interval, newRestTimeOnWeek);
        }

        [TestMethod]
        public void NotifyLastUser_OneUserInGroupWithNotFinishedWork_ShouldBeNotEmptyMessageHelper()
        {
            var users = new PersonFactory().GetUsers(5);
            users.ForEach(user => user.Save());
            
            var firstUserId = users.First().ID.Value;
            WorkEvent.CreateEvent(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-1), firstUserId, (int)WorkEventType.MainWork);
            
            var groups = new UserGroupFactory().GetUserGroups(1);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(new GroupRepository());

            new UserGroupFiller().FillGroupRepository(groupRepository, groups, users);

            var timeEntity = new TimeEntityFactory().GetDefaultTimeEntity();
            var rule = new RuleFactory().GetNotifyLastUserRules(new List<TimeEntity> { timeEntity }).First();

            var subject = "TestSubject";
            rule.Subject = subject;

            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, new List<Rule> { rule });

            var messageHelper = new MessageHelper();
            ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);
            var notifyLastUserExecutor = new NotifyLastUserExecutor(new DbActiveStateUserRecognizer(), ruleInstanceRepository, messageHelper, firstUserId);

            var savedRule = ruleInstanceRepository.RuleRepository.GetAllRulesByType<NotifyLastUserRule>().First();
            var ruleInstance = new RuleInstance(savedRule)
            {
                BeginTime = DateTime.Now.AddDays(-1),
                EndTime = null,
                ExpiredTime = DateTime.Now.AddDays(10),
                LaunchTime = DateTime.Now.AddMilliseconds(-1)
            };
            var result = notifyLastUserExecutor.ExecuteRule(savedRule, ruleInstance);

            Assert.IsTrue(result);
            Assert.AreEqual(string.Format("{0}) {1}{2}", 1, subject, Environment.NewLine), messageHelper.Body);
        }

        [TestMethod]
        public void NotifyByTime_OneUserAndZeroMailsInDataBase_ShouldBeOneMailInDB()
        {
            var users = new PersonFactory().GetUsers(1);
            users.ForEach(user => user.Save());
            var groups = new UserGroupFactory().GetUserGroups(1);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(new GroupRepository());

            new UserGroupFiller().FillGroupRepository(groupRepository, groups, users);

            var timeEntity = new TimeEntityFactory().GetDefaultTimeEntity();

            var rule = new RuleFactory().GetNotifyByTimeRules(new List<TimeEntity> { timeEntity }).First();

            var subject = "TestSubject";
            var information = "TestInformation";

            rule.Subject = subject;
            rule.Information = information;
            
            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, new List<Rule> { rule });

            var timeNotification = GetTimeNotification(ruleRepository);
            timeNotification.GenerateShedule(null);
            var dbMailStorage = new DataBaseMailStorage();
            var countMailsBefore = dbMailStorage.GetMails(false).Count;
            
            timeNotification.ExecuteRules(null);

            var mails = dbMailStorage.GetMails(false);
            var countMailsAfter = mails.Count;
            var person = users.First();
            var expectedMail = new MailItem {Body = information, Subject = subject, ToAddress = person.PrimaryEMail};
            var actualMail = mails.First();
            Assert.AreEqual(countMailsAfter, countMailsBefore+1);
            Assert.IsTrue(IsEqual(expectedMail, actualMail));
            
        }

        [TestMethod]
        public void NotReportToMoscow_FiveUsersInGroupAndTenUsersInDataBase_ShouldBeSequenceEqualBitsOfStreams()
        {
            var users = new PersonFactory().GetUsers(10);
            users.ForEach(user => user.Save());

            users.ForEach(user => Role.GetRole("Employee").AddUser(user.ID.Value));
            


            users.ForEach(user => WorkEvent.CreateEvent(DateTime.Now.AddHours(-1), DateTime.Now, user.ID.Value, (int)WorkEventType.MainWork));
            var groups = new UserGroupFactory().GetUserGroups(1);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(new GroupRepository());
            ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);
            new UserGroupFiller().FillGroupRepository(groupRepository, groups, users.Skip(5).ToList());

            var timeEntity = new TimeEntityFactory().GetDefaultTimeEntity();
            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).First();
            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, new List<Rule> { rule });

            var beginDate = DateTime.Now.AddDays(-10);
            var endDate = DateTime.Now;
            var notReportToMoscowExecutor = new ReportComposerToMoscowExecutor(ruleInstanceRepository, beginDate, endDate);
             var ruleInstance = new RuleInstance(rule)
            {
                BeginTime = DateTime.Now.AddDays(-1),
                EndTime = null,
                ExpiredTime = DateTime.Now.AddDays(10),
                LaunchTime = DateTime.Now.AddMilliseconds(-1)
            };

            var expectedStream = new ReportToMoscowProducer().ProduceReport(beginDate, endDate, users.Take(5).ToList());
            var expectedArray = new byte[expectedStream.Length];
            expectedStream.Read(expectedArray, 0, (int)expectedStream.Length);
            
            var success = notReportToMoscowExecutor.ExecuteRule(rule, ruleInstance);

            var actualStream = notReportToMoscowExecutor.Stream;
            var actualArray = new byte[actualStream.Length];
            actualStream.Read(actualArray, 0, (int) actualStream.Length);

            Assert.IsTrue(success);
            Assert.IsTrue(expectedArray.SequenceEqual(actualArray));
        }

        private bool IsEqual(MailItem mail1, MailItem mail2)
        {
            return
                mail1.Body == mail2.Body &&
                mail1.Subject == mail2.Subject &&
                mail1.ToAddress == mail2.ToAddress;
        }
    }
}
