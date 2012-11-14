using System;
using System.Collections.Generic;

using Core;

using UIPProcess.DataBinding;
using UIPProcess.DataValidating;
using UIPProcess.Helpers;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers
{
    /// <summary>
    /// The base class for all controllers, to process User's actions 
    /// with persistent entities.
    /// </summary>
    public class EntityViewController<TEntity> : WebCtlControllerBase<IWebControl>
        where TEntity : BasePlainObject, new()
    {
        public EntityViewController(Navigator navigator)
            : base(navigator)
        {}

        #region Controls Events Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnLoadView(Object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            IWebControl control = sender as IWebControl;
            if (control == null)
                throw new Exception(string.Format("Incorrect usage {0} - {1}.",
                                                  GetType(), sender.GetType()));

            if (IsEntityWasSelected)
                DataValidator.ClearValidators(control);

            if ( ControllerActionHelper.IsAddAction(control)
                 || (ControllerActionHelper.IsDeleteAction(control) && SelectedEntity != null
                     && DeleteSelectedEntity())
                )
            {
                SelectedEntity = default(TEntity);
                OnAfterSetSelectedEntity();

                ViewStorage = null;
                OnAfterChangeViewStorageContent();
                
                PageDataChanged = true;
                DataValidator.ClearValidators(control);
                return;
            }

            if (control.IsPostBack && !IsEntityWasSelected)
            {
                ViewStorage = new Dictionary<String, Object>();
                DataBinder.DataBindFromControlAttributedProps(control, ViewStorage);
                OnAfterChangeViewStorageContent();
            }

            LoadSysParameters();

            if (ControllerActionHelper.IsSaveAction(control)
                || ControllerActionHelper.IsSaveAsNewAction(control)
                || ControllerActionHelper.IsSaveAndNewAction(control))
            {
                IsSelectedEntityWasSaved = Save(control);
            }
        }

        public override void OnPreRenderView(Object sender, EventArgs e)
        {
            base.OnPreRenderView(sender, e);

            IWebControl control = sender as IWebControl;
            if (control == null)
                throw new Exception(string.Format("IncorrectUsage - {0} {1}",
                                                  GetType(), sender.GetType()));

            /*String strNavigate = ControllerActionHelper.GetNavigateCommand(control);
            if (!String.IsNullOrEmpty(strNavigate))
            {
                //Fix this. Pass parameters through command from the client.
                State.GetWindowsViewsManager(Navigator).OpenView(strNavigate, false, false
                                                                 , control.PageInterface.WindowsManager);
            }*/
        }

        public override void OnUnloadView(object control, EventArgs e)
        {
            base.OnUnloadView(control, e);

            PageDataChanged = false;

            ViewStorage = null;
            OnAfterChangeViewStorageContent();

            ErrorMessage = null;
            IsSelectedEntityWasSaved = false;
        }

        #endregion

        #region Actions Processing Methods

        /// <summary>
        /// Called in the case of rolling back of database transaction.
        /// All dependent objects should be immediately refreshed.
        /// </summary>
        protected virtual void OnCurrentActionError()
        {
        }

        /// <summary>
        /// This function is used to create the entity. Could be overwritten to 
        /// perform additional actions during creation.
        /// </summary>
        /// <returns>Created <b>TPersistentEntity</b> object.</returns>
        protected virtual TEntity CreateNewEntity()
        {
            TEntity entity = new TEntity();
            return entity;
        }

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
        /// The function is used to delete the entity from the database.
        /// </summary>
        /// <returns><b>True</b> if entity was successfully deleted, <b>false</b> otherwise.</returns>
        protected virtual Boolean DeleteSelectedEntity()
        {
            try
            {
                OnDeleteEntity(SelectedEntity);
                SelectedEntity.Delete();

                return true;
            }
            catch (Exception ex)
            {
                OnCurrentActionError();

                ErrorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// The virtual function, which could be overwritten to load the System Parameters
        /// for entity.
        /// </summary>
        protected virtual void LoadSysParameters()
        {
        }

        /// <summary>
        /// The Data Binding function from the control to Domain object (Persistent Entity).
        /// Could be overwritten to perform additional binding operations and initializations of 
        /// entity's properties. Called during saving of the entity.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="entity"></param>
        protected virtual Boolean DataBind(IWebControl control, TEntity entity)
        {
            DataBinder.DataBindFromControlAttributedProps(control, entity);
            return true;
        }

        /// <summary>
        /// Called for each entity just before the actual save of entity in the database.
        /// Should be overwritten to perform additional operations for adding of entity 
        /// to persistent collections, etc.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnSaveEntity(TEntity entity)
        {
        }

        /// <summary>
        /// Called for each entity just after the actual save of entity in the database.
        /// Should be overwritten to perform additional operations for storing of 
        /// other entitie's related information in the database.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnAfterSaveEntity(TEntity entity)
        {
        }

        /// <summary>
        /// Called after setting of SelectedEntity property.
        /// </summary>
        protected virtual void OnAfterSetSelectedEntity()
        {
        }

        /// <summary>
        /// Called after changing of the content of ViewStorage.
        /// </summary>
        protected virtual void OnAfterChangeViewStorageContent()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected virtual Boolean Save(IWebControl control)
        {
            if (!DataValidator.Validate(control))
            {
                ErrorMode = true;
                return false;
            }

            TEntity entity = default(TEntity);
            try
            {
                entity = ControllerActionHelper.IsSaveAsNewAction(control)
                             ? default(TEntity)
                             : SelectedEntity;

                if (entity == null)
                    entity = CreateNewEntity();
                if (entity == null)
                    return false;

                if (!DataBind(control, entity))
                    return false;
                
                OnSaveEntity(entity);
                entity.Save();
                OnAfterSaveEntity(entity);

                if (!ControllerActionHelper.IsSaveAndNewAction(control))
                {
                    SelectedEntity = entity;
                    OnAfterSetSelectedEntity();
                }
                else
                {
                    SelectedEntity = default(TEntity);
                    OnAfterSetSelectedEntity();
                }

                PageDataChanged = true;
                
                ViewStorage = null;
                OnAfterChangeViewStorageContent();

                ErrorMode = false;

                return true;
            }
            catch (Exception ex)
            {
                OnCurrentActionError();
                
                ErrorMessage = ex.Message;
                ErrorMode = true;
                return false;
            }
        }

        #endregion

        #region State Mapped Properties

        /// <summary>
        /// <b>State mapped property.</b> Selected entity for processing.
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
        /// <b>State mapped property.</b> The error message which was generated during the 
        /// last action of the User. <b>Empty</b>, if there is was no error during the last action.
        /// </summary>
        public virtual String ErrorMessage
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "ErrorMessage", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "ErrorMessage", GetType()));
            }
        }

        /// <summary>
        /// <b>State mapped property.</b> 
        /// <para>If <b>true</b>, then User has done several actions,
        /// tried to save the <b>Entity</b> in the database and receive an error (either 
        /// generated by database constraints or by validations on the server).
        /// </para>
        /// <para>
        /// If <b>false</b>, then there was not any error in the last saving actions which User
        /// has done with the <b>Entity</b>.
        /// </para>
        /// </summary>
        public virtual Boolean ErrorMode
        {
            set { }
        }

        /// <summary>
        /// <b>State mapped property.</b> Used for temporary storing of information,
        /// entered by the User during Post Backs.
        /// </summary>
        public virtual IDictionary<String, Object> ViewStorage
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "ViewStorage", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "ViewStorage", GetType()));
            }
        }

        /// <summary>
        /// <b>State mapped property.</b>
        /// </summary>
        public virtual Boolean PageDataChanged
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "PageDataChanged", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "PageDataChanged", GetType()));
            }
        }

        /// <summary>
        /// <para><b>State mapped property.</b> Indicates whether SelectedEntity was 
        /// saved during current save operation.</para>
        /// <para>This property could be used without mapping in the scope of one controller.</para>
        /// </summary>
        public virtual Boolean IsSelectedEntityWasSaved
        {
            get
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "IsSelectedEntityWasSaved", GetType()));
            }
            set
            {
                throw new Exception(string.Format("StateMappingError: {0} - {1}."
                                                  , "IsSelectedEntityWasSaved", GetType()));
            }
        }

        /// <summary>
        /// State mapped property
        /// </summary>
        public virtual Boolean IsEntityWasSelected
        {
            get { return false; }
        }

        #endregion
    }
}