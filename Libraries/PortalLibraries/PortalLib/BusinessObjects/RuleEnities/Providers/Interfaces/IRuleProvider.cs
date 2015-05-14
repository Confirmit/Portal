using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers_of_rules
{
    public interface IRuleProvider<T> where T : Rule
    {
        IList<T> GetAllRules();
        void SaveRule(T rule);
        void DeleteRule(int ruleId);
        T GetRuleById(int ruleId);
        void AddGroupIdsToRule(int groupId, params int[] userIds);
        void DeleteGroupIdsFromRule(int groupId, params int[] userIds);
        IList<UserGroup> GetAllGroupsByRule(int ruleId);
        bool IsUserExists(int ruleId, int userId);
    }
}
