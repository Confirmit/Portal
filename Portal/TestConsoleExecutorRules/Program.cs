﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.Rules;
using TestConsoleExecutorRules.Factory;
using TestOfImplementersOfRules.CommonTestClasses;

namespace TestConsoleExecutorRules
{
    class Program
    {
        public static RuleProcessor ruleProcessor;
        public static Stream GeneralStream = null;
        public static MainFactory mainFactory = new MainFactory();
        public static RuleRepository ruleRepository;
        public static GroupRepository groupRepository = new GroupRepository();
        public static NotifyLastUserExecutor NotifyLastUserExecutor;
        public static ReportComposerToMoscowExecutor ReportComposerToMoscowExecutor;
        public static NotifyByTimeRuleExecutor NotifyByTimeRuleExecutor;
        public static RuleVisitor ruleVisitor;

        public static Timer timer;


        public static void InitialyzeRuleProcessor()
        {
            var subject = "Don't forget something";
            ruleRepository = new RuleRepository(groupRepository);
            mainFactory = new MainFactory();
            var messageHelper = new MessageHelper(subject);
            NotifyLastUserExecutor = new NotifyLastUserExecutor(ruleRepository, new TestWorkEventTypeRecognizer(WorkEventType.TimeOff), new InstanceRuleRepository(), messageHelper, 1);
            ReportComposerToMoscowExecutor = new ReportComposerToMoscowExecutor(ruleRepository, new InstanceRuleRepository(), DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-4));
            NotifyByTimeRuleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, mainFactory.GetMailProvider(), mainFactory.GetExecutedRuleRepository());
            ruleVisitor = new RuleVisitor(null, NotifyByTimeRuleExecutor, NotifyLastUserExecutor, ReportComposerToMoscowExecutor);
            ruleProcessor = new RuleProcessor(ruleVisitor);
        }

        public static void NotifyLastUserRuleTest()
        {
            var subject = "Don't forget something";
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotifyLastUser();
            var rules = mainFactory.GetRuleFactory().GetNotifyLastUserRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);
            //TODO для пользователя делать это, если вдруг еще будет делаться
            var messageHelper = new MessageHelper(subject);
            ruleProcessor.NotifyLastUserExecutor.MessageHelper = messageHelper;
           

            var necessaryRules = ruleRepository.GetAllRulesByType<NotifyLastUserRule>();
            ruleProcessor.ExecuteRule(necessaryRules.ToArray());

            //Console.WriteLine(messageHelper.Body);
        }
        
        public static void NotReportToMoscowRuleTest()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForMoscow();
            var rules = mainFactory.GetRuleFactory().GetNotReportToMoscowRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForMoscow(), ruleRepository, groupRepository);

            var necessaryRules = ruleRepository.GetAllRulesByType<NotReportToMoscowRule>();
            ruleProcessor.ExecuteRule(necessaryRules.ToArray());

            var stream = ruleProcessor.ReportComposerToMoscow.Stream;

            //Console.WriteLine(stream.Length);
        }

        public static void NotifyByTimeRulesTest()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotfyByTime();
            var rules = mainFactory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);
            var necesaryRules = ruleRepository.GetAllRulesByType<NotifyByTimeRule>();
            //ruleProcessor.ExecuteRule(necesaryRules.ToArray());
        }

        public static void TestWithFilters()
        {
            var allRules = ruleRepository.GetAllRules();
            var filter = new FilterFactory().GetCompositeFilter();

            var filterRules = allRules.Where(rule => filter.IsNeccessaryToExecute(rule)).ToArray();
            ruleProcessor.ExecuteRule(filterRules.ToArray());
            
        }

        private static void SaveRuleGrousAndUsers<T>(List<T> rules, List<UserGroup> groups, List<int> users, RuleRepository ruleRepository, GroupRepository groupRepository) where T : Rule, new()
        {
            foreach (var userGroup in groups)
            {
                groupRepository.SaveGroup(userGroup);
                groupRepository.AddUserIdsToGroup(userGroup.ID.Value, users.ToArray());
            }
            
            foreach (var rule in rules)
            {
                ruleRepository.SaveRule(rule);
                foreach (var userGroup in groups)
                {
                    ruleRepository.AddGroupIdsToRule(rule.ID.Value, userGroup.ID.Value);
                }
            }
        }

        public static void Main(params string[] str)
        {
            Manager.ResolveConnection();
            InitialyzeRuleProcessor();

            NotifyLastUserRuleTest();

            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");

            NotReportToMoscowRuleTest();
            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");

            NotifyByTimeRulesTest();
            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");
            Console.WriteLine("----------------------");


            StartTimer();

            //TestWithFilters();

            Console.ReadKey();
        }

        public static void StartTimer()
        {
            timer = new Timer(Callback, null, new TimeSpan(0), new TimeSpan(0,0,19995));
        }

        private static void Callback(object state)
        {
            TestWithFilters();
        }
    }
}
