using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects
{
    public class EntitiesManipulationEventArgs
    {
        public int WrapperEntityId { get; set; }
        public IList<int> IdsSelectedEntities { get; set; }
    }
}
