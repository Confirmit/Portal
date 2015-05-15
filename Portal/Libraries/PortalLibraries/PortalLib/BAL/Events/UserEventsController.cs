using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.DAL;

using UlterSystems.PortalLib.BusinessObjects;
using UIPProcess.Controllers;
using Navigator=UIPProcess.UIP.Navigators.Navigator;
using UserEvent=UlterSystems.PortalLib.BusinessObjects.UserEvent;

namespace ConfirmIt.PortalLib.BAL.Events
{
    /// <summary>
    /// Class for providing work with SiteProvider.Events of DAL level.
    /// </summary>
    public class UserEventInfoCtlController : EntityViewController<UserEvent>
    {
        public UserEventInfoCtlController(Navigator navigator)
            : base(navigator)
        {}

        #region Fields

        private readonly string QueryStringEventId_Key = "EventID";
        private readonly string[] m_Formats = {
                                                  "d", "D", "f", "F", "g", "G", "m",
                                                  "R", "s", "u", "y"
                                              };
        #endregion

        public override void OnLoadView(object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            if (HttpContext.Current.Request.QueryString[QueryStringEventId_Key] == null
                || ProcessedControl.IsPostBack)
                return;

            int eventID = int.Parse(HttpContext.Current.Request.QueryString[QueryStringEventId_Key]);
            UserEvent userEvent = new UserEvent(eventID);
            Person currentUser = (Person) ProcessedControl.PageInterface.CurrentWorkingUser;

            if (!currentUser.Events.Contains(userEvent))
            {
                HttpContext.Current.Response.Redirect("AccessDenied.aspx");
                return;
            }

            SelectedEntity = userEvent;
        } 

        protected override void LoadSysParameters()
        {
            base.LoadSysParameters();

            Person curUser = (Person)ProcessedControl.PageInterface.CurrentWorkingUser;

            Roles = curUser.IsInRole(RolesEnum.Administrator)
                           ? Role.GetAllRoles()
                           : curUser.Roles;

            Users = curUser.IsInRole(RolesEnum.Administrator)
                        ? UserList.GetYaroslavlOfficeUsersList()
                        : new[] {curUser};

            DateTime date = (SelectedEntity == null || SelectedEntity.DateTime.Equals(DateTime.MinValue))
                    ? DateTime.Now
                    : SelectedEntity.DateTime;

            ListItemCollection collection = new ListItemCollection();
            foreach (string format in m_Formats)
            {
                collection.Add(new ListItem(date.ToString(format), format));
            }

            DateFormats = collection;
        }

        protected override void OnAfterSaveEntity(UserEvent entity)
        {
            processSavePersonalEvents(entity);
            processSaveGroupsEvents(entity);
        }

        private void processSavePersonalEvents(UserEvent entity)
        {
            var ownersIDs = SiteProvider.Events.GetUsersOfEvent(entity.ID.Value);

            foreach (int newOwnerId in entity.OwnersUsersID)
            {
                if (!ownersIDs.Contains(newOwnerId))
                    UserEventsManager.AddIndividualEvent(newOwnerId, entity);
            }

            foreach (int oldOwnerId in ownersIDs)
            {
                if (!entity.OwnersUsersID.Contains(oldOwnerId))
                    UserEventsManager.DeleteIndividualEvent(oldOwnerId, entity.ID.Value);
            }
        }

        private void processSaveGroupsEvents(UserEvent entity)
        {
            var oldRoles = SiteProvider.Events.GetGroupsOfEvent(entity.ID.Value);

            foreach (Role newRole in entity.OwnersGroups)
            {
                if (!oldRoles.Contains(newRole))
                    newRole.AddGroupEvent(entity);
            }

            foreach (Role oldRole in oldRoles)
            {
                if (!entity.OwnersGroups.Contains(oldRole))
                    oldRole.DeleteGroupEvent(entity.ID.Value);
            }
        }

        #region State Mapped Properties

        /// <summary>
        /// State Mapped Property.
        /// </summary>
        public virtual IList<Role> Roles
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// State Mapped Property.
        /// </summary>
        public virtual IList<Person> Users
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// State Mapped Property.
        /// </summary>
        public virtual ListItemCollection DateFormats
        {
            get { return null; }
            set {}
        }

        #endregion
    }
}