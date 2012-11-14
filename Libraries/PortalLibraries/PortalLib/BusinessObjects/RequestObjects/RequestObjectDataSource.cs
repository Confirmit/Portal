using System;
using System.Collections.Generic;

using ConfirmIt.PortalLib.FiltersSupport;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DataSource;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects
{
    public class RequestObjectHistoryDataSource : ObjectDataSource
    {
        #region Properties

        public int ObjectID { get; set; }
        
        #endregion

        public override System.Data.DataSet Select(string SortExpression, int maximumRows, int startRowIndex)
        {
            return SiteProvider.RequestObjects.GetRequestObjectHistory(ObjectID, SortExpression, maximumRows, startRowIndex);
        }

        public override int SelectCount()
        {
            return SiteProvider.RequestObjects.GetRequestObjectHistoryCount(ObjectID);
        }

        public override void DeleteEntity(int id)
        {}
    }

    public class RequestObjectDataSource : FilteredDataSourceBase<RequestObject>
    {
        #region [ Properties ]

        public Type RequestObjectType 
        {
            get { return m_RequestObjectType; }
            set { m_RequestObjectType = value; }
        }
        private Type m_RequestObjectType = null;

        #endregion

        public override IList<RequestObject> Select(string SortExpression, int maximumRows, int startRowIndex)
        {
            return SiteProvider.RequestObjects.GetFilteredRequestObjects(RequestObjectType, (RequestObjectFilter)Filter, SortExpression, maximumRows, startRowIndex);
        }

        public override int SelectCount()
        {
            return SiteProvider.RequestObjects.GetFilteredRequestObjectsCount(RequestObjectType, (RequestObjectFilter)Filter);
        }

        public override void DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }
    }
}