<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminMenu.ascx.cs" Inherits="Admin_AdminMenul" %>

<%@ Register TagName="ImageLinkButton" TagPrefix="uc1" Src="~/Controls/ImageLinkButton.ascx" %>

<br />
<div style="float: left; padding-left: 40px;">
    <div style="float: left;">
        <uc1:ImageLinkButton ID="hlEvents" runat="server"
            ImageUrl="~/Images/admin/events.gif"
            ImageWidth="30"
            Href="~/Admin/AdminEventsPage.aspx" />
    </div>
    
    <div style="float: left;">
        <uc1:ImageLinkButton ID="hlUsers" runat="server"
            ImageUrl="~/Images/admin/users.gif"
            ImageWidth="30"
            Href="~/Admin/AdminUsersPage.aspx" />
    </div>
	
	<div style="float: left;">
        <uc1:ImageLinkButton ID="hlStatistics" runat="server"
            ImageUrl="~/Images/admin/pie_chart.gif"
            ImageWidth="30"
            Href="~/Admin/AdminStatisticsPage.aspx" />
   </div>
</div>   
<br />     