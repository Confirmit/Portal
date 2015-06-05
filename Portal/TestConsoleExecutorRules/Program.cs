using System;
using System.Collections.Generic;
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
        public static void NotifyLastUserRuleTest()
        {
            var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForNotifyLastUser();

            var ruleRepository = new RuleRepository(groupRepository);
            var ruleExecutor = new NotifyLastUserExecutor(ruleRepository, new TestActivityRuleChecking(true), new TestWorkEventTypeRecognizer(WorkEventType.TimeOff));
            var rules = factory.GetRuleFactory().GetNotifyLastUserRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);

            var messageHelper = new MessageHelper();
            var isSuccess= ruleExecutor.FillNotificationMessage(messageHelper, 1, "Don't forget something");
            if (isSuccess)
            {
                Console.WriteLine(messageHelper.Subject);
                Console.WriteLine(messageHelper.Body);
            }

        }
        
        public static void NotReportToMoscowRuleTest()
        {
            var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForMoscow();
           
            var ruleRepository = new RuleRepository(groupRepository);
            var ruleExecutor = new NotReportToMoscowExecutor(ruleRepository);
            var rules = factory.GetRuleFactory().GetNotReportToMoscowRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForMoscow(), ruleRepository, groupRepository);

            var users = ruleExecutor.GetUsersId();
            Array.ForEach(users.ToArray(), Console.WriteLine);
        }

        public static void NotifyByTimeRulesTest()
        {
            var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForNotfyByTime();

            var ruleRepository = new RuleRepository(groupRepository);
            var ruleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, factory.GetMailProvider(), factory.GeTimeExecutedRulesInspector(), factory.GetExecutedRuleRepository());
            var rules = factory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);

            ruleExecutor.GenerateAndSaveMails(new DateTime(2015, 6, 2), new DateTime(2015, 6, 4));
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
            var rules = new RuleRepository(new GroupRepository()).GetAllRules();

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
