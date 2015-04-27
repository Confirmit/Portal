using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IRule
    {
        void AddGroupId(int id);
        void RemoveGroupId(int id);
        List<int> GetGroupsId();
    }
}
