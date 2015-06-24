using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using TestOfImplementersOfRules.CommonTestClasses.TestRepositories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;

namespace TestOfImplementersOfRules.Factories
{
    public class RuleRepositoryFactory
    {
        public IGroupRepositoryFactory GroupRepositoryFactory { get; set; }

        public RuleRepositoryFactory(IGroupRepositoryFactory groupRepositoryFactory)
        {
            GroupRepositoryFactory = groupRepositoryFactory;
        }


        public IRuleRepository GetRuleRepository()
        {
            var groupRepository = GroupRepositoryFactory.GetGroupRepository();
            var ruleRepository = new TestRuleRepository(groupRepository);

            var groupIds = groupRepository.GetAllGroups().Select(group => group.ID.Value).ToArray();
            var rules = new RuleFactory(new FirstTimeEntityFactory()).GetNotifyByTimeRules();
            rules.ForEach(ruleRepository.SaveRule);

            rules.ForEach(rule => ruleRepository.AddGroupIdsToRule(rule.ID.Value, groupIds));

            return ruleRepository;
        }
    }
}