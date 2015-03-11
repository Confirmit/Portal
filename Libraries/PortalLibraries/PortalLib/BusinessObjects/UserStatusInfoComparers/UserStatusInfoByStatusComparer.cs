using System;
using System.Collections.Generic;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.UserStatusInfoComparers
{
    public class UserStatusInfoByStatusComparer : IComparer<UserStatusInfo>
    {
        public int Compare(UserStatusInfo firstUserStatusInfo, UserStatusInfo secondUserStatusInfo)
        {
            return String.Compare(firstUserStatusInfo.Status, secondUserStatusInfo.Status, StringComparison.Ordinal);
        }
    }
}
