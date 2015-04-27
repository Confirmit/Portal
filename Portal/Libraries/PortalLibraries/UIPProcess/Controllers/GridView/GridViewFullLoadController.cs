using System;
using System.Collections;
using System.Web.UI.WebControls;

using Core;
using UIPProcess.Controllers.GridView;
using UIPProcess.UIP.Navigators;

namespace UIPProcess.Controllers.GridView
{
    /// <summary>
    /// The base class for grid view controllers with full loading of the entities from the database.
    /// </summary>
    /// <typeparam name="TEntity">The type of Persistent Entity.</typeparam>
    public abstract class GridViewFullLoadController<TEntity>: GridViewControllerBase<TEntity>
        where TEntity : BasePlainObject, new()
    {
        #region Constructor

        public GridViewFullLoadController(Navigator navigator) : base(navigator)
        {}

        #endregion

        #region Control Events Handlers

        public override void OnLoadView(Object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            /*if (IsShouldStopProcessing(sender))
                return;
             * */

            IEntitiesGridView gridView = sender as IEntitiesGridView;
            if (gridView == null)
                throw new Exception(string.Format("IncorrectUsage {0} - {1}", GetType(), sender.GetType()));

            // Always reload the list of entities, so we do not need special
            // processing for transaction's rollback, delete operation, etc.
            LoadEntitiesList(gridView);
        }

        /// <summary>
        /// Page Index Changed event handler.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public virtual void OnGridViewPageIndexChanged(Object obj, GridViewPageEventArgs e)
        {
            System.Web.UI.WebControls.GridView grid = (System.Web.UI.WebControls.GridView)obj;

            if (0 <= e.NewPageIndex && e.NewPageIndex < grid.PageCount)
                PageIndex = e.NewPageIndex;
        }

        #endregion

        #region Actions Processing Methods

        /// <summary>
        /// This function is used to load the whole list of entities to process in a Grid View.
        /// </summary>
        /// <param name="gridView">The GridView object, used to display entities.</param>
        protected virtual void LoadEntitiesList(IEntitiesGridView gridView)
        {
            BaseObjectCollection<TEntity> coll =
                (BaseObjectCollection<TEntity>) BasePlainObject.GetObjects(typeof (TEntity));
            EntitiesList = coll;
        }

        #endregion

        #region State Mapped Properties

        /// <summary>
        /// <b>State mapped property.</b> The list of loaded Persistent Entities.
        /// </summary>
        public virtual ICollection EntitiesList
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "EntitiesList", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "EntitiesList", GetType()));
            }
        }

        /// <summary>
        /// <b>State mapped property.</b> The index of selected Page in a grid.
        /// </summary>
        public virtual Int32 PageIndex
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "PageIndex", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "PageIndex", GetType()));
            }
        }

        /// <summary>
        /// The Order expression for loading of entities.
        /// </summary>
        /*protected virtual String OrderExpression
        {
            get { return String.Empty; }
        }*/

        #endregion
    }
}