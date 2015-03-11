using System;
using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.UserStatusInfoComparers
{
    public class UserStatusInfoByNameComparer : IComparer<UserStatusInfo>
    {
        public int Compare(UserStatusInfo firstUserStatusInfo, UserStatusInfo secondUserStatusInfo)
        {
            return String.Compare(firstUserStatusInfo.UserName, secondUserStatusInfo.UserName, StringComparison.Ordinal);
        }
    }
}
