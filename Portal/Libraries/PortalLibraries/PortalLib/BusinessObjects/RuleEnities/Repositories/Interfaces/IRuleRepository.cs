using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.Rules;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces
{
    public interface IRuleRepository
    {
        IList<Rule> GetAllRules();
        IList<T> GetAllRulesByType<T>() where T : Rule, new(); 
        void SaveRule(Rule rule);
        void DeleteRule(int ruleId);
        T GetRuleById<T>(int ruleId) where T : Rule, new();
        void AddGroupIdsToRule(int ruleId, params int[] groupIds);
        void DeleteGroupIdsFromRule(int ruleId, params int[] groupIds);
        IList<UserGroup> GetAllGroupsByRule(int ruleId);
        bool IsUserExistsInRule(int ruleId, int userId);
        IList<Person> GetAllUsersByRule(int ruleId);
    }
}
