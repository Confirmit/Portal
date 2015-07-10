using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestConsoleExecutorRules.Factory
{
    public class RuleFactory
    {
        public TimeSpan Time = DateTime.Now.TimeOfDay;
        public TimeSpan ExpirationTime = new TimeSpan(0, 20, 20);


        public HashSet<DayOfWeek> Days { get; set; }

        public List<NotifyByTimeRule> GetNotifyByTimeRules()
        {
            var timeEntity = new TimeEntity(ExpirationTime, Time, Days, DateTime.Now.AddDays(-10),
                DateTime.Now.AddDays(10));
            var result = new List<NotifyByTimeRule>
            {
                new NotifyByTimeRule("description","NotifyByTimeRuleSubject1", "information1ByNotifyByTimeRule",timeEntity),
                new NotifyByTimeRule("description","NotifyByTimeRuleSubject2", "informationByNotifyByTimeRule", timeEntity),
                new NotifyByTimeRule("description","NotifyByTimeRuleSubject3", "informationByNotifyByTimeRule", timeEntity)
            };

            return result;
        }

        public List<NotReportToMoscowRule> GetNotReportToMoscowRules()
        {
            var result = new List<NotReportToMoscowRule>
            {
                new NotReportToMoscowRule("description",new TimeEntity(new TimeSpan(0),Time, Days, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10)))
            };

            return result;
        }

        public List<NotifyLastUserRule> GetNotifyLastUserRules()
        {
            var timeEntity = new TimeEntity(ExpirationTime, Time, Days, DateTime.Now.AddDays(-10),
               DateTime.Now.AddDays(10));

            var result = new List<NotifyLastUserRule>
            {
                new NotifyLastUserRule("description","NotifyLastUserRuleSubject1", timeEntity),
                new NotifyLastUserRule("description","NotifyLastUserRuleSubject2", timeEntity),
                new NotifyLastUserRule("description","NotifyLastUserRuleSubject3", timeEntity)
            };

            return result;
        }

    }
}