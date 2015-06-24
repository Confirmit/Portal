using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;

namespace TestOfImplementersOfRules.Factories
{
    public interface IGroupRepositoryFactory
    {
        IGroupRepository GetGroupRepository();
    }
}
