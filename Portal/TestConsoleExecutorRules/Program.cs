using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;
using ConfirmIt.PortalLib.Rules;
using TestConsoleExecutorRules.TestImplementation;
using UlterSystems.PortalLib.Notification;

namespace TestConsoleExecutorRules
{
    class Program
    {
        public static void NotifyByTimeRulesTest()
        {
            var factory = new RuleFactory();

            factory.Time = new DateTime(2015, 6, 3);
            factory.Days = new List<DayOfWeek>{DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday};
                
            var groupRepository = new GroupRepository();
            groupRepository.SaveGroup(new UserGroup());
            groupRepository.SaveGroup(new UserGroup());
            groupRepository.SaveGroup(new UserGroup());
            

            groupRepository.AddUserIdsToGroup();
            var ruleRepository = new RuleRepository<NotifyByTimeRule>(groupRepository);
            var ruleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, new MailProvider("TestAddress@confirmit.ru", MailTypes.NRNotification, new TestMailStorage()),
                new TimeExecutedRulesInspector<NotifyByTimeRule>(new ExecutedRuleRepository()), new ExecutedRuleRepository() );
            List<NotifyByTimeRule> rules = factory.GetNotifyByTimeRules();
         
         
            foreach (var rule in rules)
            {
                ruleRepository.SaveRule(rule);
            }

            ruleExecutor.GenerateAndSaveMails(new DateTime(2015, 6, 2), new DateTime(2015, 6, 4));
        }

        public static void Main(params string[] str)
        {
            Manager.ResolveConnection();
            NotifyByTimeRulesTest();
        }
    }
}
