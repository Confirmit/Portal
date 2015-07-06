using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;

namespace TestOfImplementersOfRules.Factories
{
    public class RuleFactory
    {
        private int _ruleCount = 0;
        
        public List<NotifyByTimeRule> GetNotifyByTimeRules(IList<TimeEntity> timeEntities)
        {
            var rules = new List<NotifyByTimeRule>();
            for (int i = 0; i < timeEntities.Count; i++)
            {
                rules.Add(new NotifyByTimeRule("description", "TestSubject", "TestInformation", timeEntities[i]) { ID = _ruleCount++ });
            }
            return rules;
        }

        public List<NotReportToMoscowRule> GetNotReportToMoscowRules(IList<TimeEntity> timeEntities)
        {
            var rules = new List<NotReportToMoscowRule>();
            for (int i = 0; i < timeEntities.Count; i++)
            {
                rules.Add(new NotReportToMoscowRule("description", timeEntities[i]) { ID = _ruleCount++ });
            }
            return rules;
        }

        public List<NotifyLastUserRule> GetNotifyLastUserRules(IList<TimeEntity> timeEntities)
        {
            var rules = new List<NotifyLastUserRule>();
            for (int i = 0; i < timeEntities.Count; i++)
            {
                rules.Add(new NotifyLastUserRule("description", "subject", timeEntities[i]) { ID = _ruleCount++ });
            }
            return rules;
        }

        public List<InsertTimeOffRule> GetInsertTimeOffRules(IList<TimeEntity> timeEntities)
        {
            var rules = new List<InsertTimeOffRule>();
            for (int i = 0; i < timeEntities.Count; i++)
            {
                rules.Add(new InsertTimeOffRule("description", new TimeSpan(0), timeEntities[i]) { ID = _ruleCount++ });
            }
            return rules;
        }
    }
}