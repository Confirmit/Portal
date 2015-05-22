<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="AdminEventsPage.aspx.cs" Inherits="Admin_AdminPage" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Controls/Users/UsersList.ascx" TagPrefix="uc1" TagName="UsersList" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
	<uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
   <asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
   
   <table width="100%">
		<tr>
			<td align="center">
				<asp:Calendar ID="Calendar" runat="server" Width="90%" OnSelectionChanged="ReportDateChanged"/>
			</td>
		</tr>
		<tr>
			<td align="center">
				<uc1:UsersList ID="usersList" Width="90%" runat="server" ControlMode="Admin" AdminNavigateURL="~/Admin/AdminUserInfo.aspx" />
			</td>
		</tr>
   </table>
   
</asp:Content>