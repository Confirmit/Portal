
using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IUserGroup
    {
        List<int> GetUsersId();
        void AddUserId(int id);
        void RemoveUserId(int id);
        string Description { get; set; }
        string TableAccordName { get; }
    }
}
