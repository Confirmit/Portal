<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminOperations.aspx.cs" Inherits="Admin_AdminOperations" MasterPageFile="~/MasterPages/Main.master" meta:resourcekey="PageResource1" %>

<%@ Register TagName="ImageLinkButton" TagPrefix="uc1" Src="~/Controls/ImageLinkButton.ascx" %>
<%@ Register TagName="DescriptionExtender" TagPrefix="uc2" Src="~/Controls/AjaxControls/DescriptionExtender.ascx" %>

<asp:Content ID="cntntMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">	
	<div class="sectionCaption">
		<asp:Label ID="lblCaption" runat="server" meta:resourcekey="Caption" />
	</div>
	
	<table border="0px" cellspacing="0">
	    <tr style="display: none;">
	        <td align="left">
                <uc1:ImageLinkButton ID="hlSettings" runat="server"
                    ImageUrl="~/Images/admin/settings.png"
                    ImageWidth="30"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminGlobalSettingsPage.aspx" 
                    meta:resourcekey="Settings" 
                    Visible="false" />
            </td>
            <td>
                <asp:Image ID="imgSettings" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="SettingsDescriptionExtender" runat="server"
                    TargetControlID="imgSettings"
                    MoveHorizontal="30"
                    MoveVertical="-50"
                    meta:resourcekey="SettingsDescription" />
            </td>
        </tr>	
	    <tr>
	        <td align="left">
                <uc1:ImageLinkButton ID="hlEvents" runat="server"
                    ImageUrl="~/Images/admin/events.gif"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminEventsPage.aspx" 
                    meta:resourcekey="Events" />
            </td>
            <td>
                <asp:Image ID="imgEvents" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="desciptorExtenderEvents" runat="server"
                    TargetControlID="imgEvents"
                    MoveHorizontal="30"
                    MoveVertical="-50"
                    meta:resourcekey="EventsDescription" />
            </td>
       </tr>
       <tr>
	        <td align="left">
                <uc1:ImageLinkButton ID="hlArrangements" runat="server"
                    ImageUrl="~/Images/admin/events.gif"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminArrangements/AdminArrangementsPage.aspx" 
                    meta:resourcekey="Arrangements"
                />
            </td>
            <td>
                <asp:Image ID="imgArrangements" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="DescriptionExtender1" runat="server"
                    TargetControlID="imgArrangements"
                    MoveHorizontal="30"
                    MoveVertical="-50"
                    meta:resourcekey="ArrangementsDescription" 
                />
            </td>
       </tr>
       <tr>
            <td align="left">
                <uc1:ImageLinkButton ID="hlUsers" runat="server"
                    ImageUrl="~/Images/admin/user.gif"
                    ImageWidth="35"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminUserOperations.aspx" 
                    meta:resourcekey="Users" 
                />
            </td>
            <td>
                <asp:Image ID="imgUsers" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="desciptorExtenderUsers" runat="server" 
                    TargetControlID="imgUsers" 
                    MoveHorizontal="40"
                    MoveVertical="-30" 
                    meta:resourcekey="UsersDescription" 
                />
           </td>
      </tr>
      <tr>
            <td align="left">
                <uc1:ImageLinkButton ID="hlStatistics" runat="server"
                     ImageUrl="~/Images/admin/graph.gif"
                    ImageWidth="35"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminStatisticsPage.aspx" 
                    meta:resourcekey="Statistics"
                />
            </td>
            <td>
                <asp:Image ID="imgStatistics" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description" 
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="descriptionExtenderStatistics" 
                    runat="server" TargetControlID="imgStatistics" 
                    MoveHorizontal="50"
                    MoveVertical="-5" 
                    meta:resourcekey="StatisticsDescription"
                />
            </td>
       </tr>
       <tr>
            <td align="left">
                <uc1:ImageLinkButton ID="hlCalendar" runat="server"
                    ImageUrl="~/Images/admin/calendar.gif"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminCalendarPage.aspx" 
                    meta:resourcekey="Calendar"
                />
            </td>
            <td>
                <asp:Image ID="imgCalendar" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description" 
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="descriptionExtenderCalendar" 
                    runat="server" TargetControlID="imgCalendar"
                    MoveHorizontal="60"
                    MoveVertical="5" 
                    meta:resourcekey="CalendarDescription"
                />
            </td>
      </tr>
      <tr>
            <td align="left">
                <uc1:ImageLinkButton ID="hlAttributesTypes" runat="server"
                    ImageUrl="~/Images/admin/user_attr.png"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminUserAttributes.aspx" 
                    meta:resourcekey="PersonAttributes"
                />
            </td>
            <td>
                <asp:Image ID="imgPersonAttr" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20px" 
                    ToolTip="Description" 
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="descrExtPersonAttr" 
                    runat="server" TargetControlID="imgPersonAttr"
                    MoveHorizontal="60"
                    MoveVertical="5" 
                    meta:resourcekey="PersonAttributesDescription"
                />
            </td>
      </tr>
</table>
</asp:Content>