using System;

using Core.ORM.Attributes;
using ConfirmIt.PortalLib.FiltersSupport;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters
{
    [DBTable("DiscountCard", true)]
    public class CardFilter: RequestObjectFilter
    {
        #region [ Properties ]

        [DBFilterField("ValuePercent")]
        public int? ValuePercent { get; set; }

        [DBFilterField("ShopName")]
        public string ShopName { get; set; }

        [DBFilterField("ShopSite")]
        public string ShopSite { get; set; }
        
        #endregion
    }
}