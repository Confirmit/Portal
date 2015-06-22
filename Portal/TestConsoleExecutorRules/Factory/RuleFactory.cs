using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestConsoleExecutorRules.Factory
{
    public class RuleFactory
    {
        public DateTime Time = DateTime.Now;

        public HashSet<DayOfWeek> Days { get; set; }

        public List<NotifyByTimeRule> GetNotifyByTimeRules()
        {
            var result = new List<NotifyByTimeRule>
            {
                new NotifyByTimeRule("NotifyByTimeRuleSubject1", "information1ByNotifyByTimeRule", new TimeEntity(new TimeSpan(1,0,0), Time, Days)),
                new NotifyByTimeRule("NotifyByTimeRuleSubject2", "informationByNotifyByTimeRule", new TimeEntity(new TimeSpan(1,0,0), Time, Days)),
                new NotifyByTimeRule("NotifyByTimeRuleSubject3", "informationByNotifyByTimeRule", new TimeEntity(new TimeSpan(1,0,0), Time, Days))
            };

            return result;
        }

        public List<NotReportToMoscowRule> GetNotReportToMoscowRules()
        {
            var result = new List<NotReportToMoscowRule>
            {
                new NotReportToMoscowRule(new TimeEntity(new TimeSpan(0), DateTime.Now, Days))
            };

            return result;
        }

        public List<NotifyLastUserRule> GetNotifyLastUserRules()
        {
            var result = new List<NotifyLastUserRule>
            {
                new NotifyLastUserRule("NotifyLastUserRuleSubject1", new TimeEntity(new TimeSpan(1,0,0), Time, Days)),
                new NotifyLastUserRule("NotifyLastUserRuleSubject2",new TimeEntity(new TimeSpan(1,0,0), Time, Days)),
                new NotifyLastUserRule("NotifyLastUserRuleSubject3",new TimeEntity(new TimeSpan(1,0,0), Time, Days))
            };

            return result;
        }

    }
}