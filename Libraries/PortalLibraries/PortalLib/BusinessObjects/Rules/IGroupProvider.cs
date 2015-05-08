using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public interface IGroupProvider<T> where T : UserGroup
    {
        IList<T> GetGroupsByRule(int idRule);
        void SaveGroup(T rule);
        void DeleteGroup(int id);
        T GetGroupById(int id);
    }
}