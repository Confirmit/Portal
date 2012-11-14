using System;
using System.ServiceModel;
using ConfirmIt.PortalLib.Arrangements;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
    /// <summary>
    /// Interface of Arrangement services.
    /// </summary>
    [ServiceContract]
    public interface IArrangementService
    {
        [OperationContract]
        XMLSerializableConferenceHall[] GetDayConferenceHallsList(int OfficeID, DateTime Date);

        [OperationContract]
        XMLSerializableConferenceHall[] GetConferenceHallsList(int OfficeID);
        
        [OperationContract]
        XMLSerializableArrangement[] GetDayArragementsList(int ConferenceHallID, DateTime Date);

        [OperationContract]
        bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, DateTime dBegin, DateTime dEnd);

        [OperationContract]
        void AddArrangement(string Name, string Description, int ConferenceHallID, DateTime DateBegin, DateTime DateEnd, string ListOfGuests, string Equipment);

        [OperationContract]
        void AddDailyCyclicArrangement(string Name, string Description, int ConferenceHallID, DateTime TimeBegin, DateTime TimeEnd, int Cycle, DateTime DateEnd, int Count, string ListOfGuests, string Equipment);

        [OperationContract]
        void AddWeeklyCyclicArrangement(string Name, string Description, int ConferenceHallID, DateTime TimeBegin, DateTime TimeEnd,
            int WeeksCycle, bool Mo, bool Tu, bool We, bool Th, bool Fr, bool Sa, bool Su, DateTime DateEnd, int Count, string ListOfGuests, string Equipment);

        [OperationContract]
        bool CheckCyclicArrangement(int ArrID);

        [OperationContract]
        void EditArrangement(int ArrangementID, string Name, string Description, int ConferenceHallID, DateTime DateBegin, DateTime DateEnd, string ListOfGuests, string Equipment);

        [OperationContract]
        void DeleteOneOfCyclicArrangements(int ArrangementID, DateTime Date);

        [OperationContract]
        void DeleteArrangement(int ArrangementID);

        [OperationContract]
        XMLSerializableArrangement GetArrangement(int ArrangementID);

        [OperationContract]
        XMLSerializableConferenceHall GetConferenceHall(int ConferenceHallID);

        [OperationContract]
        void AddConferenceHall(string Name, string Description, int OfficeID);

        [OperationContract]
        void EditConferenceHall(int ConferenceHallID, string Name, string Description, int OfficeID);

        [OperationContract]
        void DeleteConferenceHall(int ConferenceHallID);
    }
}
