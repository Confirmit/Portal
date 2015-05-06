using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.FilesManagers;
using ConfirmIt.PortalLib.Logger;
using ConfirmIt.PortalLib.WebServiceSupport;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.DB;
using System.Web;

namespace SLService
{
    public enum TimeKey
    {
        TodayWork = 0,
        TodayRest = 2,
        WeekRest = 3,
        MonthRest = 4
    }

	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SLService : ISLService
    {
        public IList<Event> GetEventsForUser(int userID)
        {
            return SiteProvider.Events.GetAllUserEventsData(userID, true);
        }

        public WorkEvent[] GetTodayWorkEventsOfUser(int userID)
        {
            return WorkEvent.GetEventsOfDate(userID, DateTime.Today);
        }

        public IDictionary<TimeKey ,TimeSpan> GetFullDayTimes(int userID)
        {
            IDictionary<TimeKey, TimeSpan> dict = new Dictionary<TimeKey, TimeSpan>();
            UserTimeCalculator TimesCalc = new UserTimeCalculator(userID);
            
            dict[TimeKey.TodayWork] = TimesCalc.GetWorkedTimeWithLunch(DateTime.Today);

            TimeSpan todayRest = TimesCalc.GetRateWithLunch(DateTime.Today);
            todayRest -= TimesCalc.GetWorkedTimeWithLunch(DateTime.Today);
            dict[TimeKey.TodayRest] = todayRest;

            dict[TimeKey.WeekRest] =  TimesCalc.GetRestTimeWithLunch(DateClass.WeekBegin(DateTime.Today),
                                                    DateClass.WeekBegin(DateTime.Today).AddDays(6));

            dict[TimeKey.MonthRest] = TimesCalc.GetRestTimeWithLunch(
                         new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0),
                         new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0).AddDays(
                             DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) - 1)
                         );

            return dict;
        }

        public IList<WorkEvent> GetMainWorkAndLastEvent(int userID)
        {
            return new List<WorkEvent>
                       {
                           WorkEvent.GetMainWorkEvent(userID, DateTime.Today),
                           WorkEvent.GetCurrentEventOfDate(userID, DateTime.Today)
                       };
        }

        public WorkEvent[] SetUserWorkEvent(int userID, bool isOpenAction, WorkEventType eventType)
        {
            try
            {
                var userWorkEvents = new UserWorkEvents(userID);

                switch (eventType)
                {
                    case WorkEventType.MainWork:
                        {
                            if (isOpenAction)
                                userWorkEvents.OpenMainWorkEvent();
                            else
                                userWorkEvents.CloseMainWorkEvent();

                            break;
                        }

                    case WorkEventType.TimeOff:
                        {
                            if (isOpenAction)
                                userWorkEvents.OpenWorkBreakEvent(WorkEventType.TimeOff);
                            else
                                userWorkEvents.CloseWorkBreakEvent();
                            break;
                        }

                    case WorkEventType.LanchTime:
                        {
                            if (isOpenAction)
                                userWorkEvents.OpenLunchEvent();
                            else
                                userWorkEvents.CloseLunchEvent();
                            break;
                        }

                    case WorkEventType.StudyEnglish:
                        {
                            if (isOpenAction)
                                userWorkEvents.OpenWorkBreakEvent(WorkEventType.StudyEnglish);
                            else
                                userWorkEvents.CloseWorkBreakEvent();

                            break;
                        }
                }
                return GetTodayWorkEventsOfUser(userID);
            }
            catch
            {
                return null;
            }
        }

        public int GetNumberOfActiveUsers()
        {
            return DBManager.GetNumberOfActiveUsers();
        }

    	public bool IsLocalHttpRequest()
    	{
    		return WebHelpers.IsRequestIPAllowed();
    	}

    	#region User photos supply

        public IList<String> GetUserPhotosAbsoluteURI(int userID)
        {
            IList<String> resList = new List<String>();
            FileManager fileManager = new FileManager(userID);

            foreach (PersonAttribute attribute in PersonAttributes.GetPersonAttributesByKeyword(userID, PersonAttributeTypes.Photo.ToString()))
            {
                if (!String.IsNullOrEmpty(attribute.StringField))
                    resList.Add(fileManager.GetFileURL(attribute.StringField));
            }

            return resList;
        }

        public bool DeleteUserPhoto(int userID, String photoAbsoluteURI)
        {
            try
            {
                FileManager fileManager = new FileManager(userID);
                FileProperties properties = fileManager.GetFilePropertiesByURI(photoAbsoluteURI);

                IList<PersonAttribute> userPhoto =
                    PersonAttributes.GetPersonAttributesByKeyword(userID, PersonAttributeTypes.Photo.ToString(),
                                                                  "StringField", properties.FileName);
                if (userPhoto == null || userPhoto.Count == 0)
                    return false;
                
                userPhoto[0].Delete();
                fileManager.DeleteFileByURI(photoAbsoluteURI);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Instance.Error("Error while deleting user photo attribute." , ex);
                return false;
            }
        }

        #endregion
    }
}