using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Providers.Interfaces
{
    public interface IRuleRepository<T> where T : Rule
    {
        IList<T> GetAllRules();
        void SaveRule(T rule);
        void DeleteRule(int ruleId);
        T GetRuleById(int ruleId);
        void AddGroupIdsToRule(int ruleId, params int[] groupIds);
        void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds);
        IList<UserGroup> GetAllGroupsByRule(int ruleId);
        bool IsUserExistsInRule(int ruleId, int userId);
        IList<Person> GetAllUsersByRule(int ruleId);
    }
}
