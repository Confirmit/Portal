<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsListPage.aspx.cs" Inherits="Portal.Admin.AdminGroupsListPage" MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/GroupsControls/GroupsListForEditingControl.ascx" TagPrefix="uc" TagName="GroupsListForEditingControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div style="margin: 5px;">
        <asp:Button ID="AddNewGroupButton" runat="server" Text="Add Group"/>
    </div>
    <uc:GroupsListForEditingControl ID="GroupsListForEditingControl" runat="server"/>
</asp:Content>

