<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsEditingPage.aspx.cs" Inherits="Portal.Admin.AdminGroupEditingPage" MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/EntitiesManipulationControls/EntitiesManipulationControl.ascx" TagPrefix="uc" TagName="EntitiesManipulationControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <a href="/Admin/AdminGroupsListPage.aspx" style="color: #000000;">Link for redirection to 'AdminGroupsListPage'</a>
    <asp:PlaceHolder ID="GroupConfigurationPlaceHolder" runat="server" />
    <div style="margin: 5px;">
        <uc:EntitiesManipulationControl ID="UsersManipulationControl" runat="server"/>
    </div>
</asp:Content>

