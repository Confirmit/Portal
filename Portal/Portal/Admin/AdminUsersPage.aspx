<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
	Inherits="Admin_AdminUsersPage" CodeBehind="AdminUsersPage.aspx.cs" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Admin/UserInfo.ascx" TagPrefix="uc1" TagName="UserInfo" %>
<%@ Register Src="~/Controls/Users/UsersGrid.ascx" TagPrefix="uc1" TagName="UsersGrid" %>
<%@ Register Src="~/Controls/Users/UsersFilter.ascx" TagPrefix="uc1" TagName="UsersFilter" %>
<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
	<uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
	
	<div style="width: 50%">
		<uc1:UsersFilter ID="usersFilter" runat="server" />
		
		<div class="control-line-between"></div>
		<uc1:UsersGrid ID="userGrid" runat="server" />
		
		<asp:UpdatePanel ID="upFull" runat="server" ChildrenAsTriggers="true">
			<ContentTemplate>
				<asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
				<asp:PlaceHolder ID="plhUsersList" runat="server">
					<div style="float: right; padding-top: 4px; padding-bottom: 4px;">
						<asp:Button Width="150px" CssClass="control-button" ID="btnNewUser" runat="server"
							OnClick="createNewUser" meta:resourcekey="btnNewUser" TabIndex="1" UseSubmitBehavior="false" />
					</div>
				</asp:PlaceHolder>
				<uc1:UserInfo ID="userInfo" runat="server" OnApply="applyUserInfo" OnCancel="cancelUserInfo" />
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="userInfo" EventName="Apply" />
				<asp:AsyncPostBackTrigger ControlID="userInfo" EventName="Cancel" />
			</Triggers>
		</asp:UpdatePanel>
	</div>
</asp:Content>