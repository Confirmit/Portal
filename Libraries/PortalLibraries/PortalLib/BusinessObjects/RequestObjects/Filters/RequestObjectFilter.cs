using System;
using ConfirmIt.PortalLib.FiltersSupport;

using UIPProcess.Controllers;
using Core.ORM.Attributes;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters
{
    [DBTable("RequestObject")]
    public class RequestObjectFilter : IEntitiesListFilter
    {
        #region [ Properties ]

        [DBFilterField("Title")]
        public virtual string Title { get; set; }

        [DBFilterField("OfficeID")]
        public virtual int? OfficeID { get; set; }

        [DBFilterField("OwnerID")]
        public virtual int? OwnerID { get; set; }
        
        #endregion

        #region [ IEntitiesListFilter Members ]

        public virtual bool IsFiltered
        {
            get { return !(OfficeID == null && Title == null && OwnerID == null); }
        }

        #endregion
    }
}