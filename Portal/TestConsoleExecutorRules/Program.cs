using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Executors;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities.ExecutableChecking;
using TestConsoleExecutorRules.TestImplementation;
using UlterSystems.PortalLib.Notification;

namespace TestConsoleExecutorRules
{
    class Program
    {
        public static void NotifyByTimeRulesTest()
        {
            var factory = new RuleFactory();

            factory.Time = DateTime.Parse("3.6.2015");
            factory.Days = new List<DayOfWeek>{DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday};
                
            
            var ruleRepository = new RuleRepository<NotifyByTimeRule>(new GroupRepository());
            var ruleExecutor = new NotifyByTimeRuleExecutor(ruleRepository, new MailProvider("TestAddress@confirmit.ru", MailTypes.NRNotification, new TestMailStorage()),
                new TimeExecutedRulesInspector<NotifyByTimeRule>(new ExecutedRuleRepository()), new ExecutedRuleRepository() );

            List<NotifyByTimeRule> rules = factory.GetNotifyByTimeRules();
         
         
            foreach (var rule in rules)
            {
                ruleRepository.SaveRule(rule);
            }

            ruleExecutor.GenerateAndSaveMails(DateTime.Parse("6/2/2015"), DateTime.Parse("6/4/2015"));
        }

        public static void Main(params string[] str)
        {
            Manager.ResolveConnection();
            NotifyByTimeRulesTest();
        }
    }
}
