<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsEditingPage.aspx.cs" Inherits="Portal.Admin.AdminGroupEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/AdminGroupsEditingControl.ascx" TagPrefix="grp" TagName="GroupsEditingControl" %>
<%@ Register Src="~/Controls/GroupsControls/UserGroupsSelectionControl.ascx" TagPrefix="grpslc" TagName="UserGroupsSelectionControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
     <grp:GroupsEditingControl ID="ControlForEditingGroups" runat="server" />
    <grpslc:UserGroupsSelectionControl ID="UsersListForCurrentGroupControl" runat="server" />
</asp:Content>

