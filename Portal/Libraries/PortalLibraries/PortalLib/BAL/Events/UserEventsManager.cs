using ConfirmIt.PortalLib.DAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL.Events
{
    public class UserEventsManager
    {
        #region Methods

        /// <summary>
        /// Add individual event to person.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="eventData">event data.</param>
        public static void AddIndividualEvent(int userID, Event eventData)
        {
            SiteProvider.Events.AddPersonalUserEvent(userID, eventData);
        }

        /// <summary>
        /// Remove individual person event.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="eventID">Event ID.</param>
        public static void DeleteIndividualEvent(int userID, int eventID)
        {
            SiteProvider.Events.DeleteEventFromUser(userID, eventID);
        }

        public static void SubscribeUserOnEvent(int userID, int eventID)
        {
            SiteProvider.Events.SubscribeUserOnEvent(userID, eventID);
        }

        public static void UnSubscribeUserOnEvent(int userID, int eventID)
        {
            SiteProvider.Events.UnSubscribeUserOnEvent(userID, eventID);
        }

        #endregion
    }
}
