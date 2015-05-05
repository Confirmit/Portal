using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;

namespace TestOfImplementersOfRules
{
    public class TestNotificationLastUserProvider : INotificationLastUserProvider
    {

        public List<INotificationLastUser> GetRules()
        {
            throw new NotImplementedException();
        }
    }
}