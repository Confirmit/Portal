<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsEditingPage.aspx.cs" Inherits="Portal.Admin.AdminGroupEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/GroupsControls/GroupCreatorControl.ascx" TagPrefix="uc" TagName="GroupCreatorControl" %>
<%@ Register Src="~/Controls/GroupsControls/AdminGroupsEditingControl.ascx" TagPrefix="uc" TagName="GroupsEditingControl" %>
<%@ Register Src="~/Controls/GroupsControls/UsersGroupManipulationControl.ascx" TagPrefix="uc" TagName="UsersGroupManipulationControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <uc:GroupCreatorControl ID="GroupCreator" runat="server" />
    <div style="width: 100%;">
        <uc:GroupsEditingControl ID="ControlForEditingGroups" runat="server" />
    </div>
    <div style="width: 100%;">
        <uc:UsersGroupManipulationControl ID="UsersGroupManipulationControl" runat="server" />
    </div>
</asp:Content>

