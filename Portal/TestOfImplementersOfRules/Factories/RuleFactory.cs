using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestOfImplementersOfRules.Factories
{
    public class RuleFactory
    {
        private const int DefaultCount = 5;

        public ITimeEntityFactory TimeEntityFactory { get; set; }

        public RuleFactory(ITimeEntityFactory timeEntityFactory)
        {
            TimeEntityFactory = timeEntityFactory;
        }

        public List<NotifyByTimeRule> GetNotifyByTimeRules(int number = DefaultCount)
        {
            var rules = new List<NotifyByTimeRule>();
            var timeEntites = TimeEntityFactory.GetTimeEntities(number);

            for (int i = 0; i < number; i++)
            {
                rules.Add(new NotifyByTimeRule("TestSubject", "TestInformation", timeEntites[i]));
            }

            return rules;
        }
    }
}