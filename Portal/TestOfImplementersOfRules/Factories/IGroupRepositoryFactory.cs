using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.Interfaces;

namespace TestOfImplementersOfRules.Factories
{
    public interface IGroupRepositoryFactory
    {
        IGroupRepository GetGroupRepository();
    }
}
