using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces
{
    public interface IRuleRepository<T> where T : Rule
    {
        IList<T> GetAllRules();
        void SaveRule(T rule);
        void DeleteRule(int ruleId);
        T GetRuleById(int ruleId);
        void AddGroupIdsToRule(int groupId, params int[] userIds);
        void DeleteGroupIdsFromRule(int groupId, params int[] userIds);
        IList<UserGroup> GetAllGroupsByRule(int ruleId);
        bool IsUserExistsInRule(int ruleId, int userId);
        HashSet<int> GetAllUserIdsByRule(int ruleId);
    }
}
