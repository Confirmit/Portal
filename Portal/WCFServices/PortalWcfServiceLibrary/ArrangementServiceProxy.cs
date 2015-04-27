using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using ConfirmIt.Portal.WcfServiceLibrary;

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IArrangementService")]
public interface IArrangementService
{
    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/GetDayConferenceHallsList", ReplyAction = "http://tempuri.org/IArrangementService/GetDayConferenceHallsListResponse")]
    ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall[] GetDayConferenceHallsList(int OfficeID, System.DateTime Date);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/GetConferenceHallsList", ReplyAction = "http://tempuri.org/IArrangementService/GetConferenceHallsListResponse")]
    ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall[] GetConferenceHallsList(int OfficeID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/GetDayArragementsList", ReplyAction = "http://tempuri.org/IArrangementService/GetDayArragementsListResponse")]
    ConfirmIt.PortalLib.Arrangements.XMLSerializableArrangement[] GetDayArragementsList(int ConferenceHallID, System.DateTime Date);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/CheckArrangementAdding", ReplyAction = "http://tempuri.org/IArrangementService/CheckArrangementAddingResponse")]
    bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, System.DateTime dBegin, System.DateTime dEnd);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/AddArrangement", ReplyAction = "http://tempuri.org/IArrangementService/AddArrangementResponse")]
    void AddArrangement(string Name, string Description, int ConferenceHallID, System.DateTime DateBegin, System.DateTime DateEnd, string ListOfGuests, string Equipment);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/AddDailyCyclicArrangement", ReplyAction = "http://tempuri.org/IArrangementService/AddDailyCyclicArrangementResponse")]
    void AddDailyCyclicArrangement(string Name, string Description, int ConferenceHallID, System.DateTime TimeBegin, System.DateTime TimeEnd, int Cycle, System.DateTime DateEnd, int Count, string ListOfGuests, string Equipment);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/AddWeeklyCyclicArrangement", ReplyAction = "http://tempuri.org/IArrangementService/AddWeeklyCyclicArrangementResponse")]
    void AddWeeklyCyclicArrangement(string Name, string Description, int ConferenceHallID, System.DateTime TimeBegin, System.DateTime TimeEnd,
            int WeeksCycle, bool Mo, bool Tu, bool We, bool Th, bool Fr, bool Sa, bool Su, System.DateTime DateEnd, int Count, string ListOfGuests, string Equipment);
        
    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/CheckCyclicArrangement", ReplyAction = "http://tempuri.org/IArrangementService/CheckCyclicArrangementResponse")]
    bool CheckCyclicArrangement(int ArrID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/EditArrangement", ReplyAction = "http://tempuri.org/IArrangementService/EditArrangementResponse")]
    void EditArrangement(int ArrangementID, string Name, string Description, int ConferenceHallID, System.DateTime DateBegin, System.DateTime DateEnd, string ListOfGuests, string Equipment);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/DeleteArrangement", ReplyAction = "http://tempuri.org/IArrangementService/DeleteArrangementResponse")]
    void DeleteArrangement(int ArrangementID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/DeleteOneOfCyclicArrangements", ReplyAction = "http://tempuri.org/IArrangementService/DeleteOneOfCyclicArrangementsResponse")]
    void DeleteOneOfCyclicArrangements(int ArrangementID, System.DateTime Date);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/GetArrangement", ReplyAction = "http://tempuri.org/IArrangementService/GetArrangementResponse")]
    ConfirmIt.PortalLib.Arrangements.XMLSerializableArrangement GetArrangement(int ArrangementID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/GetConferenceHall", ReplyAction = "http://tempuri.org/IArrangementService/GetConferenceHallResponse")]
    ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall GetConferenceHall(int ConferenceHallID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/AddConferenceHall", ReplyAction = "http://tempuri.org/IArrangementService/AddConferenceHallResponse")]
    void AddConferenceHall(string Name, string Description, int OfficeID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/EditConferenceHall", ReplyAction = "http://tempuri.org/IArrangementService/EditConferenceHallResponse")]
    void EditConferenceHall(int ConferenceHallID, string Name, string Description, int OfficeID);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IArrangementService/DeleteConferenceHall", ReplyAction = "http://tempuri.org/IArrangementService/DeleteConferenceHallResponse")]
    void DeleteConferenceHall(int ConferenceHallID);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IArrangementServiceChannel : IArrangementService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class ArrangementServiceProxy : System.ServiceModel.ClientBase<IArrangementService>, IArrangementService
{
    #region Fields

    private readonly int? m_UserID;

    #endregion

    #region Constructors
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="url">URL of Web service.</param>
    /// <param name="userName">User ID.</param>
    public ArrangementServiceProxy(string url, int? userID)
        : base(new WSHttpBinding(), new EndpointAddress(url)) //?
    {
        m_UserID = userID;
    }

    public ArrangementServiceProxy()
    {
    }
    #endregion

    public ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall[] GetDayConferenceHallsList(int OfficeID, System.DateTime Date)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return Channel.GetDayConferenceHallsList(OfficeID, Date);
        }
    }

    public ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall[] GetConferenceHallsList(int OfficeID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return base.Channel.GetConferenceHallsList(OfficeID);
        }
    }

    public ConfirmIt.PortalLib.Arrangements.XMLSerializableArrangement[] GetDayArragementsList(int ConferenceHallID, System.DateTime Date)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return Channel.GetDayArragementsList(ConferenceHallID, Date);
        }
    }

    public bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, System.DateTime dBegin, System.DateTime dEnd)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return Channel.CheckArrangementAdding(ConferenceHallID, ArrangementID, dBegin, dEnd);
        }
    }

    public void AddArrangement(string Name, string Description, int ConferenceHallID, System.DateTime DateBegin, System.DateTime DateEnd, string ListOfGuests, string Equipment)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            Channel.AddArrangement(Name, Description, ConferenceHallID, DateBegin, DateEnd, ListOfGuests, Equipment);
        }
    }

    public void AddDailyCyclicArrangement(string Name, string Description, int ConferenceHallID, System.DateTime TimeBegin, System.DateTime TimeEnd, int Cycle, System.DateTime DateEnd, int Count, string ListOfGuests, string Equipment)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            Channel.AddDailyCyclicArrangement(Name, Description, ConferenceHallID, TimeBegin, TimeEnd, Cycle, DateEnd, Count, ListOfGuests, Equipment);
        }
    }

    public void AddWeeklyCyclicArrangement(string Name, string Description, int ConferenceHallID, System.DateTime TimeBegin, System.DateTime TimeEnd,
            int WeeksCycle, bool Mo, bool Tu, bool We, bool Th, bool Fr, bool Sa, bool Su, System.DateTime DateEnd, int Count, string ListOfGuests, string Equipment)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            Channel.AddWeeklyCyclicArrangement(Name, Description, ConferenceHallID, TimeBegin, TimeEnd,
            WeeksCycle, Mo, Tu, We, Th, Fr, Sa, Su, DateEnd, Count, ListOfGuests, Equipment);
        }
    }

    public bool CheckCyclicArrangement(int ArrID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return Channel.CheckCyclicArrangement(ArrID);
        }
    }

    public void EditArrangement(int ArrangementID, string Name, string Description, int ConferenceHallID, System.DateTime DateBegin, System.DateTime DateEnd, string ListOfGuests, string Equipment)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.EditArrangement(ArrangementID, Name, Description, ConferenceHallID, DateBegin, DateEnd, ListOfGuests, Equipment);
        }
    }

    public void DeleteArrangement(int ArrangementID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.DeleteArrangement(ArrangementID);
        }
    }

    public void DeleteOneOfCyclicArrangements(int ArrangementID, System.DateTime Date)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.DeleteOneOfCyclicArrangements(ArrangementID, Date);
        }
    }

    public ConfirmIt.PortalLib.Arrangements.XMLSerializableArrangement GetArrangement(int ArrangementID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return base.Channel.GetArrangement(ArrangementID);
        }
    }

    public ConfirmIt.PortalLib.Arrangements.XMLSerializableConferenceHall GetConferenceHall(int ConferenceHallID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            return base.Channel.GetConferenceHall(ConferenceHallID);
        }
    }

    public void AddConferenceHall(string Name, string Description, int OfficeID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.AddConferenceHall(Name, Description, OfficeID);
        }
    }

    public void EditConferenceHall(int ConferenceHallID, string Name, string Description, int OfficeID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.EditConferenceHall(ConferenceHallID, Name, Description, OfficeID);
        }
    }

    public void DeleteConferenceHall(int ConferenceHallID)
    {
        AuthIDOnlyHeader AuthIDOnlyHeader = new AuthIDOnlyHeader(m_UserID);
        MessageHeader<AuthIDOnlyHeader> header = new MessageHeader<AuthIDOnlyHeader>(AuthIDOnlyHeader);

        using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
            base.Channel.DeleteConferenceHall(ConferenceHallID);
        }
    }
}
