using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace TestOfImplementersOfRules.CommonTestClasses.TestRepositories
{
    public class TestRuleRepository : IRuleRepository
    {
        private HashSet<Rule> _rules = new HashSet<Rule>();
        private IGroupRepository _groupRepository;

        private Dictionary<int, IList<int>> _groups = new Dictionary<int, IList<int>>();

        public TestRuleRepository(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IList<Rule> GetAllRules()
        {
            return _rules.ToList();
        }

        public IList<T> GetAllRulesByType<T>() where T : Rule, new()
        {
            return _rules.OfType<T>().ToList();
        }

        public void SaveRule(Rule rule)
        {
            DeleteRule(rule);
            _rules.Add(rule);
        }

        public void DeleteRule(Rule rule)
        {
            _rules.RemoveWhere(Rule => Rule.ID.Value == rule.ID.Value);
        }

        public T GetRuleById<T>(int ruleId) where T : Rule, new()
        {
            var rule = GetRuleById(ruleId) as T;
            if (rule == null) 
                throw new NullReferenceException("The rule by this type was not found");

            return rule;
        }

        public void AddGroupIdsToRule(int ruleId, params int[] groupIds)
        {
            if (_groups.ContainsKey(ruleId))
            {
                var Ids = new HashSet<int>(_groups[ruleId]);
                Ids.UnionWith(groupIds);
                _groups[ruleId] = new List<int>(Ids);
                return;
            }

            _groups.Add(ruleId, groupIds);
        }

        public void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds)
        {
            if (!_groups.ContainsKey(ruleId))
                throw new KeyNotFoundException("Id of rule was not found in the memory");

            var ids = _groups[ruleId];
            var excepteedgroupIds = ids.Except(groupIds);

            _groups[ruleId] = new List<int>(excepteedgroupIds);
        }

        public IList<UserGroup> GetAllGroupsByRule(int ruleId)
        {
            if (!_groups.ContainsKey(ruleId))
                throw new KeyNotFoundException("Id of rule was not found in the memory");

            return _groups[ruleId].Select(groupsId => new UserGroup { ID = groupsId }).ToList();
        }

        public bool IsUserExistedInRule(int ruleId, int userId)
        {
            var userIds = GetAllUserIdsByRuleId(ruleId);

            return userIds.Contains(userId);
        }

        public IList<Person> GetAllUsersByRule(int ruleId)
        {
            var userIds = GetAllUserIdsByRuleId(ruleId);

            return userIds.Select(userId => new Person {ID = userId}).ToList();
        }

        public Rule GetRuleById(int ruleId)
        {
            return _rules.Where(rule => rule.ID.Value == ruleId).Single();
        }

        private HashSet<int> GetAllUserIdsByRuleId(int ruleId)
        {
            if (!_groups.ContainsKey(ruleId))
                throw new KeyNotFoundException("Id of rule was not found in the memory");

            var groupIds = _groups[ruleId];
            var userIds = new HashSet<int>();

            foreach (var groupId in groupIds)
            {
                userIds.UnionWith(_groupRepository.GetAllUserIdsByGroup(groupId));
            }
            return userIds;
        }
    }
}