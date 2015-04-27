using System;
using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces
{
    public interface IRule
    {
        void AddGroupId(int id);
        void RemoveGroupId(int id);
        List<int> GetGroupsId();
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        int IdType { get; }
        string XmlInformation { get; set; }
    }
}
