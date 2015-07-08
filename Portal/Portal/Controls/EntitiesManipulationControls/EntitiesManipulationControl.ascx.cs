using System;
using System.Collections.Generic;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects;

namespace Portal.Controls.EntitiesManipulationControls
{
    public partial class EntitiesManipulationControl : UserControl
    {
        public event EventHandler<EntitiesManipulationEventArgs> AddEntitiesToWrapperEntityAction;
        public event EventHandler<EntitiesManipulationEventArgs> RemoveEntitiesToWrapperEntityAction;
        public Func<int, IList<object>> GetIncludedEntities;
        public Func<int, IList<object>> GetNotIncludedEntities;

        public int CurrentWrapperEntityId
        {
            get { return ViewState["CurrentWrapperEntityId"] is int ? (int)ViewState["CurrentWrapperEntityId"] : -1; }
            set { ViewState["CurrentWrapperEntityId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddEntitiesButton.Click += AddEntitiesButtonOnClick;
            RemoveEntitiesButton.Click += RemoveEntitiesButtonOnClick;

            if (GetIncludedEntities != null)
                EntitiesListIncludedControl.GetGroupsForBindingFunction += GetEntitiesForEntitiesListIncludedControl;
            if (GetNotIncludedEntities != null)
                EntitiesListNotIncludedControl.GetGroupsForBindingFunction += GetEntitiesEntitiesListNotIncludedControl;

            if (!Page.IsPostBack)
            {
                EntitiesListIncludedControl.OnEntityChanging();
                EntitiesListNotIncludedControl.OnEntityChanging();
            }
        }

        public IList<object> GetEntitiesForEntitiesListIncludedControl()
        {
            return GetIncludedEntities(CurrentWrapperEntityId);
        }

        public IList<object> GetEntitiesEntitiesListNotIncludedControl()
        {
            return GetNotIncludedEntities(CurrentWrapperEntityId);
        }

        private void AddEntitiesButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedEntities = EntitiesListNotIncludedControl.GetIdsSelectedEntities();
            var entitiesManipulationEventArgs = new EntitiesManipulationEventArgs
            {
                IdsSelectedEntities = idsSelectedEntities,
                WrapperEntityId = CurrentWrapperEntityId
            };
            if (AddEntitiesToWrapperEntityAction != null)
                AddEntitiesToWrapperEntityAction(this, entitiesManipulationEventArgs);

            EntitiesListIncludedControl.BindEntities();
            EntitiesListNotIncludedControl.BindEntities();
        }

        private void RemoveEntitiesButtonOnClick(object sender, EventArgs eventArgs)
        {
            var idsSelectedGroups = EntitiesListIncludedControl.GetIdsSelectedEntities();
            var entitiesManipulationEventArgs = new EntitiesManipulationEventArgs
            {
                IdsSelectedEntities = idsSelectedGroups,
                WrapperEntityId = CurrentWrapperEntityId
            };
            if (RemoveEntitiesToWrapperEntityAction != null)
                RemoveEntitiesToWrapperEntityAction(this, entitiesManipulationEventArgs);

            EntitiesListIncludedControl.BindEntities();
            EntitiesListNotIncludedControl.BindEntities();
        }
    }
}