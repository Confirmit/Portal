using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IRule
    {
        void AddGroupId(int id);
        void RemoveGroupId(int id);
        List<IUserGroup> GetUserGroups();
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        int IdType { get; }
        string XmlInformation { get; set; }
    }
}
