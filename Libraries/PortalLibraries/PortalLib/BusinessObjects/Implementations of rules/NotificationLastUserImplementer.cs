using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules;

namespace ConfirmIt.PortalLib.BusinessObjects.Implementations_of_rules
{
    public class NotificationLastUserImplementer
    {
        private List<INotificationLastUser> _rules;

        public int UserId { get; private set; }

        public NotificationLastUserImplementer(INotificationLastUserProvider providerRules, int userId)
        {
            _rules = providerRules.GetRules();
            UserId = userId;
        }
    }
}
