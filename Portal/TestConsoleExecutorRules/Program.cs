using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;

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
            var ruleExecutor = new NotifyLastUserExecutor(ruleRepository, new TimeActivityRuleChecking(), new DBWorkEventTypeRecognizer());
            var rules = factory.GetRuleFactory().GetNotifyLastUserRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyLastUser(), ruleRepository, groupRepository);

            var users = ruleExecutor.GetRulesForLastUser(1);
        }

        public static void NotReportToMoscowRuleTest()
        {
            var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForMoscow();
            //TODO save groups and get ids
            var ruleRepository = new RuleRepository<NotReportToMoscowRule>(groupRepository);
            var ruleExecutor = new NotReportToMoscowExecutor(ruleRepository);
            var rules = factory.GetRuleFactory().GetNotReportToMoscowRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForMoscow(), ruleRepository, groupRepository);

            var users = ruleExecutor.GetUsersId();

            //TODO проверить
        }


        public static void NotifyByTimeRulesTest()
        {
           var factory = new MainFactory();

            var groupRepository = new GroupRepository();
            var groups = factory.GetGroupFactory().GetUserGroupsForNotfyByTime();
            //TODO save groups and get ids
            
            var ruleRepository = new RuleRepository<NotifyByTimeRule>(groupRepository);
            var ruleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, factory.GetMailProvider(), factory.GeTimeExecutedRulesInspector(), factory.GetExecutedRuleRepository() );
            var rules = factory.GetRuleFactory().GetNotifyByTimeRules();

            SaveRuleGrousAndUsers(rules, groups, factory.GetUserFactory().GetUserIdForNotifyByTime(), ruleRepository, groupRepository);

            ruleExecutor.GenerateAndSaveMails(new DateTime(2015, 6, 2), new DateTime(2015, 6, 4));
        }

        private static void SaveRuleGrousAndUsers<T>(List<T> rules, List<UserGroup> groups, List<int> users, RuleRepository<T> ruleRepository, GroupRepository groupRepository) where T : Rule, new()
        {
            foreach (var rule in rules)
            {
                ruleRepository.SaveRule(rule);
                foreach (var userGroup in groups)
                {
                    ruleRepository.AddGroupIdsToRule(rule.ID.Value, userGroup.ID.Value);
                    foreach (var user in users)
                    {
                        groupRepository.AddUserIdsToGroup(user);
                    }
                }
            }
        }

        public static void Main(params string[] str)
        {
            Manager.ResolveConnection();
            NotifyByTimeRulesTest();
        }
    }
}
