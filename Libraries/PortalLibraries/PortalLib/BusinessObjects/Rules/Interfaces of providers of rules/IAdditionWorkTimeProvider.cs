using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces_of_providers_of_rules
{
    public interface IAdditionWorkTimeProvider
    {
        List<IAdditionWork> GetRules();
    }
}
