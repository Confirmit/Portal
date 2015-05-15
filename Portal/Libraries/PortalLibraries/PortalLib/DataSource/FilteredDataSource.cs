using System.Collections.Generic;

using Core;
using ConfirmIt.PortalLib.FiltersSupport;
using UIPProcess.DataSources;

namespace ConfirmIt.PortalLib.DataSource
{
    public abstract class FilteredDataSourceBase<TEntityType> : DataSourceBase<TEntityType>
        where TEntityType : BasePlainObject
    {
        #region [ Filter ]

        public override object Filter
        {
            get { return m_filter; }
            set { m_filter = value; }
        }
        private object m_filter;

        #endregion

        #region [ Methods ]

        public abstract void DeleteEntity(int id);
        
        #endregion
    }

    public abstract class FilteredDataSource<TEntityType> : FilteredDataSourceBase<TEntityType>
        where TEntityType : BasePlainObject, new()
    {
        #region [ Methods ]

        public override void DeleteEntity(int id)
        {
            BasePlainObject.DeleteObjectByID(typeof(TEntityType), id);
        }

        #endregion
    }
}