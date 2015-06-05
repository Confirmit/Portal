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
            //TODO save groups and get ids
            var ruleRepository = new RuleRepository<NotifyLastUserRule>(groupRepository);
            var ruleExecutor = new NotifyLastUserExecutor(ruleRepository, new TestActivityRuleChecking(true), new TestWorkEventTypeRecognizer(WorkEventType.TimeOff));
            var rules = factory.GetRuleFactory().GetNotifyLastUserRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);

            var rulesForLastUser = ruleExecutor.GetRulesForLastUser(1);
            if (rulesForLastUser.Count != 0)
            {
                var messageBuilder = new MessageBuilder();
                var messageHelper = new MessageHelper("Notification about last user");
                messageBuilder.BuildScript(1, "someSubject", messageHelper, rulesForLastUser);
                Console.WriteLine(messageHelper.ToString());
            }
        }
        
        public static void NotReportToMoscowRuleTest()
        {
            var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForMoscow();
           
            var ruleRepository = new RuleRepository<NotReportToMoscowRule>(groupRepository);
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

            var ruleRepository = new RuleRepository<NotifyByTimeRule>(groupRepository);
            var ruleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, factory.GetMailProvider(), factory.GeTimeExecutedRulesInspector(), factory.GetExecutedRuleRepository());
            var rules = factory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);

            ruleExecutor.GenerateAndSaveMails(new DateTime(2015, 6, 2), new DateTime(2015, 6, 4));
        }

        private static void SaveRuleGrousAndUsers<T>(List<T> rules, List<UserGroup> groups, List<int> users, RuleRepository<T> ruleRepository, GroupRepository groupRepository) where T : Rule, new()
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
            //NotifyLastUserRuleTest();
            //NotReportToMoscowRuleTest();
            //NotifyByTimeRulesTest();
            Console.ReadKey();
        }
    }
}
