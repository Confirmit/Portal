using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers
{
    public interface IUserProvider
    {
        IList<int> GetUsersByGroup(int groupId);
    }
}
