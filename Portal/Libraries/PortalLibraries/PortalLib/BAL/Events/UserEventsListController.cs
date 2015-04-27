using System;
using UIPProcess.Controllers.GridView;
using UlterSystems.PortalLib.BusinessObjects;
using Navigator=UIPProcess.UIP.Navigators.Navigator;
using UserEvent=UlterSystems.PortalLib.BusinessObjects.UserEvent;

namespace ConfirmIt.PortalLib.BAL.Events
{
    public class UserEventsListController : GridViewDataSourceController<UserEvent>
    {
        public UserEventsListController(Navigator navigator)
            : base(navigator)
        {}

        public override void OnDataSourceCreated(object source, System.Web.UI.WebControls.ObjectDataSourceEventArgs e)
        {
            UserEventsProvider dataSource = e.ObjectInstance as UserEventsProvider;
            if (dataSource == null)
                return;

            dataSource.UserID = Person.RequestUser().ID.Value;
            
            base.OnDataSourceCreated(source, e);
        }

        protected override void Action(IEntitiesGridView gridView, string ationName)
        {
            if (!ationName.Equals("unsubscribe"))
                return;

            if (gridView.ObjectGridView.SelectedEntityId <= 0 && gridView.ObjectGridView.SelectedRowsIds.Length <= 0)
                return;

            DataChanged = true;

            try
            {
                Person currentUser = (Person) ProcessedControl.PageInterface.CurrentWorkingUser;

                for (int i = 0; i < gridView.ObjectGridView.SelectedRowsIds.Length; i++)
                {
                    int idEntity = gridView.ObjectGridView.SelectedRowsIds[i];
                    UserEventsManager.UnSubscribeUserOnEvent(currentUser.ID.Value, idEntity);
                }

                if (gridView.ObjectGridView.SelectedRowsIds.Length <= 0 && gridView.ObjectGridView.SelectedEntityId > 0)
                    UserEventsManager.UnSubscribeUserOnEvent(currentUser.ID.Value, gridView.ObjectGridView.SelectedEntityId);
            }
            catch (Exception e)
            {
                OnCurrentActionError();
                ErrorMessage = e.Message;
                return;
            }

            return;
        }
    }
}
