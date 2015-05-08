using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.Rules;
using Core;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class GroupProvider<T> : IGroupProvider<T> where T : UserGroup, new()
    {
        private List<T> _groups = new List<T>();
        public IList<T> GetGroupsByRule(int idRule)
        {
            if (_groups.Count != 0) return _groups;
            var rules = (IEnumerable<T>)BasePlainObject.GetObjectsPageWithCondition(typeof(T), new PagingArgs(0, 100, "ID", true), "IdType", idRule).Result;
            return rules.ToList();
        }

        public void SaveGroup(T rule)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteGroup(int id)
        {
            GetGroupById(id).Delete();
        }

        public T GetGroupById(int id)
        {
            T instance = new T();
            instance.Load(id);
            return instance;
        }
    }
}