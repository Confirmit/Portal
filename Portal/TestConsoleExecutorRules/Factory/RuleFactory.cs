using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestConsoleExecutorRules.Factory
{
    public class RuleFactory
    {
        public DateTime Time = DateTime.Now;
        public TimeSpan ExpirationTime = new TimeSpan(0, 20, 20);


        public HashSet<DayOfWeek> Days { get; set; }

        public List<NotifyByTimeRule> GetNotifyByTimeRules()
        {
            var timeEntity = new TimeEntity(ExpirationTime, Time, Days, DateTime.Now.AddDays(-10),
                DateTime.Now.AddDays(10));
            var result = new List<NotifyByTimeRule>
            {
                new NotifyByTimeRule("NotifyByTimeRuleSubject1", "information1ByNotifyByTimeRule",timeEntity),
                new NotifyByTimeRule("NotifyByTimeRuleSubject2", "informationByNotifyByTimeRule", timeEntity),
                new NotifyByTimeRule("NotifyByTimeRuleSubject3", "informationByNotifyByTimeRule", timeEntity)
            };

            return result;
        }

        public List<NotReportToMoscowRule> GetNotReportToMoscowRules()
        {
            var result = new List<NotReportToMoscowRule>
            {
                new NotReportToMoscowRule(new TimeEntity(new TimeSpan(0), DateTime.Now, Days, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10)))
            };

            return result;
        }

        public List<NotifyLastUserRule> GetNotifyLastUserRules()
        {
            var timeEntity = new TimeEntity(ExpirationTime, Time, Days, DateTime.Now.AddDays(-10),
               DateTime.Now.AddDays(10));

            var result = new List<NotifyLastUserRule>
            {
                new NotifyLastUserRule("NotifyLastUserRuleSubject1", timeEntity),
                new NotifyLastUserRule("NotifyLastUserRuleSubject2", timeEntity),
                new NotifyLastUserRule("NotifyLastUserRuleSubject3", timeEntity)
            };

            return result;
        }

    }
}