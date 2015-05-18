using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;
using Core;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public class RuleProvider<T> : IRuleProvider<T> where T : Rule, new()
    {
        public const string TableName = "AccordRules";

        private IGroupProvider _groupProvider;

        public RuleProvider(IGroupProvider groupProvider)
        {
            _groupProvider = groupProvider;
        }

        public IList<T> GetAllRules()
        {
            var typeOfRule = new T().RuleType;
            var result = BasePlainObject.GetObjectsPageWithCondition(typeof(T), new PagingArgs(0, int.MaxValue, "ID", true),
                "TypeId", (int)typeOfRule);
            var rules = new List<T>();
            if (result.TotalCount != 0)
            {
                rules = ((IEnumerable<T>)result.Result).ToList();
            }
            return rules.ToList();
        }

        public IList<UserGroup> GetAllGroupsByRule(int ruleId)
        {
            return GetGroupIdsForRule(ruleId).Select(_groupProvider.GetGroupById).ToList();
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
            command.Command.Connection.Close();

            return groupsId;
        }
        public void AddGroupIdsToRule(int ruleId, params int[] groupIds)
        {
            var groupIdsFromDataBase = GetAllGroupsByRule(ruleId).Select(item => item.ID.Value);
            var nonAddingGroups = groupIds.Except(groupIdsFromDataBase);

            if (nonAddingGroups.Count() == 0) return;

            foreach (var groupId in nonAddingGroups)
            {
                var command = new Query(string.Format("INSERT INTO {0} (RuleId, UserGroupId) VALUES  (@ruleId, @groupId)", TableName));
                command.Add("@RuleId", ruleId);
                command.Add("@GroupId", groupId);
                command.ExecNonQuery();
            }
        }

        public void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds)
        {
            var groupIdsFromDataBase = GetAllGroupsByRule(ruleId).Select(item => item.ID.Value);
            var nonDeletingGroups = groupIdsFromDataBase.Intersect(groupIds);

            if (nonDeletingGroups.Count() == 0) return;

            var groupsIdForDeleting = string.Join(",", nonDeletingGroups);

            var command = new Query(string.Format("DELETE FROM {0} WHERE RuleId = @ruleId and UserGroupId in ({1})", TableName, groupsIdForDeleting));
            command.Add("@ruleId", ruleId);
            command.ExecNonQuery();
        }

        public bool IsUserExists(int ruleId, int userId)
        {
            foreach (var group in GetAllGroupsByRule(ruleId))
            {
                if (_groupProvider.GetAllUserIdsByGroup(group.ID.Value).Contains(userId)) return true;
            }
            return false;
        }

        public void SaveRule(T rule)
        {
            rule.Save();
        }

        public void DeleteRule(int ruleId)
        {
            GetRuleById(ruleId).Delete();
        }

        public T GetRuleById(int ruleId)
        {
            T instance = new T();
            instance.Load(ruleId);
            return instance;
        }
    }
}
