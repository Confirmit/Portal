using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;
using Core;
using Core.DB;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository
{
    public class RuleRepository : IRuleRepository
    {
        private const string TableName = "AccordRules";
        
        private readonly IGroupRepository _groupRepository;

        public RuleRepository(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }


        public IList<Rule> GetAllRules()
        {
            var result = BasePlainObject.GetObjects(typeof(Rule));
            var rules = ((IEnumerable<Rule>)result).ToList();
            return rules;
        }

        public IList<T> GetAllRulesByType<T>() where T : Rule, new()
        {
            var typeOfRule = new T().RuleType;
            var result = BasePlainObject.GetObjectsPageWithCondition(typeof(T), new PagingArgs(0, int.MaxValue, "ID", true),
                "TypeId", (int)typeOfRule);
            var rules = new List<T>();
            if (result.TotalCount != 0)
            {
                rules = ((IEnumerable<T>)result.Result).ToList();
            }
            rules.ForEach(rule => rule.DeserializeInstance());
            return rules;
        }

        public IList<UserGroup> GetAllGroupsByRule(int ruleId)
        {
            return GetGroupIdsForRule(ruleId).Select(_groupRepository.GetGroupById).ToList();
        }

        private IEnumerable<int> GetGroupIdsForRule(int ruleId)
        {
            var groupsId = new List<int>();

            var command = new Query(string.Format("Select UserGroupId FROM {0} WHERE RuleId = @ruleId", TableName));
            
            command.Add("@ruleId", ruleId);

            using (var reader = command.ExecReader())
            {
                while (reader.Read())
                {
                    groupsId.Add((int)reader["UserGroupId"]);
                }
            }
            command.Destroy();

            return groupsId;
        }

        public void AddGroupIdsToRule(int ruleId, params int[] groupIds)
        {
            var groupIdsFromDataBase = GetAllGroupsByRule(ruleId).Select(item => item.ID.Value);
            int[] nonAddingGroups = groupIds.Except(groupIdsFromDataBase).ToArray();

            if (!nonAddingGroups.Any()) return;

            var insertQuery = new StringBuilder();

            for(int i = 0; i < nonAddingGroups.Count(); i++)
            {
                insertQuery.Append(string.Format("INSERT INTO {0} (RuleId, UserGroupId) VALUES  (@ruleId, @{1}groupId);", TableName,i));
            }
            var query = new Query(insertQuery.ToString());
            query.Add("@ruleId", ruleId);

            for (int i = 0; i < nonAddingGroups.Count(); i++)
            {
                query.Add(string.Format("@{0}groupId", i), nonAddingGroups[i]);
            }
            
            query.ExecNonQuery();
        }

        public void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds)
        {
            var groupIdsFromDataBase = GetAllGroupsByRule(ruleId).Select(item => item.ID.Value);
            var nonDeletingGroups = groupIdsFromDataBase.Intersect(groupIds);

            if (!nonDeletingGroups.Any()) return;

            var groupsIdForDeleting = string.Join(",", nonDeletingGroups);

            var command = new Query(string.Format("DELETE FROM {0} WHERE RuleId = @ruleId and UserGroupId in ({1})", TableName, groupsIdForDeleting));
            command.Add("@ruleId", ruleId);
            command.ExecNonQuery();
        }

        public IList<Person> GetAllUsersByRule(int ruleId)
        {
            var userIds = new HashSet<int>();
            var groups = GetAllGroupsByRule(ruleId);
            foreach (var group in groups)
            {
                userIds.UnionWith(_groupRepository.GetAllUserIdsByGroup(group.ID.Value));
            }
            return userIds.Select(Person.GetPersonByID).ToList();
        }

        public bool IsUserExistedInRule(int ruleId, int userId)
        {
            foreach (var group in GetAllGroupsByRule(ruleId))
            {
                if (_groupRepository.GetAllUserIdsByGroup(group.ID.Value).Contains(userId)) return true;
            }
            return false;
        }

        public void SaveRule(Rule rule)
        {
            rule.Save();
        }

        public void DeleteRule(Rule rule)
        {
            rule.Delete();
        }

        public T GetRuleById<T>(int ruleId) where T : Rule, new()
        {
            T instance = new T();
            instance.Load(ruleId);
            return instance;
        }
    }
}
