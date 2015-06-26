using System.Collections.Generic;

namespace Portal.TestingEntitiesManipulation
{
    public interface IEntityManipulationPage
    {
        IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId);
        IList<object> GetNotIncludedEntitiesForBinding(int wrapperEntityId);
        void AddEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities);
        void RemoveEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities);
    }
}