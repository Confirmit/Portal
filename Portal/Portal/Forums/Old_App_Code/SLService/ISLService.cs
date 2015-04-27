using System;
using System.Collections.Generic;
using System.ServiceModel;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace SLService
{
    [ServiceContract]
    public interface ISLService
    {
        [OperationContract]
        IList<Event> GetEventsForUser(int userID);

        [OperationContract]
        WorkEvent[] GetTodayWorkEventsOfUser(int userID);

        [OperationContract]
        IDictionary<TimeKey, TimeSpan> GetFullDayTimes(int userID);

        [OperationContract]
        IList<WorkEvent> GetMainWorkAndLastEvent(int userID);

        [OperationContract]
        WorkEvent[] SetUserWorkEvent(int userID, bool isOpenAction, WorkEventType eventType);

        [OperationContract]
        IList<string> GetUserPhotosAbsoluteURI(int userID);

        [OperationContract]
        bool DeleteUserPhoto(int userID, string photoURI);

        [OperationContract]
        int GetNumberOfActiveUsers();

    	[OperationContract]
    	bool IsLocalHttpRequest();

    }
}
