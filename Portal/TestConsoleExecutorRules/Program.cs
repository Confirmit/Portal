using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
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
        public static RuleInstanceRepository ruleInstanceRepository;
        public static RuleManager ruleManager;
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
            ruleInstanceRepository = new RuleInstanceRepository(ruleRepository);

            ruleManager = new RuleManager(ruleInstanceRepository,  new FilterFactory().GetCompositeFilter());
            NotifyLastUserExecutor = new NotifyLastUserExecutor( new TestWorkEventTypeRecognizer(WorkEventType.TimeOff), new RuleInstanceRepository(ruleRepository), messageHelper, 1);
            ReportComposerToMoscowExecutor = new ReportComposerToMoscowExecutor(new RuleInstanceRepository(ruleRepository), DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-4));
            NotifyByTimeRuleExecutor = new NotifyByTimeRuleExecutor(mainFactory.GetMailProvider(), mainFactory.GetExecutedRuleRepository());
            ruleVisitor = new RuleVisitor(null, NotifyByTimeRuleExecutor, null, null);
            ruleProcessor = new RuleProcessor(ruleVisitor);
        }

        public static void SaveNotifyLastUserRules()
        {
            var subject = "Don't forget something";
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotifyLastUser();
            var rules = mainFactory.GetRuleFactory().GetNotifyLastUserRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);
        }
        
        public static void SaveNotReportToMoscowRules()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForMoscow();
            var rules = mainFactory.GetRuleFactory().GetNotReportToMoscowRules();
            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForMoscow(), ruleRepository, groupRepository);
        }

        public static void SaveNotifyByTimeRules()
        {
            var groups = mainFactory.GetGroupFactory().GetUserGroupsForNotfyByTime();
            var rules = mainFactory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, mainFactory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);
        }

        public static void TestWithFilters()
        {
            ruleManager.GenerareSchedule();

            var ruleEntities = ruleManager.GetFilteredRules(DateTime.Now);
            ruleProcessor.ExecuteRule(ruleEntities.ToArray());
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

            SaveNotReportToMoscowRules();
            SaveNotifyByTimeRules();
            
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
