using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.Rules;
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
        public static Visitor visitor;


        public static void InitialyzeRuleProcessor()
        { 
            ruleRepository = new RuleRepository(groupRepository);
            mainFactory = new MainFactory();
            NotifyLastUserExecutor = new NotifyLastUserExecutor(ruleRepository, new TestActivityRuleChecking(true), new TestWorkEventTypeRecognizer(WorkEventType.TimeOff), new ExecutedRuleRepository());
            ReportComposerToMoscowExecutor = new ReportComposerToMoscowExecutor(ruleRepository, new ExecutedRuleRepository(), GeneralStream, DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-4));
            NotifyByTimeRuleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, mainFactory.GetMailProvider(), mainFactory.GeTimeExecutedRulesInspector(), mainFactory.GetExecutedRuleRepository());
            visitor = new Visitor(null, NotifyByTimeRuleExecutor, NotifyLastUserExecutor, ReportComposerToMoscowExecutor);
            ruleProcessor = new RuleProcessor(visitor);
        }

        public static void NotifyLastUserRuleTest()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotifyLastUser();
            var rules = mainFactory.GetRuleFactory().GetNotifyLastUserRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);

            var messageHelper = new MessageHelper();
            ruleProcessor.NotifyLastUserExecutor.MessageHelper = messageHelper;
            ruleProcessor.NotifyLastUserExecutor.Subject = "Don't forget something";

            var necessaryRules = ruleRepository.GetAllRulesByType<NotifyLastUserRule>();
            ruleProcessor.ExecuteRule(necessaryRules.ToArray());

            Console.WriteLine(messageHelper.Body);
        }
        
        public static void NotReportToMoscowRuleTest()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForMoscow();
            var rules = mainFactory.GetRuleFactory().GetNotReportToMoscowRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForMoscow(), ruleRepository, groupRepository);

            var necessaryRules = ruleRepository.GetAllRulesByType<NotReportToMoscowRule>();
            ruleProcessor.ExecuteRule(necessaryRules.ToArray());

            var stream = ruleProcessor.ReportComposerToMoscow.Stream;

            Console.WriteLine(stream.Length);
        }

        public static void NotifyByTimeRulesTest()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotfyByTime();
            var rules = mainFactory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);
            var necesaryRules = ruleRepository.GetAllRulesByType<NotifyByTimeRule>();
            ruleProcessor.ExecuteRule(necesaryRules.ToArray());
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

            Console.ReadKey();
        }
    }
}
