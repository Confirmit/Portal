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
                new NotifyByTimeRule("Subject1", "information1", Time, Days.ToArray()),
                new NotifyByTimeRule("Subject2", "information2", Time, Days.ToArray()),
                new NotifyByTimeRule("Subject3", "information3", Time, Days.ToArray())
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
                new NotifyLastUserRule("Subject1"),
                new NotifyLastUserRule("Subject2"),
                new NotifyLastUserRule("Subject3")
            };

            return result;
        }

    }
}