using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace TestConsoleExecutorRules.Factory
{
    public class RuleFactory
    {
        public DateTime Time { get; set; }

        public List<DayOfWeek> Days { get; set; }

        public List<NotifyByTimeRule> GetNotifyByTimeRules()
        {
            var result = new List<NotifyByTimeRule>
            {
                new NotifyByTimeRule("NotifyByTimeRuleSubject1", "information1ByNotifyByTimeRule", Time, Days.ToArray()),
                new NotifyByTimeRule("NotifyByTimeRuleSubject2", "informationByNotifyByTimeRule", Time, Days.ToArray()),
                new NotifyByTimeRule("NotifyByTimeRuleSubject3", "informationByNotifyByTimeRule", Time, Days.ToArray())
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
                new NotifyLastUserRule("NotifyLastUserRuleSubject1"),
                new NotifyLastUserRule("NotifyLastUserRuleSubject2"),
                new NotifyLastUserRule("NotifyLastUserRuleSubject3")
            };

            return result;
        }

    }
}