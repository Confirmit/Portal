<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="UserEvents" Codebehind="UserEvents.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="UserEventsGrid" Src="~/Controls/Events/UserEventsGrid.ascx" %>
<%@ Register TagPrefix="uc2" TagName="EventInfo" Src="~/Controls/Events/EventInfo.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">

    <div style="width: 510px;">
        <uc1:UserEventsGrid ID="userEventsGrid" runat="server" />
    </div>
    
    <div class="control-line-between"></div>
    <div style="width: 510px;">
        <uc2:EventInfo ID="userEventInfo" runat="server" />
    </div>

</asp:Content>