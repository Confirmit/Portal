using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Core;
using Core.ORM.Attributes;
using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.DAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects
{
    /// <summary>
    /// Card class.
    /// </summary>
    [DBTable("DiscountCard", true)]
    public class Card : RequestObject
    {
        #region Properties

        [DBRead("ValuePercent")]
        public int ValuePercent { get; set; }

        [DBNullable]
        [DBRead("ShopName")]
        public string ShopName { get; set; }

        [DBNullable]
        [DBRead("ShopSite")]
        public string ShopSite { get; set; }

        #endregion

        #region Methods

        public override IList<RequestObject> GetAllRequestObjects()
        {
            return ((BaseObjectCollection<Card>)Card.GetObjects(typeof(Card))).ToArray();
        }

        #endregion
    }
}