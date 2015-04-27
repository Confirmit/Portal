<%@ Control Language="C#" AutoEventWireup="true" Inherits="Calendar" Codebehind="Calendar.ascx.cs" %>

<asp:ImageButton runat="Server" ID="imgCalendar" 
    ImageUrl="~/images/calendar.png" 
    AlternateText="Click to show calendar" />

<ajaxToolkit:CalendarExtender ID="calendar" runat="server" PopupButtonID="imgCalendar" />