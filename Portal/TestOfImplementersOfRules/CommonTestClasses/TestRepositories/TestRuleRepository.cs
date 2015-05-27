using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using ConfirmIt.PortalLib.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestRuleRepository<T> : IRuleRepository<T> where T : Rule, new()
    {
        private readonly IGroupRepository _groupRepository;

        public TestRuleRepository(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        
        Dictionary<T,List<int>> _rules = new Dictionary<T, List<int>>();

        public IList<T> GetAllRules()
        {
            return _rules.Keys.ToList();
        }

        public void SaveRule(T rule)
        {
            if(_rules.ContainsKey(rule)) return;

            _rules.Add(rule, new List<int>());
        }

        public void DeleteRule(int ruleId)
        {
            foreach (var rule in _rules.Keys)
            {
                if (rule.ID.Value == ruleId) 
                    _rules.Remove(rule);
            }
        }

        public T GetRuleById(int ruleId)
        {
            return _rules.First(rule => rule.Key.ID.Value == ruleId).Key;
        }

        public void AddGroupIdsToRule(int ruleId, params int[] groupIds)
        {
            var rule = GetRuleById(ruleId);
            _rules[rule].AddRange(groupIds);
        }

        public void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds)
        {
            var rule = GetRuleById(ruleId);
            _rules[rule].RemoveAll(groupId => groupIds.Contains(groupId));
        }

        public IList<UserGroup> GetAllGroupsByRule(int ruleId)
        {
            var rule = GetRuleById(ruleId);
            return _rules[rule].Select(_groupRepository.GetGroupById).ToList();
        }

        public bool IsUserExistsInRule(int ruleId, int userId)
        {
            var groups = GetAllGroupsByRule(ruleId);
            foreach (var group in groups)
            {
                if (_groupRepository.GetAllUserIdsByGroup(group.ID.Value).Contains(userId))
                    return true;
            }
            return false;
        }

        public IList<Person> GetAllUsersByRule(int ruleId)
        {
            var userIds = new HashSet<int>();
            var groups = GetAllGroupsByRule(ruleId);
            foreach (var group in groups)
            {
                userIds.UnionWith(_groupRepository.GetAllUserIdsByGroup(group.ID.Value));
            }
            return userIds.Select(userId => new Person() {ID = userId}).ToList();
        }
    }
}