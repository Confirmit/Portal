<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsEditingPage.aspx.cs" Inherits="Portal.Admin.AdminGroupEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/AdminGroupsEditingControl.ascx" TagPrefix="uc" TagName="GroupsEditingControl" %>
<%@ Register Src="~/Controls/GroupsControls/UserListInGroupControl.ascx" TagPrefix="uc" TagName="UserGroupsSelectionControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <uc:GroupsEditingControl ID="ControlForEditingGroups" runat="server" />
    <uc:UserGroupsSelectionControl ID="UsersListForCurrentGroupControl" runat="server" />
</asp:Content>

