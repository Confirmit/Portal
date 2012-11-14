using System;
using ConfirmIt.PortalLib.FiltersSupport;
using Core.ORM.Attributes;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters
{
    [DBTable("Disks", true)]
    public class DiskFilter: RequestObjectFilter
    {
        #region [ Properties ]

        [DBFilterField("Manufacturers")]
        public string Manufacturers { get; set; }

        [DBFilterField("Annotation")]
        public string Annotation { get; set; }

        [DBFilterField("PublishingYear", Operator = ">=")]
        public int FromYear { get; set; }

        [DBFilterField("PublishingYear", Operator = "<=")]
        public int ToYear { get; set; }
        
        #endregion
    }
}
