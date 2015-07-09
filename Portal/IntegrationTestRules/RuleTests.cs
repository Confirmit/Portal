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
using UlterSystems.PortalService;
using Rule = ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.Rule;

namespace IntegrationTestRules
{
    [TestClass]
    public class RuleTests
    {
        [TestInitialize]
        public void Init()
        {
            Manager.ResolveConnection();
            new DataBaseHelper().RestoreDatabaseFromOriginal();
        }

        public TimeNotification GetTimeNotification(IRuleRepository ruleRepository)
        {
            var mailStorage = new DataBaseMailStorage();
            
            var ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);
            var compositeRuleFilter = new CompositeRuleFilter(new ActiveTimeFilter(), new ExperationTimeFilter(), new DayOfWeekFilter());

            var ruleManager = new RuleManager(ruleInstanceRepository, compositeRuleFilter);

            var insertTimeOffExecutor = new InsertTimeOffRuleExecutor(ruleInstanceRepository);
            var notifyByTimeExecutor = new NotifyByTimeRuleExecutor(new MailProvider("TestAddress", MailTypes.NRNotification, mailStorage), ruleInstanceRepository);

            var ruleVisitor = new RuleVisitor(insertTimeOffExecutor, notifyByTimeExecutor, null, null);
            var ruleProcessor = new RuleProcessor(ruleVisitor);

            return new TimeNotification(null, mailStorage, ruleManager, ruleProcessor);
            
        }


        [TestMethod]
        public void InsertTimeOff_()
        {
            var users = new PersonFactory().GetUsers(1);
            users.ForEach(user => user.Save());
            var groups = new UserGroupFactory().GetUserGroups(1);

            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(new GroupRepository());

            new UserGroupFiller().FillGroupRepository(groupRepository, groups, users);

            var expiration = TimeSpan.FromDays(100);
            var interval = TimeSpan.FromHours(1);
            var launchTime = DateTime.Now.AddMilliseconds(-1).TimeOfDay;
            var beginTime = DateTime.Now.AddDays(-1);
            var endTime = DateTime.Now.AddDays(100);
            var daysOfWeek = new HashSet<DayOfWeek>
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(expiration, launchTime, daysOfWeek,
                beginTime, endTime);

            var rule = new RuleFactory().GetInsertTimeOffRules(new List<TimeEntity> {timeEntity}).First();
            rule.Interval = interval;
            

            new RuleRepositoryFiller().FillRuleRepository(ruleRepository, new List<Rule>{rule});

            var timeNotification = GetTimeNotification(ruleRepository);
            timeNotification.GenerateShedule(null);


            var userId = users.First().ID.Value;
            
            WorkEventDetails details = new WorkEventDetails("", DateTime.Now.AddHours(-4),
                                                            DateTime.Now, userId,
                                                            1, 1,
                                                            10);

            new SqlWorkEventsProvider().CreateEvent(details);

            var timeDic = new SLService.SLService().GetFullDayTimes(userId);
            var oldRestTimeOnWeek = timeDic[TimeKey.WeekRest];

            timeNotification.ExecuteRules(null);

            timeDic = new SLService.SLService().GetFullDayTimes(userId);
            var newRestTimeOnWeek = timeDic[TimeKey.WeekRest];

            Assert.AreEqual(oldRestTimeOnWeek + TimeSpan.FromHours(1), newRestTimeOnWeek);
        }
    }
}
