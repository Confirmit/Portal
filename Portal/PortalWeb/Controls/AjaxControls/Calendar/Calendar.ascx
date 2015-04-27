<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Calendar.ascx.cs" Inherits="Calendar" %>

<asp:ImageButton runat="Server" ID="imgCalendar" 
    ImageUrl="~/images/calendar.png" 
    AlternateText="Click to show calendar" 
/>
<ajaxToolkit:CalendarExtender ID="calendar" runat="server"
    PopupButtonID="imgCalendar"
    ScriptPath="Calendar.js"
/> 