using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;

namespace TestConsoleExecutorRules.Factory
{
    public class RuleFactory
    {
        public DateTime Time { get; set; }

        public HashSet<DayOfWeek> Days { get; set; }

        public List<NotifyByTimeRule> GetNotifyByTimeRules()
        {
            var result = new List<NotifyByTimeRule>
            {
                new NotifyByTimeRule("NotifyByTimeRuleSubject1", "information1ByNotifyByTimeRule", new TimeEntity(new TimeSpan(0), Time, Days)),
                new NotifyByTimeRule("NotifyByTimeRuleSubject2", "informationByNotifyByTimeRule", new TimeEntity(new TimeSpan(0), Time, Days)),
                new NotifyByTimeRule("NotifyByTimeRuleSubject3", "informationByNotifyByTimeRule", new TimeEntity(new TimeSpan(0), Time, Days))
            };

            return result;
        }

        public List<NotReportToMoscowRule> GetNotReportToMoscowRules()
        {
            var result = new List<NotReportToMoscowRule>
            {
                new NotReportToMoscowRule()
            };

            return result;
        }

        public List<NotifyLastUserRule> GetNotifyLastUserRules()
        {
            var result = new List<NotifyLastUserRule>
            {
                new NotifyLastUserRule("NotifyLastUserRuleSubject1", new TimeEntity(new TimeSpan(0), Time, Days)),
                new NotifyLastUserRule("NotifyLastUserRuleSubject2",new TimeEntity(new TimeSpan(0), Time, Days)),
                new NotifyLastUserRule("NotifyLastUserRuleSubject3",new TimeEntity(new TimeSpan(0), Time, Days))
            };

            return result;
        }

    }
}