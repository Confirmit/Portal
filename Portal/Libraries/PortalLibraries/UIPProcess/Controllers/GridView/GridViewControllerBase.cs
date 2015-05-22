using System;

using Core;
using UIPProcess.Helpers;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers.GridView
{
    /// <summary>
    /// Interface contains definitions of Controller event handlers.
    /// </summary>
    public interface IGridViewControllerBase
    {
        void OnSelectRow(Object sender, EventArgs args);
    }

    /// <summary>
    /// The base class for controllers, which are used to process entities in a grid.
    /// </summary>
    /// <typeparam name="TEntity">The type of Entity.</typeparam>
    public abstract class GridViewControllerBase<TEntity> : WebCtlControllerBase<IWebControl>, IGridViewControllerBase
        where TEntity : BasePlainObject, new()
    {
        #region Constructor

        public GridViewControllerBase(Navigator navigator) 
            : base(navigator)
        {}
        
        #endregion

        #region Control Events Handlers

        public virtual void OnSelectRow(Object sender, EventArgs args)
        {
            Controls.HotGridView.GridView gridView = sender as Controls.HotGridView.GridView;
            if (gridView == null)
                return;
            
            if (gridView.SelectedEntityId > 0 && !String.IsNullOrEmpty(gridView.DefaultRowType))
                Select(gridView.SelectedEntityId, gridView.DefaultRowType);
        }

        /// <summary>
        /// Overridden method, used to route "Delete" action to particular 
        /// handler.
        /// </summary>
        /// <param name="sender">The grid view control for processing.</param>
        /// <param name="e">The event arguments.</param>
        public override void OnLoadView(Object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            IsEntityWasSelected = false;

            IEntitiesGridView gridView = sender as IEntitiesGridView;
            if (gridView == null)
                throw new Exception(String.Format("IncorrectUsage {0} - {1}", GetType(), sender.GetType()));

            if (ControllerActionHelper.IsDeleteAction(gridView))
            {
                Delete(gridView);
                return;
            }

            String action = ControllerActionHelper.GetActionFieldValue(gridView);
            if (!string.IsNullOrEmpty(action))
            {
                Action(gridView, action);
                return;
            }

            if (gridView.ObjectGridView.SelectedEntityId > 0 && String.IsNullOrEmpty(gridView.ObjectGridView.NavigateOnAdd))
                Select(gridView.ObjectGridView.SelectedEntityId, gridView.ObjectGridView.SelectedEntityType);
        }

        /// <summary>
        /// Overridden method, used to route "New" and "Select" actions to particular 
        /// handlers.
        /// </summary>
        /// <param name="sender">The grid view control for processing.</param>
        /// <param name="e">The event arguments.</param>
        public override void OnPreRenderView(Object sender, EventArgs e)
        {
            base.OnPreRenderView(sender, e);

            IEntitiesGridView gridView = sender as IEntitiesGridView;
            if (gridView == null)
                throw new Exception(string.Format("IncorrectUsage {0} - {1}", GetType(), sender.GetType()));

            /*if (ControllerActionHelper.IsAddAction(gridView))
            {
                String strNavigate = ControllerActionHelper.GetNavigateCommand(gridView);
                if (!String.IsNullOrEmpty(strNavigate))
                    State.GetWindowsViewsManager(Navigator).OpenView(strNavigate, true, false
                                                                     , gridView.PageInterface.WindowsManager);
                else if (!String.IsNullOrEmpty(gridView.ObjectGridView.NavigateOnAdd))
                    State.GetWindowsViewsManager(Navigator).OpenView(gridView.ObjectGridView.NavigateOnAdd, true, false
                                                                     , gridView.PageInterface.WindowsManager);
            }
            else*/
            if (gridView.ObjectGridView.SelectedEntityId > 0 && !String.IsNullOrEmpty(gridView.ObjectGridView.NavigateOnSelect))
            {
                //State.GetWindowsViewsManager(Navigator).OpenView(gridView.ObjectGridView.NavigateOnSelect, true, false
                //                                                 , gridView.PageInterface.WindowsManager);
                Select(gridView.ObjectGridView.SelectedEntityId, gridView.ObjectGridView.SelectedEntityType);
            }
        }

        public override void OnUnloadView(object control, EventArgs e)
        {
            base.OnUnloadView(control, e);

            IsEntityWasSelected = false;
            ErrorMessage = null;
        }

        #endregion

        #region Actions Processing Methods
        
        /// <summary>
        /// Called in the case of rolling back of database transaction.
        /// All dependent objects should be immediately refreshed.
        /// </summary>
        protected virtual void OnCurrentActionError()
        {}

        /// <summary>
        /// Called for each entity just before the actual deletion of entity from the database.
        /// Should be overwritten to perform additional operations for deleting of entity (removing 
        /// it from persistent collections, etc.)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnDeleteEntity(TEntity entity)
        {
        }

        /// <summary>
        /// Processing of the deleting of selected entities in a grid view.
        /// </summary>
        /// <param name="gridView">Grid View with the set of selected entities for deletion.</param>
        /// <returns><b>True</b> if all selected entities were successfully deleted, 
        /// <b>false</b> otherwise.</returns>
        protected virtual Boolean Delete(IEntitiesGridView gridView)
        {
            if (gridView.ObjectGridView.SelectedEntityId <= 0 && gridView.ObjectGridView.SelectedRowsIds.Length <= 0)
                return false;

            try
            {
                for (Int32 i = 0; i < gridView.ObjectGridView.SelectedRowsIds.Length; i++)
                {
                    Int32 idEntity = gridView.ObjectGridView.SelectedRowsIds[i];
                    deleteEntity(idEntity);
                }

                if (gridView.ObjectGridView.SelectedRowsIds.Length <= 0 && gridView.ObjectGridView.SelectedEntityId > 0)
                    deleteEntity(gridView.ObjectGridView.SelectedEntityId);
            }
            catch (Exception e)
            {
                OnCurrentActionError();
                ErrorMessage = e.Message;
                return false;
            }

            return true;
        }

        private void deleteEntity(int idEntity)
        {
            var entity = new TEntity();

            entity.Load(idEntity);
            OnDeleteEntity(entity);
            entity.Delete();
        }

        /// <summary>
        /// Processin other action.
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="ationName"></param>
        protected virtual void Action(IEntitiesGridView gridView, String ationName)
        {}

        /// <summary>
        /// Processing of "Select" command. User is able to select the entity for
        /// particular row in a grid view, or the "part" of this entity, by clicking to
        /// the link in the corresponding column.
        /// </summary>
        /// <param name="idEntity"></param>
        /// <param name="strFullType"></param>
        protected virtual void Select(Int32 idEntity, String strFullType)
        {
            //if (String.IsNullOrEmpty(strFullType))
            //  return;

            try 
            {
                TEntity entity = new TEntity();
                entity.Load(idEntity);

                SelectedEntity = entity;
                                
                IsEntityWasSelected = true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                OnCurrentActionError();
            }
        }

        #endregion

        #region State Mapped Properties

        /// <summary>
        /// <b>State mapped property.</b> Selected entity in a grid, used when User do 
        /// a selection of particular entity in a row, or a part of entity (other object),
        /// displayed in a column.
        /// </summary>
        public virtual TEntity SelectedEntity
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "SelectedEntity", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "SelectedEntity", GetType()));
            }
        }

        /// <summary>
        /// <para><b>State mapped property.</b> Indicated if <b>Entity</b> selected 
        /// during the last operation (selection).</para>
        /// <para>This property could be used without mapping inside one controller.</para>
        /// </summary>
        public virtual Boolean IsEntityWasSelected
        {
            get { return _isEntityWasSelected; }
            set { _isEntityWasSelected = value; }
        }
        Boolean _isEntityWasSelected = false;

        /// <summary>
        /// <b>State mapped property.</b> The error message which was generated during the 
        /// last action of the User. <b>Empty</b>, if there is was no error during the last action.
        /// </summary>
        public virtual String ErrorMessage
        {
            get { return String.Empty; }
            set { }
        }

        #endregion
    }
}