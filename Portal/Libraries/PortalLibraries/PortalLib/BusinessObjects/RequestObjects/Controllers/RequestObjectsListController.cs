using System;

using Core;

using UIPProcess.UIP.Navigators;
using UIPProcess.Controllers.GridView;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class BooksListController : RequestObjectsListController<Book>
    {
        #region [ Constructors ]

        public BooksListController(Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class DiskListController : RequestObjectsListController<Disk>
    {
        #region [ Constructors ]

        public DiskListController(Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class CardListController : RequestObjectsListController<Card>
    {
        #region [ Constructors ]

        public CardListController(Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class RequestObjectsListController<TEntity> : FilteredGridViewController<RequestObjectFilter, TEntity>
        where TEntity : BasePlainObject, new()
    {
        #region [ Constructors ]

        public RequestObjectsListController(Navigator navigator)
            : base(navigator)
        { }

        #endregion

        public override void OnDataSourceCreated(object source, System.Web.UI.WebControls.ObjectDataSourceEventArgs e)
        {
            var dataSource = (RequestObjectDataSource)e.ObjectInstance;
            dataSource.RequestObjectType = RequestObjectType;

            base.OnDataSourceCreated(source, e);
        }

        /// <summary>
        /// State mapped property
        /// </summary>
        public virtual Type RequestObjectType
        {
            get { return null; }
            set { }
        }
    }
}