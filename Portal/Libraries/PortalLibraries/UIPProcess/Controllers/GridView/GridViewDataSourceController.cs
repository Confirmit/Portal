using System;
using System.Web.UI.WebControls;

using Core;

using UIPProcess.Controllers.GridView;
using UIPProcess.UIP.Navigators;
using UIPProcess.DataSources;

namespace UIPProcess.Controllers.GridView
{
    /// <summary>
    /// Interface contains definitions of Controller event handlers.
    /// </summary>
    public interface IGridViewDataSourceController : IGridViewControllerBase
    {
        void OnDataSourceCreated(Object source, ObjectDataSourceEventArgs e);
        void OnGridViewSorting(Object obj, GridViewSortEventArgs e);
    }

    /// <summary>
    /// The base class for controllers, which are used to process entities in a grid
    /// and the <b>Object Data Source</b> to load entities.
    /// </summary>
    public abstract class GridViewDataSourceController<TEntity> : GridViewControllerBase<TEntity>, IGridViewDataSourceController
        where TEntity : BasePlainObject, new()
    {
        #region [ Constructors ]

        public GridViewDataSourceController(Navigator navigator)
            : base(navigator)
        {}

        #endregion

        #region [ Controls Events Handlers ]

        /// <summary>
        /// The DataSourceCreated event handler. Used to initialse the Data Source.
        /// </summary>
        /// <param name="source">The created Data Source, should implemente IDataSourceBase interface.</param>
        /// <param name="e"></param>
        public virtual void OnDataSourceCreated(Object source, ObjectDataSourceEventArgs e)
        {
        }

        /// <summary>
        /// Handler of sorting event.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public virtual void OnGridViewSorting(Object obj, GridViewSortEventArgs e)
        {
            SortingExpression = e.SortExpression + (e.SortDirection == SortDirection.Descending ? " DESC" : "");
        }

        public override void OnUnloadView(object control, EventArgs e)
        {
            base.OnUnloadView(control, e);
            DataChanged = false;
        }

        #endregion

        #region [ Actions Processing Methods ]

        protected override Boolean Delete(IEntitiesGridView gridView)
        {
            Boolean fRes = base.Delete(gridView);

            if (fRes)
                DataChanged = true;

            return fRes;
        }

        #endregion

        #region [ State Mapped Properties ]

        /// <summary>
        /// <b>State mapped property.</b> Used to store the currently selected sorting expression in a State.
        /// </summary>
        public virtual String SortingExpression
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// <b>State mapped property.</b> Used to notify the view that data was changed and should
        /// be binded to the Grid View.
        /// </summary>
        public virtual Boolean DataChanged
        {
            get { return false; }
            set { }
        }

        #endregion
    }
}