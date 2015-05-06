using UIPProcess.Controllers.GridView;
using UlterSystems.PortalLib.BusinessObjects;
using Navigator=UIPProcess.UIP.Navigators.Navigator;
using UserEvent=UlterSystems.PortalLib.BusinessObjects.UserEvent;

namespace ConfirmIt.PortalLib.BAL.Events
{
    public class UserSubscribeEventsListController : GridViewDataSourceController<UserEvent>
    {
        public UserSubscribeEventsListController(Navigator navigator)
            : base(navigator)
        {}

        public override void OnDataSourceCreated(object source, System.Web.UI.WebControls.ObjectDataSourceEventArgs e)
        {
            var dataSource = e.ObjectInstance as UserSubscribeEventsDataSource;
            if (dataSource == null)
                return;

            dataSource.UserID = Person.RequestUser().ID.Value;
            
            base.OnDataSourceCreated(source, e);
        }

        protected override void Select(int idEntity, string strFullType)
        {
            Person currentUser = (Person)ProcessedControl.PageInterface.CurrentWorkingUser;

            UserEventsManager.SubscribeUserOnEvent(currentUser.ID.Value, idEntity);
            DataChanged = true;
        }
    }
}

